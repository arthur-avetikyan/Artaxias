using Artaxias.Web.Common.DataTransferObjects.UserManagement;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface IRoleApiAccessor
    {
        Task<string> CreateRoleAsync(RoleRequest request);

        Task DeleteRoleAsync(string roleName);

        Task<List<PermissionResponse>> GetPremissions();

        Task<RoleResponse> GetRoleAsync(string roleName);

        Task<List<RoleResponse>> GetRolesAsync(int pageSize = 10, int currentPage = 0);

        Task<RoleResponse> UpdateRoleAsync(int roleId, RoleRequest request);
    }
}
