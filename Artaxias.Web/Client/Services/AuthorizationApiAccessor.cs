using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Account;

using Microsoft.AspNetCore.Components.Authorization;

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class AuthorizationApiAccessor : IAuthorizationApiAccessor
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthorizationApiAccessor(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task Login(LoginRequest loginRequest)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/account/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                AuthenticationResult loginResult = await response.Content.ReadFromJsonAsync<AuthenticationResult>();
                await ((IdentityAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginResult);
                return;
            }
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> Register(RegisterParameters registerParameters)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/account/register", registerParameters);
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

        public async Task<string> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/account/confirmEmail", confirmEmailRequest);
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

        public async Task Logout()
        {
            HttpResponseMessage response = await _httpClient.PostAsync("api/account/logout", null);
            if (response.IsSuccessStatusCode)
            {
                await ((IdentityAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
