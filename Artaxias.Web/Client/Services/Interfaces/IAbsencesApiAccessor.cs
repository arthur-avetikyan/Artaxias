
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface IAbsencesApiAccessor
    {
        public Task<List<AbsenceResponse>> GetAllAsync(int employeeId);

        public Task<AbsenceResponse> GetAsync(int id);

        public Task<string> CreateAsync(AbsenceRequest absenceRequest);

        public Task<AbsenceResponse> UpdateAsync(int id, AbsenceRequest absenceRequest);

        public Task DeleteAsync(int id);

        public Task<List<AbsenceTypeResponse>> GetAllTypesAsync(int pageSize = int.MaxValue);

        public Task<string> ApproveAsync(int id, int manageId);

        public Task<string> RejectAsync(int id, int manageId);
    }
}
