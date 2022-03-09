using Artaxias.Core.Enums;

using Microsoft.AspNetCore.Authorization;

namespace Artaxias.Web.Server.Infrastructure.Security
{
    public static class PolicyStaticProvider
    {
        public static AuthorizationPolicy GetAdminOnlyPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(nameof(EAuthorizationRoles.Administrator))
                .Build();
        }

        public static AuthorizationPolicy GetUserOnlyPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(nameof(EAuthorizationRoles.User))
                .Build();
        }
    }
}
