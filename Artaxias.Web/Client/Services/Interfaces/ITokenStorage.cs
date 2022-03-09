using Artaxias.Web.Common.DataTransferObjects.Account;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface ITokenStorage
    {
        Task<AuthenticationResult> GetParametersForRefresh();
        Task RemoveAccessTokensAsync();
        Task RemoveRefreshTokensAsync();
        Task SetAccessTokenAsync(string token);
        Task SetRefreshTokenAsync(string token);
    }
}