using Artaxias.Web.Client.Extensions;
using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using MatBlazor;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Organization.Employee
{
    public partial class EmployeeInfo
    {
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IEmployeesApiAccessor EmployeesApiAccessor { get; set; }
        [Inject] private IDepartmentApiAccessor DepartmentApiAccessor { get; set; }
        [Inject] private IContractApiAccessor ContractApiAccessor { get; set; }

        [Parameter] public int EmployeeId { get; set; }
        [Parameter] public bool IsEditDisabled { get; set; } = false;

        private EmployeeResponse EmployeeResponse { get; set; }

        private EmployeeRequest EmployeeRequest { get; set; } = new EmployeeRequest();

        private EndContractRequest EndContractRequest { get; set; } = new EndContractRequest();

        private ContractGenerationRequest ContractGenerationRequest { get; set; } = new ContractGenerationRequest();

        private List<DepartmentResponse> _departments = new List<DepartmentResponse>();
        private List<ContractTemplateResponse> _contractTemplates = new List<ContractTemplateResponse>();
        private List<DepartmentSelection> _departmentSelection = new List<DepartmentSelection>();
        private bool _isContractEndVisible = false;
        private bool _isDocumentSelectOpen = false;

        private readonly MatTheme _theme = new MatTheme { Primary = MatThemeColors.Red._500.Value, Secondary = MatThemeColors.Red._500.Value };

        private class DepartmentSelection
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public bool IsSelected { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            string query = new Uri(NavigationManager.Uri).Query;
            _isContractEndVisible = query.GetBoolFromQuery("endContract");

            await InitializeEmployeesAsync();
            await InitializeDeparmentsAsync();
            await InitializeContractsAsync();

            InitalizeRequests();
            await base.OnInitializedAsync();
        }

        private async Task InitializeEmployeesAsync()
        {
            try
            {
                EmployeeResponse = await EmployeesApiAccessor.GetEmployeeAsync(EmployeeId);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task InitializeContractsAsync()
        {
            try
            {
                _contractTemplates = await ContractApiAccessor.GetTemplatesListAsync(int.MaxValue, 0);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void InitalizeRequests()
        {
            EmployeeRequest.Id = EmployeeResponse.Id;
            EmployeeRequest.Position = EmployeeResponse.Position;
            EmployeeRequest.ContractStart = EmployeeResponse.ContractStart;
            EmployeeRequest.DepartmentIds = EmployeeResponse.Departments.Select(d => d.DepartmentId).ToList();

            EndContractRequest.EmployeeId = EmployeeId;
            EndContractRequest.ContractEndDate = EmployeeResponse.ContractEnd.GetValueOrDefault();

            ContractGenerationRequest.EmployeeId = EmployeeId;
        }

        private async Task InitializeDeparmentsAsync()
        {
            try
            {
                _departments = await DepartmentApiAccessor.GetDepartmentListAsync(pageSize: int.MaxValue);
                _departmentSelection = _departments.Select(d => new DepartmentSelection
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsSelected = EmployeeResponse.Departments.Any(ed => ed.DepartmentId == d.Id)
                }).ToList();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task UpdateEmployeeAsync()
        {
            try
            {
                EmployeeResponse apiResponse = await EmployeesApiAccessor.UpdateEmployeeAsync(EmployeeRequest);
                StateHasChanged();
                await OnInitializedAsync();
                IsEditDisabled = !IsEditDisabled;
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
                IsEditDisabled = !IsEditDisabled;
            }
        }

        private async Task EndContractAsync()
        {
            try
            {
                EmployeeResponse apiResponse = await EmployeesApiAccessor.EndContractAsync(EndContractRequest);
                StateHasChanged();
                await OnInitializedAsync();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task GenerateContractAsync()
        {
            try
            {
                Common.DataTransferObjects.File.FileDto result = await ContractApiAccessor.GenerateContractAsync(ContractGenerationRequest);
                await JSRuntime.InvokeVoidAsync("BlazorDownloadFile.downloadFile", result.FileName, "application/octet-stream", result.FileData);
                _isDocumentSelectOpen = !_isDocumentSelectOpen;
                InitalizeRequests();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
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

            StateHasChanged();
            return current;
        }
    }
}
