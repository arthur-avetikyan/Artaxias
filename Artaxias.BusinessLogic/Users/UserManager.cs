
using Artaxias.BusinessLogic.FileManagement;
using Artaxias.Core.Configurations;
using Artaxias.Core.Enums;
using Artaxias.Core.Exceptions;
using Artaxias.Data;
using Artaxias.Mailing;
using Artaxias.Models;
using Artaxias.Web.Common.DataTransferObjects.Account;
using Artaxias.Web.Common.DataTransferObjects.UserManagement;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Artaxias.BusinessLogic.Users
{
    public class UserManager : IUserManager
    {
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<User> _userRepository;
        private readonly IFileManager _fileContentProvider;
        private readonly IEmailProcessor _emailProcessor;

        public UserManager(UserManager<User> userManager,
            IRepository<User> userRepository,
            IOptions<ApplicationConfiguration> applicationConfiguration,
            IFileManager fileContentProvider,
            IEmailProcessor emailProcessor)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _fileContentProvider = fileContentProvider;
            _applicationConfiguration = applicationConfiguration.Value;
            _emailProcessor = emailProcessor;
        }

        public async Task<UserInfo> GetUserInfoAsync(string userName)
        {
            UserInfo userInfo = await _userRepository.Get(u => u.UserName == userName)
                                                .Select(u => new UserInfo
                                                {
                                                    Id = u.Id,
                                                    UserName = u.UserName,
                                                    FirstName = u.FirstName,
                                                    LastName = u.LastName,
                                                    EmployeeId = u.Employee == null ? 0 : u.Employee.Id,
                                                    DeparmentIds = u.Employee.Departments.Select(ed => ed.DepartmentId),
                                                    Role = u.UserRoles.Select(ur => ur.Role.Name)
                                                })
                                                .FirstOrDefaultAsync();
            return userInfo;
        }

        public async Task<UserDetails> GetAsync(string userName)
        {
            User user = _userRepository.Get().FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                throw new ArgumentException("No such user exists!");
            }

            return new UserDetails
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Roles = await _userManager.GetRolesAsync(user).ConfigureAwait(true) as List<string>
            };
        }

        public async Task<List<UserDetails>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            //var userList = _userManager.Users.AsQueryable();
            List<User> usersList = _userManager.Users.OrderBy(x => x.Id).Skip(currentPage * pageSize).Take(pageSize).ToList();

            List<UserDetails> resultList = new List<UserDetails>();
            foreach (User user in usersList)
            {
                resultList.Add(new UserDetails
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Id = user.Id,
                    Roles = await _userManager.GetRolesAsync(user).ConfigureAwait(true) as List<string>
                });
            }

            return resultList;
        }

        public async Task<UserDetails> UpdateAsync(UserDetails userDetails)
        {
            User user = await _userManager.FindByIdAsync(userDetails.Id.ToString());

            if (user == null)
            {
                throw new ArgumentException("User does not exist");
            }

            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;
            user.UserName = userDetails.UserName;
            user.Email = userDetails.Email;

            IdentityResult updateResult = await _userManager.UpdateAsync(user);
            IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

            IEnumerable<string> rolesToAdd = userDetails.Roles.Except(roles);
            IEnumerable<string> rolesToRemove = roles.Except(userDetails.Roles);

            IdentityResult addRolesResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
            IdentityResult deleteRolesResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            if (!updateResult.Succeeded || !addRolesResult.Succeeded || !deleteRolesResult.Succeeded)
            {
                throw new ArgumentException("User Update Failed");
            }

            return user.Map();
        }

        public async Task DeleteAsync(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new ArgumentException("User does not exist");
            }

            await _userManager.DeleteAsync(user);
        }

        public async Task ChangePassword(ChangePasswordParameters parameters)
        {
            User user = await _userManager.FindByNameAsync(parameters.UserName);
            if (user == null)
            {
                throw new ArgumentNullException($"Unable to find user {parameters.UserName}");
            }

            bool isOldPasswordCorrect = await _userManager.CheckPasswordAsync(user, parameters.OldPassword);
            if (!isOldPasswordCorrect)
            {
                throw new ArgumentException("The old password is incorrect");
            }

            string passToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, passToken, parameters.NewPassword);

            if (!result.Succeeded)
            {
                if (result.Errors.Any())
                {
                    throw new Exception(string.Join(',', result.Errors.Select(x => x.Description)));
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        public async Task<UserDetails> CreateAsync(RegisterParameters parameters)
        {
            User user = await RegisterNewUserAsync(parameters);
            return user.Map();
        }

        public async Task<List<UserDetails>> GetUnemployeedUsers()
        {
            return await _userRepository.Get(u => u.Employee == null)
                                        .Select(u => new UserDetails
                                        {
                                            Id = u.Id,
                                            FirstName = u.FirstName,
                                            LastName = u.LastName
                                        })
                                        .ToListAsync();
        }

        /// Generates a Random Password
        /// respecting the given strength requirements.
        /// </summary>
        /// <param name="opts">A valid PasswordOptions object
        /// containing the password strength requirements.</param>
        /// <returns>A random password</returns>
        private static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null)
            {
                opts = new PasswordOptions()
                {
                    RequiredLength = 8,
                    RequiredUniqueChars = 4,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonAlphanumeric = true,
                    RequireUppercase = true
                };
            }

            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                "0123456789",                   // digits
                "!@$?_-"                        // non-alphanumeric
            };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);
            }

            if (opts.RequireLowercase)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);
            }

            if (opts.RequireDigit)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);
            }

            if (opts.RequireNonAlphanumeric)
            {
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);
            }

            for (int i = chars.Count; i < opts.RequiredLength
                                      || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        private async Task<User> RegisterNewUserAsync(RegisterParameters parameters)
        {
            User user = parameters.Map();

            IdentityResult createUserResult;
            UserRegistrationGeneratedPasswordEmail credentialsEmail = null;
            if (parameters.Password == null)
            {
                string tempPassword = GenerateRandomPassword();
                createUserResult = await _userManager.CreateAsync(user, tempPassword);

                string credentialsEmailTemplate = _fileContentProvider.ReadFile(
                    Path.Combine(_applicationConfiguration.EmailTemplatesFilePath, "NewUserCredentialsGeneratedEmail.template.html"));

                credentialsEmail = new UserRegistrationGeneratedPasswordEmail
                {
                    UserName = user.UserName,
                    Password = tempPassword,
                    Url = _applicationConfiguration.Url,
                    Template = credentialsEmailTemplate
                };
            }
            else
            {
                createUserResult = await _userManager.CreateAsync(user, parameters.Password);
            }

            if (!createUserResult.Succeeded)
            {
                throw new DomainException(string.Join(",", createUserResult.Errors.Select(i => i.Description)));
            }

            await _userManager.AddToRolesAsync(user, new List<string> { nameof(EAuthorizationRoles.User) });
            //_logger.LogInformation("New user registered: {0}", user);

            EmailConfigurationParameters emailMessage = new EmailConfigurationParameters();
            if (credentialsEmail != null)
            {
                emailMessage.BuildNewUserCredentialsGeneratedEmail(credentialsEmail);
                emailMessage.DestinationAddresses.Add(user.Email);
                await _emailProcessor.SendAsync(emailMessage);
            }

            string token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
            string callbackUrl = $"{_applicationConfiguration.Url}/Account/{user.UserName}/ConfirmEmail/{token}";

            string emailTemplate = _fileContentProvider.ReadFile(
                Path.Combine(_applicationConfiguration.EmailTemplatesFilePath, "NewUserConfirmationEmail.template.html"));

            ConfirmationEmailParameters emailMessageBuilderParameters = new ConfirmationEmailParameters
            {
                UserName = user.UserName,
                Recipient = user.Email,
                CallbackUrl = callbackUrl,
                UserId = user.Id.ToString(),
                Token = token,
                Template = emailTemplate
            };

            emailMessage.BuildNewUserConfirmationEmail(emailMessageBuilderParameters); //Replace First UserName with Name if you want to add name to Registration Form

            emailMessage.DestinationAddresses.Add(user.Email);
            await _emailProcessor.SendAsync(emailMessage);

            return user;
        }


        public Task<UserDetails> UpdateAsync(string key, RegisterParameters request)
        {
            throw new NotImplementedException();
        }
    }
}
