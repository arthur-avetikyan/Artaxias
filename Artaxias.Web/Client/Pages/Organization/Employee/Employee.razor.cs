using Artaxias.Web.Client.Extensions;
using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Account;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Organization.Employee
{
    public partial class Employee
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private IEmployeesApiAccessor EmployeesApiAccessor { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        [CascadingParameter] public UserInfo UserInfo { get; set; }
        [Parameter] public int EmployeeId { get; set; }

        private EmployeeResponse CurrentEmployee { get; set; }
        private bool _isEditDisabled = false;
        private int _tabIndex = 0;
        private string _returnUrl;

        protected override async Task OnInitializedAsync()
        {
            await InitializeEmployeeAsync();
            string query = new Uri(NavigationManager.Uri).Query;
            _isEditDisabled = query.GetBoolFromQuery("isEditDisabled");
            _returnUrl = query.GetReturnUrlFromQuery();
        }

        private async Task InitializeEmployeeAsync()
        {
            try
            {
                CurrentEmployee = await EmployeesApiAccessor.GetEmployeeAsync(EmployeeId);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }
    }
}
