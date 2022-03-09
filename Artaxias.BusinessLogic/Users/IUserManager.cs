using Artaxias.Web.Common.DataTransferObjects.Account;
using Artaxias.Web.Common.DataTransferObjects.UserManagement;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Users
{
    public interface IUserManager : IManager<string, RegisterParameters, UserDetails>
    {
        //Task<ApiResponse<UserDetails>> GetAsync(string userName);

        Task ChangePassword(ChangePasswordParameters parameters);

        Task<List<UserDetails>> GetUnemployeedUsers();

        Task<UserInfo> GetUserInfoAsync(string userName);

        Task<UserDetails> UpdateAsync(UserDetails userDetails);
    }
}
