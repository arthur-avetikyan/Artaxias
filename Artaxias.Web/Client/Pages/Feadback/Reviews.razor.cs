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
    public partial class Reviews
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private IReviewApiAccessor ReviewApiAccessor { get; set; }

        private int PageSize { get; set; } = 10;
        private int CurrentPage { get; set; } = 0;

        private List<ReviewResponse> _reviews;
        private bool _isDeleteDialogOpen = false;
        private int _currentReviewId;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _reviews = await ReviewApiAccessor.GetReviewsAsync(PageSize, CurrentPage);
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
                Comparison<ReviewResponse> comparison = null;
                switch (sort.SortId)
                {
                    case "id":
                        comparison = (s1, s2) => s1.Id.CompareTo(s2.Id);
                        break;
                    case "title":
                        comparison = (s1, s2) => string.Compare(s1.Title, s2.Title, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "template":
                        comparison = (s1, s2) => string.Compare(s1.Template.Title, s2.Template.Title, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "department":
                        comparison = (s1, s2) => s1.Departments.Count.CompareTo(s2.Departments.Count);
                        break;
                    case "state":
                        comparison = (s1, s2) => s1.StateId.CompareTo(s2.StateId);
                        break;
                    case "creator":
                        comparison = (s1, s2) => string.Compare(s1.Template.CreatorName, s2.Template.CreatorName, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "question-count":
                        comparison = (s1, s2) => s1.Template.Questions.Count.CompareTo(s2.Template.Questions.Count);
                        break;
                    default:
                        break;
                }
                if (comparison != null)
                {
                    if (sort.Direction == MatSortDirection.Desc)
                    {
                        _reviews.Sort((s1, s2) => -1 * comparison(s1, s2));
                    }
                    else
                    {
                        _reviews.Sort(comparison);
                    }
                }
            }
        }

        private void OpenDeleteDialog(int id)
        {
            _currentReviewId = id;
            _isDeleteDialogOpen = true;
        }

        private async Task DeleteReview()
        {
            try
            {
                await ReviewApiAccessor.DeleteReviewAsync(_currentReviewId);
                MatToaster.Add("Deleted", MatToastType.Success);
                StateHasChanged();
                await OnInitializedAsync();
                _isDeleteDialogOpen = false;
            }
            catch (Exception ex)
            {
                _isDeleteDialogOpen = false;
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void SelectionChangedEvent(object row)
        {
            ReviewResponse current = (ReviewResponse)row;
            NavigationManager.NavigateTo($"reviews/{current.Id}?isEditDisabled={true}&returnUrl={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}");
        }


        private void ShowMore(int reviewId)
        {
            ReviewResponse review = _reviews.FirstOrDefault(r => r.Id == reviewId);
            review.DisplayMore = !review.DisplayMore;
            StateHasChanged();
        }
    }
}
