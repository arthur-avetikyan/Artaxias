
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
    public partial class Users
    {
        [Inject] private IMatToaster MatToaster { get; set; }

        [Inject] private IUsersApiAccessor UsersApiAccessor { get; set; }

        [Inject] private IRoleApiAccessor RolesApiAccessor { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        private const int RolesPopulationCount = 100;

        private int PageSize { get; set; } = 10;
        private int CurrentPage { get; set; } = 0;

        private bool _createUserDialogOpen = false;
        private bool _changePasswordDialogOpen = false;
        //private bool _collapsed = true;
        private string _currentUrl;

        private UserDetails _currentSelectedUser;
        private List<UserDetails> _users;

        private List<UserRoleStatus> UserRoleStatuses { get; set; } = new List<UserRoleStatus>();
        private UserDetails User { get; set; } = new UserDetails();
        private RegisterParameters RegisterParameters { get; set; } = new RegisterParameters();
        private ChangePasswordParameters ChangePasswordParameters { get; set; } = new ChangePasswordParameters();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await RetrieveUsersAsync();
            await RetrieveRolesAsync();

            _currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        }

        private void SelectionChangedEvent(object row)
        {
            if (row != null)
            {
                _currentSelectedUser = (UserDetails)row;
            }

            NavigationManager.NavigateTo($"users/{_currentSelectedUser.UserName}?returnUrl={_currentUrl}");
        }

        private async Task RetrieveUsersAsync()
        {
            try
            {
                _users = await UsersApiAccessor.GetListAsync(PageSize, CurrentPage);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "User Retrieval Error");
            }
        }

        private void OpenChangePasswordDialog(string userName, int userId)
        {
            ChangePasswordParameters = new ChangePasswordParameters
            {
                UserName = userName
            };
            User.Id = userId;
            _changePasswordDialogOpen = true;
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
                MatToaster.Add(ex.Message, MatToastType.Danger, "Role Retrieval Error");
            }
        }

        private async Task CreateUserAsync()
        {
            try
            {
                if (RegisterParameters.Password != RegisterParameters.PasswordConfirm)
                {
                    MatToaster.Add("Password Confirmation Failed", MatToastType.Danger, "");
                    return;
                }

                string response = await UsersApiAccessor.CreateAsync(RegisterParameters);
                RegisterParameters = new RegisterParameters(); //reset create user object after insert
                _createUserDialogOpen = false;
                StateHasChanged();
                await OnInitializedAsync();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "User Creation Error");
            }
        }

        private async Task ChangeUserPasswordAsync()
        {
            try
            {
                if (ChangePasswordParameters.NewPassword != ChangePasswordParameters.PasswordConfirm)
                {
                    MatToaster.Add("Passwords Must Match", MatToastType.Warning);
                }
                else
                {
                    await UsersApiAccessor.ChangePasswordAsync(ChangePasswordParameters);
                    MatToaster.Add("Password changed", MatToastType.Success);
                    _changePasswordDialogOpen = false;
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "Password Reset Error");
            }
        }

        private void ShowMore(int userId)
        {
            UserDetails user = _users.FirstOrDefault(r => r.Id == userId);
            user.DisplayMore = !user.DisplayMore;
            StateHasChanged();
        }
    }
}
