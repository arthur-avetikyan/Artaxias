using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class EmployeesApiAccessor : IEmployeesApiAccessor
    {
        private readonly HttpClient _httpClient;

        public EmployeesApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EmployeeResponse>> GetEmployeesListAsync(int pageSize = 10, int currentPage = 0)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/employees?pageSize={pageSize}&pageNumber={currentPage}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<EmployeeResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<EmployeeResponse> GetEmployeeAsync(int employeeId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/employees/{employeeId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<EmployeeResponse>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<EmployeeResponse> UpdateEmployeeAsync(EmployeeRequest employeeRequest)
        {
            HttpResponseMessage result = await _httpClient.PutAsJsonAsync($"api/employees/{employeeRequest.Id}", employeeRequest);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<EmployeeResponse>();
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> CreateEmployeeAsync(EmployeeRequest employeeRequest)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync($"api/employees", employeeRequest);
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

        public async Task<EmployeeResponse> EndContractAsync(EndContractRequest endContractRequest)
        {
            HttpResponseMessage result = await _httpClient.PutAsJsonAsync($"api/employees/{endContractRequest.EmployeeId}/endContract", endContractRequest);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<EmployeeResponse>();
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task DeleteEmployee(int employeeId)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"api/employees/{employeeId}");
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }
    }
}
