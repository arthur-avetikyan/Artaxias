
using Microsoft.AspNetCore.Components;

namespace Artaxias.Web.Client.Shared.Components
{
    public partial class TableRowEditButton
    {
        [Parameter] public string Link { get; set; }
        [Parameter] public EventCallback Click { get; set; }
    }
}
