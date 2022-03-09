using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Account;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Account
{
    public partial class ConfirmEmail
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private IAuthorizationApiAccessor AuthorizationApiAccessor { get; set; }

        [Parameter] public string UserName { get; set; }
        [Parameter] public string Token { get; set; }

        private ConfirmEmailRequest ConfirmEmailRequest { get; set; } = new ConfirmEmailRequest();
        private bool disableConfirmButton = false;

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Token))
            {
                disableConfirmButton = true;
                ConfirmEmailRequest = new ConfirmEmailRequest
                {
                    Token = Token,
                    UserName = UserName
                };
                await SendConfirmation();
            }
        }

        private async Task SendConfirmation()
        {
            try
            {
                string result = await AuthorizationApiAccessor.ConfirmEmailAsync(ConfirmEmailRequest);
                MatToaster.Add(result, MatToastType.Success);
                NavigationManager.NavigateTo("/");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "Email Verification");
            }
        }
    }
}
