using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.UserManagement;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class RoleApiAccessor : IRoleApiAccessor
    {
        private readonly HttpClient _httpClient;

        public RoleApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RoleResponse>> GetRolesAsync(int pageSize = 10, int currentPage = 0)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/roles?pageSize={pageSize}&pageNumber={currentPage}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<RoleResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<RoleResponse> GetRoleAsync(string roleName)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/roles/{roleName}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RoleResponse>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<PermissionResponse>> GetPremissions()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/permissions");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<PermissionResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> CreateRoleAsync(RoleRequest request)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync("api/roles", request);
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

        public async Task<RoleResponse> UpdateRoleAsync(int roleId, RoleRequest request)
        {
            HttpResponseMessage result = await _httpClient.PutAsJsonAsync($"api/roles/{roleId}", request);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<RoleResponse>();
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task DeleteRoleAsync(string roleName)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"api/roles/{roleName}");
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }
    }
}
