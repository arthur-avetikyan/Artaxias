using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Organization
{
    public partial class Departments
    {
        [Inject] private IMatToaster MatToaster { get; set; }

        [Inject] private IDepartmentApiAccessor DepartmentApiAccessor { get; set; }

        private int PageSize { get; set; } = 10;

        private int CurrentPage { get; set; } = 0;

        private List<DepartmentResponse> _departments;
        private readonly List<EmployeeSelection> _employeeSelection = new List<EmployeeSelection>();
        private string _currentDepartmentName;
        private int _currentDepartmentId;

        private bool _isInsertDialogOpen = false;
        private bool _isEditDialogOpen = false;
        private bool _isDeleteDialogOpen = false;

        private class EmployeeSelection
        {
            public int Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public bool IsSelected { get; set; }

            public string FullName => $"{FirstName} {LastName}";
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await InitializeDepartmentsListAsync();
            SortData(null);
        }

        private async Task InitializeDepartmentsListAsync()
        {
            try
            {
                _departments = await DepartmentApiAccessor.GetDepartmentListAsync(PageSize, CurrentPage);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void SortData(MatSortChangedEvent sort)
        {
            if (!(sort == null || sort.Direction == MatSortDirection.None || string.IsNullOrEmpty(sort.SortId)))
            {
                Comparison<DepartmentResponse> comparison = null;
                switch (sort.SortId)
                {
                    case "department":
                        comparison = (s1, s2) => string.Compare(s1.Name, s2.Name, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "head":
                        comparison = (s1, s2) => string.Compare(s1.Name, s2.Name, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "staff-count":
                        comparison = (s1, s2) => s1.Staff.Count.CompareTo(s2.Staff.Count);
                        break;
                    default:
                        break;
                }
                if (comparison != null)
                {
                    if (sort.Direction == MatSortDirection.Desc)
                    {
                        _departments.Sort((s1, s2) => -1 * comparison(s1, s2));
                    }
                    else
                    {
                        _departments.Sort(comparison);
                    }
                }
            }
        }

        private async Task OpenInsertDialog()
        {
            try
            {
                _currentDepartmentName = "";
                List<EmployeeResponse> employeeResponse = await DepartmentApiAccessor.GetEmployees();
                _employeeSelection.Clear();
                foreach (EmployeeResponse employee in employeeResponse)
                {
                    EmployeeSelection newEmployeesSelection = new EmployeeSelection
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName
                    };
                    _employeeSelection.Add(newEmployeesSelection);
                }
                _isInsertDialogOpen = true;
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task OpenEditDialog(int departmentId)
        {
            _currentDepartmentId = departmentId;
            _currentDepartmentName = _departments.FirstOrDefault(d => d.Id == _currentDepartmentId).Name;
            List<EmployeeResponse> employeeResponse = await DepartmentApiAccessor.GetEmployees();
            _employeeSelection.Clear();

            foreach (EmployeeResponse employee in employeeResponse)
            {
                EmployeeSelection newEmployeesSelection = new EmployeeSelection
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    IsSelected = employeeResponse != null && _departments.FirstOrDefault(d => d.Id == _currentDepartmentId).Staff.Any(e => e.Id == employee.Id)
                };
                _employeeSelection.Add(newEmployeesSelection);
            }
            _isEditDialogOpen = true;
        }

        private bool CheckIfSelceted(EmployeeSelection employee)
        {
            bool current = employee.IsSelected = !employee.IsSelected;
            return current;
        }

        private void OpenDeleteDialog(int departmentId)
        {
            _currentDepartmentId = departmentId;
            _currentDepartmentName = _departments.FirstOrDefault(d => d.Id == _currentDepartmentId).Name;
            _isDeleteDialogOpen = true;
        }

        private async Task InsertDepartmentAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_currentDepartmentName))
                {
                    MatToaster.Add("Enter a Department Name", MatToastType.Danger);
                    return;
                }

                DepartmentRequest department = new DepartmentRequest
                {
                    Name = _currentDepartmentName,
                    Staff = new List<int>()
                };

                foreach (EmployeeSelection employee in _employeeSelection.Where(e => e.IsSelected))
                {
                    department.Staff.Add(employee.Id);
                }

                string response = await DepartmentApiAccessor.CreateDepartmentAsync(department);
                await OnInitializedAsync();
                _isInsertDialogOpen = false;
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task EditDepartmentAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_currentDepartmentName))
                {
                    MatToaster.Add("Enter in a Department Name", MatToastType.Danger);
                    return;
                }

                DepartmentRequest department = new DepartmentRequest
                {
                    Id = _currentDepartmentId,
                    Name = _currentDepartmentName,
                    Staff = new List<int>()
                };
                foreach (EmployeeSelection employee in _employeeSelection.Where(e => e.IsSelected))
                {
                    department.Staff.Add(employee.Id);
                }

                DepartmentResponse response = await DepartmentApiAccessor.UpdateDepartmentAsync(department);
                await OnInitializedAsync();
                _isEditDialogOpen = false;
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task DeleteDepartmentAsync()
        {
            try
            {
                await DepartmentApiAccessor.DeleteDepartmentAsync(_currentDepartmentId);

                MatToaster.Add("Department Deleted", MatToastType.Success);
                await OnInitializedAsync();
                _isDeleteDialogOpen = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void ShowMore(int departmentId)
        {
            DepartmentResponse department = _departments.FirstOrDefault(r => r.Id == departmentId);
            department.DisplayMore = !department.DisplayMore;
            StateHasChanged();
        }
    }
}