using Artaxias.Web.Common.DataTransferObjects.Feadback;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Feedbacks
{
    public interface IFeedbackManager
    {
        Task<ProvideFeedbackResponse> CreateFeedbackAsync(ProvideFeedbackRequest request);
        Task<ProvideFeedbackResponse> GetFeedbackResponseAsync(int reviewerRevieweeId);
        Task<List<FeedbackAboutEmployeeResponse>> GetFeedbackAboutEmployee(int employeeId, int reviewId);
        Task<List<FeedbackDetails>> GetListAsync(int pageSize, int pageNumber);
    }
}