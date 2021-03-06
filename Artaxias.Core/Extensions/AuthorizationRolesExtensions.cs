
using Artaxias.Core.Enums;
using System;

namespace Artaxias.Core.Extensions
{
    public static class AuthorizationRolesExtensions
    {
        public static string GetAuthorizationRolesName(this int state)
        {
            return Enum.GetName(typeof(EAuthorizationRoles), state);
        }

        public static string GetAuthorizationRolesName(this EAuthorizationRoles state)
        {
            return Enum.GetName(typeof(EAuthorizationRoles), state);
        }
    }
}
