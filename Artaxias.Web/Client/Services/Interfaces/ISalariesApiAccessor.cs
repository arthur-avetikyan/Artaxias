using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface ISalariesApiAccessor
    {
        Task<List<SalaryResponse>> GetSalariesListAsync(int employeeId, int pageSize = 10, int currentPage = 0);

        Task<string> CreateSalaryAsync(SalaryRequest salaryRequest);
    }
}
