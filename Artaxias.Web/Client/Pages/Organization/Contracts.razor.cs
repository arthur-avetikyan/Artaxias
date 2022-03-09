using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Account;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Organization
{
    public partial class Contracts
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IContractApiAccessor ContractApiAccessor { get; set; }

        [CascadingParameter] public UserInfo UserInfo { get; set; }

        private int PageSize { get; set; } = 10;
        private int CurrentPage { get; set; } = 0;

        private List<ContractTemplateResponse> _contracts;
        private bool _isDeleteDialogOpen = false;
        private bool _isUploadDialogOpen = false;
        private int _currentcontractTemplateId;
        private string _currentContractTemplateTitle;

        private ContractTemplateRequest ContractTemplateRequest { get; set; } = new ContractTemplateRequest();
        private IMatFileUploadEntry _matFileUpload;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _contracts = await ContractApiAccessor.GetTemplatesListAsync(PageSize, CurrentPage);
                SortData(null);
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
                Comparison<ContractTemplateResponse> comparison = null;
                switch (sort.SortId)
                {
                    case "title":
                        comparison = (s1, s2) => string.Compare(s1.Title, s2.Title, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "creator":
                        comparison = (s1, s2) => string.Compare(s1.CreatorName, s2.CreatorName, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "created-date":
                        comparison = (s1, s2) => s1.CreatedDatetimeUTC.CompareTo(s2.CreatedDatetimeUTC);
                        break;
                    default:
                        break;
                }
                if (comparison != null)
                {
                    if (sort.Direction == MatSortDirection.Desc)
                    {
                        _contracts.Sort((s1, s2) => -1 * comparison(s1, s2));
                    }
                    else
                    {
                        _contracts.Sort(comparison);
                    }
                }
            }
        }

        private void OpenDeleteDialog(int contractTemplateId)
        {
            _currentcontractTemplateId = contractTemplateId;
            _currentContractTemplateTitle = _contracts.FirstOrDefault(d => d.Id == contractTemplateId).Title;
            _isDeleteDialogOpen = true;
        }

        private void OpenUploadDialog()
        {
            ContractTemplateRequest.CreatorId = UserInfo.Id;
            _isUploadDialogOpen = true;
        }

        private async Task FilesReadyMat(IMatFileUploadEntry[] files)
        {
            IMatFileUploadEntry file = files.FirstOrDefault();
            if (file != null)
            {
                using MemoryStream memoryStream = new MemoryStream();
                byte[] buffer = new byte[file.Size];
                await file.WriteToStreamAsync(memoryStream);

                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.ReadAsync(buffer);

                ContractTemplateRequest.Document.FileData = buffer;
                ContractTemplateRequest.Document.FileName = file.Name;
            }
            _matFileUpload = file;
        }

        private async Task Upload()
        {
            try
            {
                string response = await ContractApiAccessor.CreateTemplateAsync(ContractTemplateRequest);
                NavigateToEdit(response);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "Contract Template Upload Error");
                NulifyUploadData();
            }
        }

        private void NavigateToEdit(string editUrl)
        {
            NavigationManager.NavigateTo($"contracts/{editUrl}?isEditEnabled={true}&returnUrl={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}");
        }

        private void NulifyUploadData()
        {
            _isUploadDialogOpen = false;
            _matFileUpload = null;
            ContractTemplateRequest = new ContractTemplateRequest();
            StateHasChanged();
        }

        private async Task DeleteAsync()
        {
            try
            {
                await ContractApiAccessor.DeleteTemplateAsync(_currentcontractTemplateId);
                _isDeleteDialogOpen = false;
                MatToaster.Add("Contract Template Deleted", MatToastType.Success);
                await OnInitializedAsync();
            }
            catch (Exception ex)
            {
                _isDeleteDialogOpen = false;
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }
    }
}
