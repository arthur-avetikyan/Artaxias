
using Artaxias.Web.Client.Extensions;
using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Organization;
using Artaxias.Web.Common.Utilities;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Organization.Absence
{
    public partial class AbsenceProfile
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private IAbsencesApiAccessor AbsencesApiAccessor { get; set; }

        [Inject] private IEmployeesApiAccessor EmployeesApiAccessor { get; set; }

        [Parameter] public int AbsenceId { get; set; }

        private AbsenceResponse _absenceResponse;
        private AbsenceRequest _absenceRequest;
        private AbsenceRequest _tempAbsenceRequest;
        private EmployeeResponse _absenceApproverEmployee;
        private List<EmployeeResponse> _employees = new List<EmployeeResponse>();
        private List<AbsenceTypeResponse> _absenceTypes = new List<AbsenceTypeResponse>();

        private bool _isEditEnabled;
        private bool _deleteAbsenceExpansionOpen;
        private string _returnUrl;

        private readonly MatTheme _redTheme = new MatTheme() { Primary = MatThemeColors.Red._500.Value };
        private readonly MatTheme _greenTheme = new MatTheme() { Primary = MatThemeColors.Green._500.Value };

        protected override async Task OnInitializedAsync()
        {
            await RetrieveAsync();
            await RetrieveAbsenceTypesAsync();
            await InitializeEmployeesListAsync();
            await RetrieveAbsenceApproverEmployee();

            string query = new Uri(NavigationManager.Uri).Query;
            _isEditEnabled = query.GetBoolFromQuery("isEditEnabled");
            _deleteAbsenceExpansionOpen = query.GetBoolFromQuery("deleteAbsenceExpansionOpen");
            _returnUrl = query.GetReturnUrlFromQuery();

            if (_absenceRequest != null)
            {
                _tempAbsenceRequest = _absenceRequest.Clone();
            }
        }

        private async Task RetrieveAsync()
        {
            try
            {
                _absenceResponse = await AbsencesApiAccessor.GetAsync(AbsenceId);
                _absenceRequest = _absenceResponse.Map();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task InitializeEmployeesListAsync()
        {
            try
            {
                _employees = await EmployeesApiAccessor.GetEmployeesListAsync(int.MaxValue, 0);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task RetrieveAbsenceTypesAsync()
        {
            try
            {
                _absenceTypes = await AbsencesApiAccessor.GetAllTypesAsync();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task RetrieveAbsenceApproverEmployee()
        {
            try
            {
                _absenceApproverEmployee = await EmployeesApiAccessor.GetEmployeeAsync(_absenceResponse.Approver.Id);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task UpdateAsync()
        {
            try
            {
                await AbsencesApiAccessor.UpdateAsync(_absenceResponse.Id, _absenceRequest);
                StateHasChanged();
                await OnInitializedAsync();
                DisableEdit();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task DeleteAsync()
        {
            try
            {
                await AbsencesApiAccessor.DeleteAsync(AbsenceId);
                NavigationManager.NavigateTo($"/{_returnUrl}");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task ApproveAsync()
        {
            try
            {
                string message = await AbsencesApiAccessor.ApproveAsync(AbsenceId, 0);
                MatToaster.Add(message, MatToastType.Success);
                StateHasChanged();
                await OnInitializedAsync();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task RejectAsync()
        {
            try
            {
                string message = await AbsencesApiAccessor.RejectAsync(AbsenceId, 0);
                MatToaster.Add(message, MatToastType.Success);
                StateHasChanged();
                await OnInitializedAsync();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void EnableEdit()
        {
            _isEditEnabled = true;
        }
        private void DisableEdit()
        {
            _absenceRequest = _tempAbsenceRequest.Clone();
            _isEditEnabled = false;
        }
    }
}