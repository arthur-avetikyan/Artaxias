using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Account;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Organization
{
    public partial class Employees
    {
        [Inject] private IMatToaster MatToaster { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private IEmployeesApiAccessor EmployeesApiAccessor { get; set; }

        [Inject] private IUsersApiAccessor UsersApiAccessor { get; set; }

        [Inject] private IDepartmentApiAccessor DepartmentApiAccessor { get; set; }


        [CascadingParameter] public UserInfo UserInfo { get; set; }

        private int PageSize { get; set; } = 10;
        private int CurrentPage { get; set; } = 0;
        private EmployeeRequest EmployeeRequest { get; set; } = new EmployeeRequest();

        private List<EmployeeResponse> _employees;
        private List<UserDetails> _unemployeedUsers = new List<UserDetails>();
        private readonly List<DepartmentSelection> _departmentSelection = new List<DepartmentSelection>();
        private bool _isAddEmployeeDialogOpen = false;

        private class DepartmentSelection
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public bool IsSelected { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await InitializeEmployeesListAsync();
            await InitializeNewEmployeeData();
            SortEmployeesData(null);
        }

        private async Task InitializeEmployeesListAsync()
        {
            try
            {
                _employees = await EmployeesApiAccessor.GetEmployeesListAsync(PageSize, CurrentPage);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void SortEmployeesData(MatSortChangedEvent sort)
        {
            if (!(sort == null || sort.Direction == MatSortDirection.None || string.IsNullOrEmpty(sort.SortId)))
            {
                Comparison<EmployeeResponse> comparison = null;
                switch (sort.SortId)
                {
                    case "name":
                        comparison = (s1, s2) => string.Compare(s1.FullName, s2.FullName, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "position":
                        comparison = (s1, s2) => string.Compare(s1.Position, s2.Position, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "assignment-date":
                        comparison = (s1, s2) => s1.ContractStart.CompareTo(s2.ContractStart);
                        break;
                    case "resignment-date":
                        comparison = (s1, s2) => s1.ContractEnd.GetValueOrDefault().CompareTo(s2.ContractEnd.GetValueOrDefault());
                        break;
                    case "department":
                        comparison = (s1, s2) => s1.Departments.Count.CompareTo(s2.Departments.Count);
                        break;
                    default:
                        break;
                }
                if (comparison != null)
                {
                    if (sort.Direction == MatSortDirection.Desc)
                    {
                        _employees.Sort((s1, s2) => -1 * comparison(s1, s2));
                    }
                    else
                    {
                        _employees.Sort(comparison);
                    }
                }
            }
        }

        private async Task InitializeNewEmployeeData()
        {
            await InitializeUnemployeedUsers();
            await InitializeDepartmentsAsync();
        }

        private async Task InitializeUnemployeedUsers()
        {
            try
            {
                _unemployeedUsers = await UsersApiAccessor.GetListAsync(unemployeed: true);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task InitializeDepartmentsAsync()
        {
            try
            {
                List<DepartmentResponse> departments = await DepartmentApiAccessor.GetDepartmentListAsync(pageSize: int.MaxValue);
                foreach (DepartmentResponse item in departments)
                {
                    _departmentSelection.Add(new DepartmentSelection
                    {
                        Id = item.Id,
                        Name = item.Name,
                        IsSelected = false
                    });
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task CreateEmployee()
        {
            try
            {
                string message = await EmployeesApiAccessor.CreateEmployeeAsync(EmployeeRequest);
                MatToaster.Add("Employee Created", MatToastType.Success);
                StateHasChanged();
                _isAddEmployeeDialogOpen = false;
                NavigationManager.NavigateTo($"employees/{message}?isEditDisabled={true}&returnUrl={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task DeleteEmployee(int employeeId)
        {
            try
            {
                await EmployeesApiAccessor.DeleteEmployee(employeeId);
                MatToaster.Add("Employee Deleted", MatToastType.Success);
                StateHasChanged();
                NavigationManager.NavigateTo($"/employees");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void SelectionChangedEvent(object row)
        {
            EmployeeResponse currentEmployee = (EmployeeResponse)row;
            NavigationManager.NavigateTo($"employees/{currentEmployee.Id}?isEditDisabled={true}&returnUrl={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}");
        }

        private void ShowMore(int employeeId)
        {
            EmployeeResponse employee = _employees.FirstOrDefault(r => r.Id == employeeId);
            employee.DisplayMore = !employee.DisplayMore;
            StateHasChanged();
        }

        private bool SelectCheckBoxMultipleAnswer(DepartmentSelection department)
        {
            bool current = department.IsSelected = !department.IsSelected;
            if (current)
            {
                EmployeeRequest.DepartmentIds.Add(department.Id);
            }
            else
            {
                EmployeeRequest.DepartmentIds.Remove(department.Id);
            }

            return current;
        }
    }
}
