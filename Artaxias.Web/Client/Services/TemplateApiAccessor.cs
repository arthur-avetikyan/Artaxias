using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Feadback;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class TemplateApiAccessor : ITemplateApiAccessor
    {
        private readonly HttpClient _httpClient;

        public TemplateApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CreateFeadbackTemplateAsync(TemplateRequest request)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync($"api/templates", request);
            string text = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return text;
            }
            else
            {
                throw new Exception(text);
            }
        }

        public async Task<TemplateResponse> UpdateFeadBackTemplateAsync(TemplateRequest request)
        {
            HttpResponseMessage result = await _httpClient.PutAsJsonAsync($"api/templates/{request.Id}", request);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<TemplateResponse>();
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<TemplateResponse>> GetFeadbackTemplatesAsync(int pageSize = 10, int currentPage = 0)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/templates?pageSize={pageSize}&pageNumber={currentPage}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<TemplateResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<TemplateResponse> GetFeadbackTemplateAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/templates/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TemplateResponse>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task DeleteTemplateAsync(int id)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"api/templates/{id}");
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<AnswerOptionTypeDetails>> GetAnswerOptionTypes()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/templates/types");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AnswerOptionTypeDetails>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
