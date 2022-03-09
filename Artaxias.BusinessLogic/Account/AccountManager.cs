using Artaxias.BusinessLogic.FileManagement;
using Artaxias.BusinessLogic.Users;
using Artaxias.Core.Configurations;
using Artaxias.Core.Enums;
using Artaxias.Core.Exceptions;
using Artaxias.Data;
using Artaxias.Data.Models;
using Artaxias.Mailing;
using Artaxias.Models;
using Artaxias.Web.Common.DataTransferObjects.Account;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Artaxias.BusinessLogic.Account
{
    public class AccountManager : IAccountManager
    {
        private readonly ApplicationConfiguration _applicationConfiguration;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;

        private readonly IAuthenticationStateManager _authenticationStateManager;
        private readonly IFileManager _fileManager;
        private readonly IEmailProcessor _emailProcessor;

        public AccountManager(
            UserManager<User> userManager,
            IRepository<User> userRepository,
            IRepository<RefreshToken> refreshTokenRepository,
            IAuthenticationStateManager authenticationManager,
            IOptions<ApplicationConfiguration> applicationConfiguration,
            IFileManager fileManager,
            IEmailProcessor emailProcessor,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticationStateManager = authenticationManager;
            _fileManager = fileManager;
            _emailProcessor = emailProcessor;
            _signInManager = signInManager;
            _applicationConfiguration = applicationConfiguration.Value;
        }

        public async Task<AuthenticationResult> Login(LoginRequest parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.UserName))
            {
                throw new ArgumentNullException(nameof(parameters.UserName), "Username is invalid.");
            }
            var result = await _userRepository
                .Get(u => u.UserName.Equals(parameters.UserName))
                .Select(item =>
                    new
                    {
                        User = item,
                        IsEmailConfirmed = item.EmailConfirmed,
                        UserRoles = item.UserRoles.Select(userRole => userRole.Role.Name)
                    })
                .FirstOrDefaultAsync();
            bool isValidLogin = false;
            if (result != null)
            {
                isValidLogin = await _userManager.CheckPasswordAsync(result.User, parameters.Password);
            }

            if (!isValidLogin)
            {
                throw new ArgumentNullException(nameof(isValidLogin), "Username or password are invalid.");
            }

            if (!result.IsEmailConfirmed)
            {
                throw new ArgumentException("Your account has not been confirmed, please confirm it first.");
            }

            IEnumerable<Claim> claims = _authenticationStateManager.GetClaims(result.User, result.UserRoles);
            AuthenticationResult authenticationResult = await _authenticationStateManager.Authenticate(result.User.Id, null, claims);

            return authenticationResult;
        }

        public async Task<UserDetails> Register(RegisterParameters parameters)
        {
            User user = await RegisterNewUserAsync(parameters);
            return user.Map();
        }

        private async Task<User> RegisterNewUserAsync(RegisterParameters parameters)
        {
            User user = parameters.Map();

            IdentityResult createUserResult = await _userManager.CreateAsync(user, parameters.Password);
            if (!createUserResult.Succeeded)
            {
                throw new DomainException(string.Join(",", createUserResult.Errors.Select(i => i.Description)));
            }

            await _userManager.AddClaimsAsync(user, new[]{
                    new Claim(nameof(EAuthorizationRoles.User), string.Empty),
                    new Claim(nameof(user.UserName), user.UserName),
                    new Claim(nameof(user.Email), user.Email),
                    new Claim(nameof(user.EmailConfirmed), "false", ClaimValueTypes.Boolean)
                });

            //Role - Here we tie the new user to the "User" role
            await _userManager.AddToRolesAsync(user, new List<string> { nameof(EAuthorizationRoles.User) });
            //_logger.LogInformation("New user registered: {0}", user);

            EmailConfigurationParameters emailMessage = new();

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
            string token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
            string callbackUrl = $"{_applicationConfiguration.Url}/Account/{user.UserName}/ConfirmEmail/{token}";

            string emailTemplate = _fileManager.ReadFile(
                Path.Combine(_applicationConfiguration.EmailTemplatesFilePath, "NewUserConfirmationEmail.template.html"));

            ConfirmationEmailParameters emailMessageBuilderParameters = new()
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
            try
            {
                await _emailProcessor.SendAsync(emailMessage);
            }
            catch (Exception)
            {
                //_logger.LogInformation("New user email failed: Body: {0}, Error: {1}", emailMessage.Body, ex.Message);
            }

            return user;
        }

        public async Task<AuthenticationResult> Refresh(AuthenticationResult authenticationResult)
        {
            if (authenticationResult is null || string.IsNullOrWhiteSpace(authenticationResult.RefreshToken))
            {
                throw new ArgumentNullException(nameof(authenticationResult), "Invalid operation");
            }

            var authModel = await _refreshTokenRepository
                .Get(t => t.Revoked == null)
                .Where(t => t.Expires > DateTime.UtcNow)
                .Where(w => w.Token.Equals(authenticationResult.RefreshToken))
                .Select(token =>
                    new
                    {
                        RefrshTokenId = token.Id,
                        token.User,
                        UserRoles = token.User.UserRoles.Select(userRole => userRole.Role.Name)
                    })
                .FirstOrDefaultAsync();

            if (authModel == null)
            {
                throw new ArgumentException("User not found");
            }

            IEnumerable<Claim> claims = _authenticationStateManager.GetClaims(authModel.User, authModel.UserRoles);
            return await _authenticationStateManager.Authenticate(authModel.User.Id, authModel.RefrshTokenId, claims);
        }

        public async Task Revoke(string userName)
        {
            User user = await _userRepository.Get(u => u.UserName.Equals(userName))
                    .Include(i => i.RefreshTokens)
                    .SingleOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            foreach (RefreshToken token in user.RefreshTokens.Where(x => x.IsActive))
            {
                token.Revoked = DateTime.UtcNow.AddMinutes(-1);
            }
            await _userRepository.SaveChangesAsync();
        }

        public async Task ConfirmEmail(ConfirmEmailRequest confirmEmailRequest)
        {
            User user = await _userManager.FindByNameAsync(confirmEmailRequest.UserName);
            if (user == null)
            {
                throw new ArgumentException($"User - {nameof(confirmEmailRequest.UserName)} does not exist.");
            }

            string token = confirmEmailRequest.Token;
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new DomainException("User Email Confirmation Failed: {0}",
                    string.Join(",", result.Errors.Select(i => i.Description)));
            }

            await _signInManager.SignInAsync(user, true);
        }

        public Task<ForgotPasswordParameters> ForgotPassword(ForgotPasswordParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}