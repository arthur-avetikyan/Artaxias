
using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Organization.Absence
{
    public partial class EmployeeAbsence
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private IEmployeesApiAccessor EmployeesApiAccessor { get; set; }
        [Inject] private IAbsencesApiAccessor AbsencesApiAccessor { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Parameter] public int EmployeeId { get; set; }
        [Parameter] public bool IsContractEnded { get; set; }
        [Parameter] public DateTime ContractStartedAt { get; set; }

        private AbsenceRequest AbsenceRequest { get; set; } = new AbsenceRequest();

        private const int PageSize = 10;

        private AbsenceResponse _currentSelectedAbsence;
        private List<EmployeeResponse> _employees = new List<EmployeeResponse>();
        private List<AbsenceTypeResponse> _absenceTypes = new List<AbsenceTypeResponse>();
        private List<AbsenceResponse> _absences;

        private bool _isCreateRequestDialogOpen;
        private string _currentUrl;

        protected override async Task OnInitializedAsync()
        {
            await RetrieveAbsencesAsync();
            await RetrieveAbsenceTypesAsync();
            _currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        }

        private async Task RetrieveAbsencesAsync()
        {
            try
            {
                _absences = await AbsencesApiAccessor.GetAllAsync(EmployeeId);
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

        private void SelectionChangedEvent(object row)
        {
            if (row != null)
            {
                _currentSelectedAbsence = (AbsenceResponse)row;
            }
            NavigationManager.NavigateTo($"absences/{_currentSelectedAbsence.Id}?returnUrl={_currentUrl}");
        }

        //void SortData(MatSortChangedEvent sort)
        //{
        //    sortedData = _absences.ToArray();
        //    if (!(sort == null || sort.Direction == MatSortDirection.None || string.IsNullOrEmpty(sort.SortId)))
        //    {
        //        Comparison<Vacation> comparison = null;
        //        switch (sort.SortId)
        //        {
        //            case "title":
        //                comparison = (s1, s2) => string.Compare(s1.Title, s2.Title, StringComparison.InvariantCultureIgnoreCase);
        //                break;
        //            case "approved":
        //                comparison = (s1, s2) => s1.Approved.CompareTo(s2.Approved);
        //                break;
        //        }
        //        if (comparison != null)
        //        {
        //            if (sort.Direction == MatSortDirection.Desc)
        //            {
        //                Array.Sort(sortedData, (s1, s2) => -1 * comparison(s1, s2));
        //            }
        //            else
        //            {
        //                Array.Sort(sortedData, comparison);
        //            }
        //        }
        //    }
        //}

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

        private async Task OpenCreateDialog()
        {
            await InitializeEmployeesListAsync();
            AbsenceRequest.ReceiverId = EmployeeId;
            _isCreateRequestDialogOpen = true;
        }

        private void CloseCreateDialog()
        {
            _isCreateRequestDialogOpen = false;
            AbsenceRequest = new AbsenceRequest();
        }

        private async Task CreateAsync()
        {
            try
            {
                string message = await AbsencesApiAccessor.CreateAsync(AbsenceRequest);
                MatToaster.Add(message, MatToastType.Success);

                NavigationManager.NavigateTo($"{message}?returnUrl={_currentUrl}");
                //CloseCreateDialog();
                //StateHasChanged();
                //await OnInitializedAsync();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "Operation creation failed");
            }
        }
    }
}
