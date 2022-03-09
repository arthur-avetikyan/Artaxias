
using Artaxias.Web.Common.DataTransferObjects.Account;
using Artaxias.Web.Common.DataTransferObjects.UserManagement;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface IUsersApiAccessor
    {
        Task<UserDetails> GetAsync(string userName);

        Task<List<UserDetails>> GetListAsync(int pageSize = 10, int currentPage = 0, bool unemployeed = false);

        Task<string> CreateAsync(RegisterParameters parameters);

        Task<UserDetails> UpdateAsync(UserDetails userDetails);

        Task DeleteAsync(string userName);

        Task ChangePasswordAsync(ChangePasswordParameters parameters);

        Task<UserInfo> GetUserInfoAsync(string userName);
    }
}
