using Artaxias.Web.Common.DataTransferObjects.Account;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface ITokenRefreshApiAccessor
    {
        Task<AuthenticationResult> Refresh(AuthenticationResult authenticationResult);
    }
}