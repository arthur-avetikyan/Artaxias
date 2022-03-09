using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface IEmployeesApiAccessor
    {
        Task<EmployeeResponse> GetEmployeeAsync(int employeeId);

        Task<List<EmployeeResponse>> GetEmployeesListAsync(int pageSize = 10, int currentPage = 0);

        Task<EmployeeResponse> UpdateEmployeeAsync(EmployeeRequest employeeRequest);

        Task<EmployeeResponse> EndContractAsync(EndContractRequest endContractRequest);

        Task<string> CreateEmployeeAsync(EmployeeRequest employeeRequest);

        Task DeleteEmployee(int employeeId);
    }
}
