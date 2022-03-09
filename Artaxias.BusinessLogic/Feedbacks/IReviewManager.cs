using Artaxias.Web.Common.DataTransferObjects.Feadback;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Feedbacks
{
    public interface IReviewManager : IManager<int, ReviewRequest, ReviewResponse>
    {
        Task<ReviewResponse> CreateAsync(ReviewRequest request, string name);
        Task<List<ReviewAboutEmployeeResponse>> GetReviewsAboutEmployee(int employeeId);
    }
}
