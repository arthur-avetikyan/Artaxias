
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Organization.Absences
{
    public interface IAbsencesManager : IManager<int, AbsenceRequest, AbsenceResponse>
    {
        Task<List<AbsenceResponse>> GetListAsync(int employeeId, int pageSize = 10, int currentPage = 0);

        Task<List<AbsenceTypeResponse>> GetAllTypesAsync(int pageSize = 10, int currentPage = 0);

        Task ApproveAsync(int id, int manageId);

        Task RejectAsync(int id, int manageId);
    }
}
