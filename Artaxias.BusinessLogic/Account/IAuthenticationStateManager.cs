using Artaxias.Models;
using Artaxias.Web.Common.DataTransferObjects.Account;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Account
{
    public interface IAuthenticationStateManager
    {
        Task<AuthenticationResult> Authenticate(int userId, int? refreshTokenId, IEnumerable<Claim> claims);

        IEnumerable<Claim> GetClaims(User user, IEnumerable<string> UserRoles);
    }
}