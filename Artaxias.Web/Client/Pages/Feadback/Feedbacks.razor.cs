using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Feadback;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Feadback
{
    public partial class Feedbacks
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IFeedbackApiAccessor FeedbackApiAccessor { get; set; }

        [Parameter] public int EmployeeId { get; set; }
        private int PageSize { get; set; } = 10;
        private int CurrentPage { get; set; } = 0;

        private List<FeedbackDetails> _feedbacks;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _feedbacks = await FeedbackApiAccessor.GeAllFeedbacks(PageSize, CurrentPage);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void SortFeedbackData(MatSortChangedEvent sort)
        {
            if (!(sort == null || sort.Direction == MatSortDirection.None || string.IsNullOrEmpty(sort.SortId)))
            {
                Comparison<FeedbackDetails> comparison = null;
                switch (sort.SortId)
                {
                    case "title":
                        comparison = (s1, s2) => string.Compare(s1.ReviewTitle, s2.ReviewTitle, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "reviewer":
                        comparison = (s1, s2) => string.Compare(s1.Reviewer.FullName, s2.Reviewer.FullName, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "reviewee":
                        comparison = (s1, s2) => string.Compare(s1.Reviewee.FullName, s2.Reviewee.FullName, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "date":
                        comparison = (s1, s2) => s1.ProvidedDate.CompareTo(s2.ProvidedDate);
                        break;
                    default:
                        break;
                }
                if (comparison != null)
                {
                    if (sort.Direction == MatSortDirection.Desc)
                    {
                        _feedbacks.Sort((s1, s2) => -1 * comparison(s1, s2));
                    }
                    else
                    {
                        _feedbacks.Sort(comparison);
                    }
                }
            }
        }

        private void SelectionChangedEvent(object row)
        {
            FeedbackDetails current = (FeedbackDetails)row;
            NavigationManager.NavigateTo($"feedbacks/{current.Reviewee.Id}?reviewId={current.ReviewId}&returnUrl={NavigationManager.Uri}");
        }
    }
}
