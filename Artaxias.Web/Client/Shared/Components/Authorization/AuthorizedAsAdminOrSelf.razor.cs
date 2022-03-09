
using Artaxias.Core.Enums;
using Artaxias.Core.Extensions;
using Artaxias.Web.Common.DataTransferObjects.Account;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Artaxias.Web.Client.Shared.Components.Authorization
{
    public partial class AuthorizedAsAdminOrSelf
    {
        [CascadingParameter]
        public UserInfo UserInfo { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string UserName { get; set; }

        [Parameter]
        public int UserId { get; set; }

        [Parameter]
        public int EmployeeId { get; set; }

        [Parameter]
        public bool DisplayNotFound { get; set; }

        private bool IsAuthorized(AuthenticationState AuthorizeContext)
        {
            string role = AuthorizeContext.User.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            if (!string.IsNullOrWhiteSpace(role) && role == EAuthorizationRoles.Administrator.GetAuthorizationRolesName())
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                return AuthorizeContext.User.Identity.Name == UserName;
            }

            if (AuthorizeContext.User.Identity.Name == UserInfo.UserName)
            {
                if (UserId > 0)
                {
                    return UserInfo.Id == UserId;
                }
                if (EmployeeId > 0)
                {
                    return UserInfo.EmployeeId == EmployeeId;
                }
            }

            return false;
        }
    }
}
