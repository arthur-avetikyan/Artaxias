using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Organization.Bonuses
{
    public interface IBonusManager : IManager<int, BonusRequest, BonusResponse>
    {
        Task<List<BonusResponse>> GetBonusesListForEmployeeAsync(int employeeId, int pageSize = 10, int currentPage = 0);
    }
}
