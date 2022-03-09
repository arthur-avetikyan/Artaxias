
using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Account;
using Artaxias.Web.Common.DataTransferObjects.UserManagement;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.UserManagement
{
    public partial class UserInfo
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private IUsersApiAccessor UsersApiAccessor { get; set; }
        [Inject] private IRoleApiAccessor RolesApiAccessor { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Parameter] public string UserName { get; set; }
        [Parameter] public bool IsEditEnabled { get; set; } = false;

        private const int RolesPopulationCount = 100;

        // private bool _changePasswordDialogOpen = false;
        private UserDetails _tempUserDetails;

        private UserDetails UserDetails { get; set; } = new UserDetails();
        private List<UserRoleStatus> UserRoleStatuses { get; set; } = new List<UserRoleStatus>();

        protected override async Task OnInitializedAsync()
        {
            await RetrieveUserAsync();
            await RetrieveRolesAsync();

            if (UserDetails != null)
            {
                _tempUserDetails = UserDetails.Clone();
            }
        }

        private async Task RetrieveUserAsync()
        {
            try
            {
                UserDetails = await UsersApiAccessor.GetAsync(UserName);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task RetrieveRolesAsync()
        {
            try
            {
                List<RoleResponse> roles = await RolesApiAccessor.GetRolesAsync(RolesPopulationCount);

                // initialize selection list with all un-selected
                UserRoleStatuses = new List<UserRoleStatus>();
                foreach (RoleResponse role in roles)
                {
                    UserRoleStatuses.Add(new UserRoleStatus
                    {
                        Name = role.Name,
                        IsSelected = false
                    });
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void EnableEdit()
        {
            foreach (UserRoleStatus role in UserRoleStatuses)
            {
                if (UserDetails != null)
                {
                    role.IsSelected = UserDetails.Roles.Contains(role.Name);
                }
            }
            IsEditEnabled = true;
        }

        private void DisableEdit()
        {
            UserDetails = _tempUserDetails.Clone();
            IsEditEnabled = false;
        }

        private async Task UpdateAsync()
        {
            try
            {
                //update the user object's role list with the new selection set
                UserDetails.Roles = UserRoleStatuses.Where(x => x.IsSelected == true).Select(x => x.Name).ToList();

                UserDetails apiResponse = await UsersApiAccessor.UpdateAsync(UserDetails);
                IsEditEnabled = false;
                NavigationManager.NavigateTo($"users/{UserDetails.UserName}");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private static void UpdateUserRole(UserRoleStatus userRoleStatus) => userRoleStatus.IsSelected = !userRoleStatus.IsSelected;
    }
}
