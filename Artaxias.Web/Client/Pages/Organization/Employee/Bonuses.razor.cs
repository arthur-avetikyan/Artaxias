using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Account;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Organization.Employee
{
    public partial class Bonuses
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IBonusesApiAccessor BonusesApiAccessor { get; set; }
        [Inject] private IEmployeesApiAccessor EmployeesApiAccessor { get; set; }

        [CascadingParameter] public UserInfo UserInfo { get; set; }
        [Parameter] public int EmployeeId { get; set; }
        [Parameter] public bool IsContractEnded { get; set; }
        [Parameter] public DateTime ContractStartedAt { get; set; }

        private int PageSize { get; set; } = 10;
        private int CurrentPage { get; set; } = 0;

        public BonusRequest Request { get; set; } = new BonusRequest();

        private List<BonusResponse> _bonuses;
        private List<EmployeeResponse> _employees = new List<EmployeeResponse>();
        private bool _isCreateDialogOpen = false;

        protected override async Task OnInitializedAsync()
        {
            await InitializeBonusesAsync();
            SortBonusData(null);
        }

        private async Task InitializeBonusesAsync()
        {
            try
            {
                List<BonusResponse> response = await BonusesApiAccessor.GetBonusesListForEmployeeAsync(employeeId: EmployeeId, pageSize: PageSize, currentPage: CurrentPage);
                if (response == null)
                {
                    MatToaster.Add("There are no bonuses for this employee.", MatToastType.Danger);
                }
                else
                {
                    _bonuses = response;
                    MatToaster.Add("Success", MatToastType.Success, "Bonuses Retrieved");
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "Operation failed");
            }
        }

        private void SortBonusData(MatSortChangedEvent sort)
        {
            if (!(sort == null || sort.Direction == MatSortDirection.None || string.IsNullOrEmpty(sort.SortId)))
            {
                Comparison<BonusResponse> comparison = null;
                switch (sort.SortId)
                {
                    case "amount":
                        comparison = (s1, s2) => s1.Amount.CompareTo(s2.Amount);
                        break;
                    case "payment-date":
                        comparison = (s1, s2) => s1.PaymentDate.CompareTo(s2.PaymentDate);
                        break;
                    case "comment":
                        comparison = (s1, s2) => s1.Comment.CompareTo(s2.Comment);
                        break;
                    case "approver":
                        comparison = (s1, s2) => s1.Approver.FullName.CompareTo(s2.Approver.FullName);
                        break;
                    case "requester":
                        comparison = (s1, s2) => s1.Requester.FullName.CompareTo(s2.Requester.FullName);
                        break;
                    case "receiver":
                        comparison = (s1, s2) => s1.Receiver.FullName.CompareTo(s2.Receiver.FullName);
                        break;
                    case "state":
                        comparison = (s1, s2) => s1.DomainStateId.CompareTo(s2.DomainStateId);
                        break;
                    default:
                        break;
                }
                if (comparison != null)
                {
                    if (sort.Direction == MatSortDirection.Desc)
                    {
                        _bonuses.Sort((s1, s2) => -1 * comparison(s1, s2));
                    }
                    else
                    {
                        _bonuses.Sort(comparison);
                    }
                }
            }
        }

        private async Task OpenCreateDialog()
        {
            Request.ReceiverId = EmployeeId;
            Request.RequesterId = UserInfo.EmployeeId;
            await InitializeEmployeesListAsync();
            _isCreateDialogOpen = true;
        }

        private async Task InitializeEmployeesListAsync()
        {
            try
            {
                _employees = await EmployeesApiAccessor.GetEmployeesListAsync(int.MaxValue, 0);
                //_employees = response.Where(e => e.IsHead).ToList();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task CreateBonusAsync()
        {
            try
            {
                string response = await BonusesApiAccessor.CreateBonusAsync(Request);
                MatToaster.Add("Bonus Requested", MatToastType.Success);
                _isCreateDialogOpen = false;
                StateHasChanged();
                await OnInitializedAsync();
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
                AbsenceResponse currentSelectedBonus = (AbsenceResponse)row;
                NavigationManager.NavigateTo($"bonuses/{currentSelectedBonus.Id}?returnUrl={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}");
            }
        }
    }
}
