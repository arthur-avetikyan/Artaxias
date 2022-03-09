using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface IBonusesApiAccessor
    {
        Task<BonusResponse> GetBonusAsync(int bonusId);
        Task<List<BonusResponse>> GetBonusesListForEmployeeAsync(int employeeId, int pageSize = 10, int currentPage = 0);
        Task<string> CreateBonusAsync(BonusRequest request);
        Task<BonusResponse> UpdateAsync(int id, BonusRequest request);
        Task DeleteAsync(int id);
    }
}
