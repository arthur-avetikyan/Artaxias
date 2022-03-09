using Artaxias.Web.Client.Extensions;
using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Account;

using MatBlazor;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using System;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Account
{
    public partial class Login
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IAuthorizationApiAccessor AuthorizationApiAccessor { get; set; }
        [Inject] private IMatToaster MatToaster { get; set; }

        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        private LoginRequest LoginRequest { get; set; } = new LoginRequest();
        public string ReturnUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            string query = new Uri(NavigationManager.Uri).Query;
            ReturnUrl = query.GetReturnUrlFromQuery();

            System.Security.Claims.ClaimsPrincipal user = (await AuthenticationStateTask).User;
            if (user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/");
            }
        }

        private async Task SubmitLogin()
        {
            try
            {
                await AuthorizationApiAccessor.Login(LoginRequest);
                if (string.IsNullOrWhiteSpace(ReturnUrl))
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    NavigationManager.NavigateTo(ReturnUrl);
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void Register()
        {
            NavigationManager.NavigateTo("/account/register");
        }
    }
}
