using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class AbsencesApiAccessor : IAbsencesApiAccessor
    {
        private readonly HttpClient _httpClient;

        public AbsencesApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AbsenceResponse>> GetAllAsync(int employeeId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/absences?employeeId={employeeId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AbsenceResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<AbsenceResponse> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/absences/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AbsenceResponse>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> CreateAsync(AbsenceRequest absenceRequest)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/absences", absenceRequest);
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

        public async Task<AbsenceResponse> UpdateAsync(int id, AbsenceRequest absenceRequest)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/absences/{id}", absenceRequest);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AbsenceResponse>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"api/absences/{id}");
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<AbsenceTypeResponse>> GetAllTypesAsync(int pageSize = int.MaxValue)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/absences/types?pageSize={pageSize}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AbsenceTypeResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> ApproveAsync(int id, int manageId)
        {
            HttpResponseMessage response = await _httpClient.PutAsync($"api/absences/{id}/approve/{manageId}", null);
            string message = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return message;
            }
            else
            {
                throw new Exception(message);
            }
        }

        public async Task<string> RejectAsync(int id, int manageId)
        {
            HttpResponseMessage response = await _httpClient.PutAsync($"api/absences/{id}/reject/{manageId}", null);
            string message = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return message;
            }
            else
            {
                throw new Exception(message);
            }
        }
    }
}
