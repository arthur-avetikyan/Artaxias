using Artaxias.Core.Enums;
using Artaxias.Web.Server.Infrastructure.Security;

using Microsoft.AspNetCore.Authorization;

namespace Artaxias.Web.Server.Extensions
{
    public static class AuthorizationOptionsExtensions
    {
        public static AuthorizationOptions AddApplicationStaticPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy(nameof(EAuthorizationRoles.Administrator), PolicyStaticProvider.GetAdminOnlyPolicy());
            options.AddPolicy(nameof(EAuthorizationRoles.User), PolicyStaticProvider.GetUserOnlyPolicy());

            return options;
        }
    }
}
