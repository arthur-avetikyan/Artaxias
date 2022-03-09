using Artaxias.Web.Common.DataTransferObjects.Account;

using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface IAuthorizationApiAccessor
    {
        Task Login(LoginRequest loginParameters);

        Task Logout();

        Task<string> Register(RegisterParameters registerParameters);

        Task<string> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest);
    }
}
