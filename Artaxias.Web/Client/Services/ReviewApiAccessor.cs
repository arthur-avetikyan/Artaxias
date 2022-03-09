using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Feadback;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class ReviewApiAccessor : IReviewApiAccessor
    {
        private readonly HttpClient _httpClient;

        public ReviewApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CreateReviewAsync(ReviewRequest request)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync($"api/reviews", request);
            string message = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return message;
            }
            else
            {
                throw new Exception(message);
            }
        }

        public async Task<ReviewResponse> UpdateReviewAsync(int reviewId, ReviewRequest request)
        {
            HttpResponseMessage result = await _httpClient.PutAsJsonAsync($"api/reviews/{reviewId}", request);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<ReviewResponse>();
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<ReviewResponse>> GetReviewsAsync(int pageSize = 10, int currentPage = 0)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/reviews?pageSize={pageSize}&pageNumber={currentPage}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ReviewResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<ReviewResponse> GetReviewAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/reviews/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ReviewResponse>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task DeleteReviewAsync(int id)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"api/reviews/{id}");
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<ReviewAboutEmployeeResponse>> GetReviewsAboutEmployeeAsync(int employeeId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/reviews/about/{employeeId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ReviewAboutEmployeeResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
