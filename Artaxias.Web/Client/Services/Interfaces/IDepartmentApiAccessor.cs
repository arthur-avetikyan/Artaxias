using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface IDepartmentApiAccessor
    {
        Task<List<DepartmentResponse>> GetDepartmentListAsync(int pageSize = 10, int currentPage = 0);

        Task<DepartmentResponse> GetDepartmentAsync(int departmentId);

        Task<List<EmployeeResponse>> GetEmployees();

        Task DeleteDepartmentAsync(int currentDepartmentId);

        Task<string> CreateDepartmentAsync(DepartmentRequest department);

        Task<DepartmentResponse> UpdateDepartmentAsync(DepartmentRequest department);
    }
}
