
using Microsoft.AspNetCore.Components;

namespace Artaxias.Web.Client.Shared.Components
{
    public partial class TableHeaderColumn
    {
        [Parameter] public int Width { get; set; }

        [Parameter] public string Label { get; set; }
    }
}
