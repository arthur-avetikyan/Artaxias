using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Feadback;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Feadback
{
    public partial class Templates
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private ITemplateApiAccessor TemplateApiAccessor { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private int PageSize { get; set; } = 10;
        private int CurrentPage { get; set; } = 0;

        private List<TemplateResponse> _templates;

        private bool _isDeleteDialogOpen = false;
        private string _currentTemplateTitle;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _templates = await TemplateApiAccessor.GetFeadbackTemplatesAsync(PageSize, CurrentPage);
                SortTemplatesData(null);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void SortTemplatesData(MatSortChangedEvent sort)
        {
            if (!(sort == null || sort.Direction == MatSortDirection.None || string.IsNullOrEmpty(sort.SortId)))
            {
                Comparison<TemplateResponse> comparison = null;
                switch (sort.SortId)
                {
                    case "title":
                        comparison = (s1, s2) => string.Compare(s1.Title, s2.Title, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "creator":
                        comparison = (s1, s2) => s1.CreatedByUserId.CompareTo(s2.CreatedByUserId);
                        break;
                    case "question-count":
                        comparison = (s1, s2) => s1.Questions.Count.CompareTo(s2.Questions.Count);
                        break;
                    case "inUse":
                        comparison = (s1, s2) => s1.InUse.CompareTo(s2.InUse);
                        break;
                    default:
                        break;
                }
                if (comparison != null)
                {
                    if (sort.Direction == MatSortDirection.Desc)
                    {
                        _templates.Sort((s1, s2) => -1 * comparison(s1, s2));
                    }
                    else
                    {
                        _templates.Sort(comparison);
                    }
                }
            }
        }

        private void OpenDeleteDialog(string templateTitle)
        {
            _currentTemplateTitle = templateTitle;
            _isDeleteDialogOpen = true;
        }

        private async Task DeleteTemplate()
        {
            try
            {
                TemplateResponse template = _templates.FirstOrDefault(t => t.Title == _currentTemplateTitle);
                await TemplateApiAccessor.DeleteTemplateAsync(template.Id);
                MatToaster.Add("Deleted", MatToastType.Success);
                StateHasChanged();
                await OnInitializedAsync();
                _isDeleteDialogOpen = false;
            }
            catch (Exception ex)
            {
                MatToaster.Add("Error", MatToastType.Danger, ex.Message);
            }
        }

        private void SelectionChangedEvent(object row)
        {
            TemplateResponse currentTemplate = (TemplateResponse)row;
            NavigationManager.NavigateTo($"templates/{currentTemplate.Id}?isEditDisabled={true}&returnUrl={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}");
        }
    }
}
