using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class DepartmentApiAccessor : IDepartmentApiAccessor
    {
        private readonly HttpClient _httpClient;
        private readonly IEmployeesApiAccessor _employeesApiAccessor;

        public DepartmentApiAccessor(HttpClient httpClient, IEmployeesApiAccessor employeesApiAccessor)
        {
            _httpClient = httpClient;
            _employeesApiAccessor = employeesApiAccessor;
        }

        public async Task<DepartmentResponse> GetDepartmentAsync(int departmentId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/departments/{departmentId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<DepartmentResponse>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<EmployeeResponse>> GetEmployees()
        {
            return await _employeesApiAccessor.GetEmployeesListAsync(int.MaxValue, 0);
        }

        public async Task<List<DepartmentResponse>> GetDepartmentListAsync(int pageSize = 10, int currentPage = 0)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/departments?pageSize={pageSize}&pageNumber={currentPage}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DepartmentResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task DeleteDepartmentAsync(int departmentId)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"api/departments/{departmentId}");
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> CreateDepartmentAsync(DepartmentRequest department)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync("api/departments", department);
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

        public async Task<DepartmentResponse> UpdateDepartmentAsync(DepartmentRequest department)
        {
            HttpResponseMessage result = await _httpClient.PutAsJsonAsync($"api/departments/{department.Id}", department);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<DepartmentResponse>();
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }
    }
}
