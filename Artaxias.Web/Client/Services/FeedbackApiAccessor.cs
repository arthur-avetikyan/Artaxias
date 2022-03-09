using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Feadback;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class FeedbackApiAccessor : IFeedbackApiAccessor
    {
        private readonly HttpClient _httpClient;

        public FeedbackApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProvideFeedbackResponse> GetProvideFeedbackAsync(string reviewerUserName, int reviewerRevieweeId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/feedbacks/{reviewerUserName}/{reviewerRevieweeId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProvideFeedbackResponse>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<FeedbackAboutEmployeeResponse>> GeFeedbackAboutEmployeeAsync(int employeeId, int reviewId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/feedbacks/{employeeId}?reviewId={reviewId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<FeedbackAboutEmployeeResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<FeedbackDetails>> GeAllFeedbacks(int pageSize = 10, int currentPage = 0)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/feedbacks?pageSize={pageSize}&pageNumber={currentPage}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<FeedbackDetails>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> ProvideFeedbackAsync(ProvideFeedbackRequest request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/feedbacks", request);
            string result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                throw new Exception(result);
            }
        }
    }
}