using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.File;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class ContractApiAccessor : IContractApiAccessor
    {
        private readonly HttpClient _httpClient;

        public ContractApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CreateTemplateAsync(ContractTemplateRequest request)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync($"api/contracts", request);
            string message = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return message;
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<ContractTemplateResponse>> GetTemplatesListAsync(int pageSize = 10, int currentPage = 0)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/contracts?pageSize={pageSize}&pageNumber={currentPage}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ContractTemplateResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<ContractTemplateResponse> GetTemplateAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/contracts/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ContractTemplateResponse>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task DeleteTemplateAsync(int id)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"api/contracts/{id}");
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<ContractMappings> GetContractTemplatePropertiesAsync(int contractTemplateId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/contracts/properties/{contractTemplateId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ContractMappings>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<IEnumerable<string>> GetAvailablePropertiesOfEmployeeAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/contracts/properties");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> SaveContractMappings(ContractMappings request)
        {
            HttpResponseMessage result = await _httpClient.PutAsJsonAsync($"api/contracts/mappings", request);
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

        public async Task<FileDto> GenerateContractAsync(ContractGenerationRequest request)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync($"api/contracts/generate", request);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<FileDto>();
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }
    }
}
