using Artaxias.Web.Client.Extensions;
using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Feadback;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Feadback
{
    public partial class FeedbackProfile
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IFeedbackApiAccessor FeedbackApiAccessor { get; set; }

        [Parameter] public int EmployeeId { get; set; }

        private string _returnUrl;
        private int _reviewId;
        private List<FeedbackAboutEmployeeResponse> _feedbackAboutEmployee;

        protected override async Task OnInitializedAsync()
        {
            await GetFeedbacksAsync();

            string query = new Uri(NavigationManager.Uri).Query;
            _reviewId = query.GetNumberFromQuery("reviewId");
            _returnUrl = query.GetReturnUrlFromQuery();
        }

        private async Task GetFeedbacksAsync()
        {
            try
            {
                _feedbackAboutEmployee = await FeedbackApiAccessor.GeFeedbackAboutEmployeeAsync(EmployeeId, _reviewId);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }
    }
}
