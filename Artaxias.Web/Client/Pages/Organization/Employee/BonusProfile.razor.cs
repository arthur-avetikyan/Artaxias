using Artaxias.Core.Enums;
using Artaxias.Web.Client.Extensions;
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
    public partial class BonusProfile
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IBonusesApiAccessor BonusesApiAccessor { get; set; }
        [Inject] private IEmployeesApiAccessor EmployeesApiAccessor { get; set; }

        [CascadingParameter] public UserInfo UserInfo { get; set; }
        [Parameter] public int BonusId { get; set; }

        private string _returnUrl;
        private bool _isEditEnabled = false;
        private bool _deleteAbsenceExpansionOpen;

        private BonusRequest Request { get; set; } = new BonusRequest();
        private BonusResponse _bonus;
        private List<EmployeeResponse> _employees = new List<EmployeeResponse>();

        private readonly MatTheme _redTheme = new MatTheme { Primary = MatThemeColors.Red._500.Value };

        private readonly MatTheme _greenTheme = new MatTheme { Primary = MatThemeColors.Green._500.Value };

        protected override async Task OnInitializedAsync()
        {
            string query = new Uri(NavigationManager.Uri).Query;
            _returnUrl = query.GetReturnUrlFromQuery();

            await InitializeEmployeesListAsync();
            await InitializeBonusAsync();
        }

        private async Task InitializeBonusAsync()
        {
            try
            {
                _bonus = await BonusesApiAccessor.GetBonusAsync(BonusId);
                InitializeRequest();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void InitializeRequest()
        {
            Request = new BonusRequest
            {
                Id = _bonus.Id,
                Amount = _bonus.Amount,
                PaymentDate = _bonus.PaymentDate,
                Comment = _bonus.Comment,
                DomainStateId = _bonus.DomainStateId,
                ReceiverId = _bonus.Receiver.Id,
                ApproverId = _bonus.Approver.Id,
                RequesterId = _bonus.Requester.Id
            };
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

        private async Task UpdateAsync()
        {
            try
            {
                await BonusesApiAccessor.UpdateAsync(_bonus.Id, Request);
                StateHasChanged();
                await OnInitializedAsync();
                DisableEdit();
            }
            catch (Exception e)
            {
                MatToaster.Add(e.Message, MatToastType.Danger);
            }
        }

        private async Task DeleteAsync()
        {
            try
            {
                await BonusesApiAccessor.DeleteAsync(_bonus.Id);
                NavigationManager.NavigateTo($"/{_returnUrl}");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task ApproveAsync()
        {
            Request.DomainStateId = (int)EDomainState.Approved;
            await UpdateAsync();
            await OnInitializedAsync();
        }

        private async Task RejectAsync()
        {
            Request.DomainStateId = (int)EDomainState.Rejected;
            await UpdateAsync();
            await OnInitializedAsync();
        }

        private void EnableEdit()
        {
            _isEditEnabled = true;
        }
        private void DisableEdit()
        {
            InitializeRequest();
            _isEditEnabled = false;
        }
    }
}
