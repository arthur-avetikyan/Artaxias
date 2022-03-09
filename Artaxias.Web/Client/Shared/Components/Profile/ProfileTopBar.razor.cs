
using Microsoft.AspNetCore.Components;

namespace Artaxias.Web.Client.Shared.Components.Profile
{
    public partial class ProfileTopBar
    {
        [Parameter] public string ReturnUrl { get; set; }

        [Parameter] public int StateId { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public bool IsAlignedLeft { get; set; }

    }
}
