using Artaxias.Web.Common.DataTransferObjects.Feadback;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface IFeedbackApiAccessor
    {
        Task<List<FeedbackDetails>> GeAllFeedbacks(int pageSize = 10, int currentPage = 0);

        Task<List<FeedbackAboutEmployeeResponse>> GeFeedbackAboutEmployeeAsync(int employeeId, int reviewId);

        Task<ProvideFeedbackResponse> GetProvideFeedbackAsync(string reviewerUserName, int reviewerRevieweeId);

        Task<string> ProvideFeedbackAsync(ProvideFeedbackRequest request);
    }
}
