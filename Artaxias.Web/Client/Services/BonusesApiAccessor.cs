using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class BonusesApiAccessor : IBonusesApiAccessor
    {
        private readonly HttpClient _httpClient;

        public BonusesApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BonusResponse>> GetBonusesListForEmployeeAsync(int employeeId, int pageSize = 10, int currentPage = 0)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/bonuses/employee/{employeeId}?pageSize={pageSize}&pageNumber={currentPage}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<BonusResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<BonusResponse> GetBonusAsync(int bonusId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/bonuses/{bonusId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BonusResponse>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> CreateBonusAsync(BonusRequest request)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync($"api/bonuses", request);
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

        public async Task<BonusResponse> UpdateAsync(int id, BonusRequest request)
        {
            HttpResponseMessage result = await _httpClient.PutAsJsonAsync($"api/bonuses/{id}", request);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<BonusResponse>();
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"api/bonuses/{id}");
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }
    }
}