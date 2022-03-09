using Artaxias.Web.Client.Services.Interfaces;

using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;

namespace Artaxias.Web.Client.Shared.Components
{
    public partial class AppBarLoginSection
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IAuthorizationApiAccessor AuthorizationApiAccessor { get; set; }

        private async Task LogoutClick()
        {
            try
            {
                await AuthorizationApiAccessor.Logout();
                NavigationManager.NavigateTo("/account/login");
            }
            catch (System.Exception)
            {
                NavigationManager.NavigateTo("/account/login");
            }
        }
    }
}
