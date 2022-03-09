using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Account;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class TokenRefreshApiAccessor : ITokenRefreshApiAccessor
    {
        private readonly HttpClient _httpClient;

        public TokenRefreshApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthenticationResult> Refresh(AuthenticationResult authenticationResult)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync("api/account/refreshToken", authenticationResult);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<AuthenticationResult>();
            }

            return null;
        }
    }
}