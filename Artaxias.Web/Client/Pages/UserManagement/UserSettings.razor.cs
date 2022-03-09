
using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.UserManagement;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.UserManagement
{
    public partial class UserSettings
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private IUsersApiAccessor UsersApiAccessor { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Parameter] public string UserName { get; set; }
        [Parameter] public bool DeleteUserExpansionOpen { get; set; }

        private ChangePasswordParameters ChangePasswordParameters { get; set; } = new ChangePasswordParameters();
        private bool _changePasswordExpansionOpen = false;

        private readonly MatTheme _redTheme = new MatTheme()
        {
            Primary = MatThemeColors.Red._500.Value,
            Secondary = MatThemeColors.Red._300.Value
        };

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
                    _changePasswordExpansionOpen = false;
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "Password Reset Error");
            }
        }

        private async Task DeleteUserAsync()
        {
            try
            {
                await UsersApiAccessor.DeleteAsync(UserName);

                MatToaster.Add("User Deleted", MatToastType.Success);
                DeleteUserExpansionOpen = false;
                StateHasChanged();
                NavigationManager.NavigateTo("/users");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "User Delete Error");
            }
        }
    }
}
