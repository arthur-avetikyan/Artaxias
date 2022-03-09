using Artaxias.Web.Client.Extensions;
using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Organization.Employee
{
    public partial class Contract
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IContractApiAccessor ContractApiAccessor { get; set; }

        [Parameter] public int ContractTemplateId { get; set; }

        private ContractMappings ContractMappings { get; set; } = new ContractMappings();

        private string _returnUrl;
        private bool _isEditEnabled;
        private IEnumerable<string> _availableProperties;

        protected override async Task OnInitializedAsync()
        {
            string query = new Uri(NavigationManager.Uri).Query;
            _returnUrl = query.GetReturnUrlFromQuery();
            _isEditEnabled = query.GetBoolFromQuery("isEditEnabled");

            await GetContractMappingsAsync();
            await GetPropertiesAsync();
        }

        private async Task GetContractMappingsAsync()
        {
            try
            {
                ContractMappings = await ContractApiAccessor.GetContractTemplatePropertiesAsync(ContractTemplateId);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task GetPropertiesAsync()
        {
            try
            {
                _availableProperties = await ContractApiAccessor.GetAvailablePropertiesOfEmployeeAsync();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void ReplaceValue(string key, string value)
        {
            if (ContractMappings.Mappings.ContainsKey(key))
            {
                ContractMappings.Mappings[key] = value;
            }
        }

        private async Task UploadTemplate()
        {
            try
            {
                string response = await ContractApiAccessor.SaveContractMappings(ContractMappings);
                MatToaster.Add(response, MatToastType.Success);
                NavigationManager.NavigateTo("contracts");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }
    }
}
