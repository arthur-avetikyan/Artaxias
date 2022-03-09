
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Client.Pages.Organization.Employee
{
    public partial class EmployeeTag
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        [Required]
        public int EmployeeId { get; set; }

        [Parameter]
        [Required]
        public string EmployeeFullName { get; set; }
    }
}
