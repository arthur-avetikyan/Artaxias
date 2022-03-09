using Artaxias.Web.Common.DataTransferObjects.Feadback;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface IReviewApiAccessor
    {
        Task<string> CreateReviewAsync(ReviewRequest request);

        Task<ReviewResponse> UpdateReviewAsync(int reviewId, ReviewRequest request);

        Task DeleteReviewAsync(int id);

        Task<ReviewResponse> GetReviewAsync(int id);

        Task<List<ReviewAboutEmployeeResponse>> GetReviewsAboutEmployeeAsync(int employeeId);

        Task<List<ReviewResponse>> GetReviewsAsync(int pageSize = 10, int currentPage = 0);
    }
}
