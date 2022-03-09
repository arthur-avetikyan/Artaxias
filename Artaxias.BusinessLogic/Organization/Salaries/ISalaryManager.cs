using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Organization.Salaries
{
    public interface ISalaryManager : IManager<int, SalaryRequest, SalaryResponse>
    {
        Task<List<SalaryResponse>> GetListAsync(int employeeId, int pageSize = 10, int currentPage = 0);
    }
}
