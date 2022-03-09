using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class SalariesApiAccessor : ISalariesApiAccessor
    {
        private readonly HttpClient _httpClient;

        public SalariesApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SalaryResponse>> GetSalariesListAsync(int employeeId, int pageSize = 10, int currentPage = 0)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/salaries/{employeeId}?pageSize={pageSize}&pageNumber={currentPage}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<SalaryResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> CreateSalaryAsync(SalaryRequest salaryRequest)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync($"api/salaries", salaryRequest);
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
    }
}
