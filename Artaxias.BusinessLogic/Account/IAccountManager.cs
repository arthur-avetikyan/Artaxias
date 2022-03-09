
using Artaxias.Web.Common.DataTransferObjects.Account;

using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Account
{
    public interface IAccountManager
    {
        Task<AuthenticationResult> Login(LoginRequest parameters);

        Task<UserDetails> Register(RegisterParameters parameters);

        Task<ForgotPasswordParameters> ForgotPassword(ForgotPasswordParameters parameters);

        Task<AuthenticationResult> Refresh(AuthenticationResult authenticationResult);

        Task Revoke(string userName);

        Task ConfirmEmail(ConfirmEmailRequest request);
    }
}
