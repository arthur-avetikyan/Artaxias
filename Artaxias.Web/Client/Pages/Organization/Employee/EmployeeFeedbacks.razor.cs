using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Feadback;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Organization.Employee
{
    public partial class EmployeeFeedbacks
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IReviewApiAccessor ReviewApiAccessor { get; set; }

        [Parameter] public int EmployeeId { get; set; }

        private List<ReviewAboutEmployeeResponse> _feedbacks;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _feedbacks = await ReviewApiAccessor.GetReviewsAboutEmployeeAsync(EmployeeId);
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
                Comparison<ReviewAboutEmployeeResponse> comparison = null;
                switch (sort.SortId)
                {
                    case "reviewId":
                        comparison = (s1, s2) => s1.ReviewId.CompareTo(s2.ReviewId);
                        break;
                    case "title":
                        comparison = (s1, s2) => string.Compare(s1.ReviewTitle, s2.ReviewTitle, StringComparison.InvariantCultureIgnoreCase);
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
            ReviewAboutEmployeeResponse current = (ReviewAboutEmployeeResponse)row;
            NavigationManager.NavigateTo($"feedbacks/{EmployeeId}?reviewId={current.ReviewId}&returnUrl={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}");
        }
    }
}
