using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Account;
using Artaxias.Web.Common.DataTransferObjects.UserManagement;

using Microsoft.AspNetCore.Components.Authorization;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class UsersApiAccessor : IUsersApiAccessor
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenStorage _tokenStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UsersApiAccessor(HttpClient httpClient, ITokenStorage tokenStorage, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _tokenStorage = tokenStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<UserDetails> GetAsync(string userName)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/users/{userName}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserDetails>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<UserInfo> GetUserInfoAsync(string userName)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/users/{userName}/userInfo");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserInfo>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<UserDetails>> GetListAsync(int pageSize = 10, int currentPage = 0, bool unemployeed = false)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/users?pageSize={pageSize}&pageNumber={currentPage}&unemployeed={unemployeed}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<UserDetails>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> CreateAsync(RegisterParameters parameters)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync("api/users", parameters);
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

        public async Task<UserDetails> UpdateAsync(UserDetails userDetails)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/users/{userDetails.Id}", userDetails);
            if (response.IsSuccessStatusCode)
            {
                UserDetails result = await response.Content.ReadFromJsonAsync<UserDetails>();
                await _tokenStorage.RemoveAccessTokensAsync();
                await ((IdentityAuthenticationStateProvider)_authenticationStateProvider).GetAuthenticationStateAsync();
                return result;
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task DeleteAsync(string userName)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/users/{userName}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task ChangePasswordAsync(ChangePasswordParameters parameters)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/users/{parameters.UserName}/changePassword", parameters);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
