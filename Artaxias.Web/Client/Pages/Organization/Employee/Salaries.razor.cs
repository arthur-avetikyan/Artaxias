using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Organization.Employee
{
    public partial class Salaries
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private ISalariesApiAccessor SalariesApiAccessor { get; set; }

        [Parameter] public int EmployeeId { get; set; }
        [Parameter] public bool IsContractEnded { get; set; }
        [Parameter] public DateTime ContractStartedAt { get; set; }

        public SalaryRequest SalaryRequest { get; set; } = new SalaryRequest();

        private List<SalaryResponse> _salaries;
        private SalaryResponse _currentSalary;
        private bool _isCreateDialogOpen = false;

        protected override async Task OnInitializedAsync()
        {
            await InitializeSalariesAsync();
            SortSalaryData(null);
        }

        private async Task InitializeSalariesAsync()
        {
            try
            {
                List<SalaryResponse> response = await SalariesApiAccessor.GetSalariesListAsync(EmployeeId);
                _salaries = response;
                _currentSalary = _salaries.Where(s => s.AssignmentDate <= DateTime.UtcNow).OrderByDescending(s => s.AssignmentDate).FirstOrDefault();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void SortSalaryData(MatSortChangedEvent sort)
        {
            if (!(sort == null || sort.Direction == MatSortDirection.None || string.IsNullOrEmpty(sort.SortId)))
            {
                Comparison<SalaryResponse> comparison = null;
                switch (sort.SortId)
                {
                    case "net":
                        comparison = (s1, s2) => s1.NetAmount.CompareTo(s2.NetAmount);
                        break;
                    case "gross":
                        comparison = (s1, s2) => s1.GrossAmount.CompareTo(s2.GrossAmount);
                        break;
                    case "date":
                        comparison = (s1, s2) => s1.AssignmentDate.CompareTo(s2.AssignmentDate);
                        break;
                    default:
                        break;
                }
                if (comparison != null)
                {
                    if (sort.Direction == MatSortDirection.Desc)
                    {
                        _salaries.Sort((s1, s2) => -1 * comparison(s1, s2));
                    }
                    else
                    {
                        _salaries.Sort(comparison);
                    }
                }
            }
        }

        private void OpenCreateDialog()
        {
            SalaryRequest.EmployeeId = EmployeeId;
            if (_currentSalary != null)
            {
                SalaryRequest.AssignmentDate = _currentSalary.AssignmentDate;
                SalaryRequest.NetSalary = _currentSalary.NetAmount;
                SalaryRequest.GrossSalary = _currentSalary.GrossAmount;
            }
            _isCreateDialogOpen = true;
        }

        private async Task ChangeSalaryAsync()
        {
            try
            {
                string response = await SalariesApiAccessor.CreateSalaryAsync(SalaryRequest);
                StateHasChanged();
                await OnInitializedAsync();
                _isCreateDialogOpen = false;
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
                _isCreateDialogOpen = false;
            }
        }
    }
}