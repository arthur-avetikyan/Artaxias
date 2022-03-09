using Artaxias.Web.Client.Extensions;
using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Feadback;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Feadback
{
    public partial class Review
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private ITemplateApiAccessor TemplateApiAccessor { get; set; }
        [Inject] private IDepartmentApiAccessor DepartmentApiAccessor { get; set; }
        [Inject] private IEmployeesApiAccessor EmployeesApiAccessor { get; set; }
        [Inject] private IReviewApiAccessor ReviewApiAccessor { get; set; }

        [Parameter] public int ReviewId { get; set; }

        private ReviewRequest ReviewRequest { get; set; }

        private bool _isPanelOpen;
        private bool _isEditDisabled;
        private string _returnUrl;
        private List<TemplateResponse> _templates;
        private readonly List<EmployeeInfo> _employees;
        private List<DepartmentResponse> _departments;
        private ReviewResponse _review;

        public Review() : base()
        {
            _isEditDisabled = false;
            _isPanelOpen = true;
            _review = new ReviewResponse();
            _templates = new List<TemplateResponse>();
            _employees = new List<EmployeeInfo>();
            _departments = new List<DepartmentResponse>();
            ReviewRequest = new ReviewRequest { ReviewerReviewees = new List<ReviewerRevieweeInfo> { new ReviewerRevieweeInfo() } };

            IEnumerable<EmployeeInfo> currentEmployees = _employees.Where(e => e.Departments.Any(ed => ReviewRequest.DepartmentIds.Any(d => d == ed)));
        }

        #region Initialization

        protected override async Task OnInitializedAsync()
        {
            if (ReviewId > 0)
            {
                await GetReviewAsync();
                InitializeRequest();
            }
            await InitializeDepartmentsAsync();
            await InitializeTemplatesAsync();
            await IntializeUsersAsync();


            string query = new Uri(NavigationManager.Uri).Query;
            _isEditDisabled = query.GetBoolFromQuery("isEditDisabled");
            _returnUrl = query.GetReturnUrlFromQuery();

        }

        private async Task GetReviewAsync()
        {
            try
            {
                _review = await ReviewApiAccessor.GetReviewAsync(ReviewId);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void InitializeRequest()
        {
            ReviewRequest = new ReviewRequest
            {
                Title = _review.Title,
                TemplateId = _review.Template.Id,
                Deadline = _review.Deadline,
                ReviewerReviewees = _review.ReviewerReviewees,
                DepartmentIds = _review.Departments.Select(d => d.DepartmentId).ToList()
            };
        }

        private async Task InitializeTemplatesAsync()
        {
            try
            {
                _templates = await TemplateApiAccessor.GetFeadbackTemplatesAsync(pageSize: 1000, currentPage: 0);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task InitializeDepartmentsAsync()
        {
            try
            {
                _departments = await DepartmentApiAccessor.GetDepartmentListAsync(int.MaxValue);
                _departments.ForEach(d => d.DisplayMore = _review.Departments.Any(rd => rd.DepartmentId == d.Id));
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task IntializeUsersAsync()
        {
            try
            {
                List<EmployeeResponse> response = await EmployeesApiAccessor.GetEmployeesListAsync(pageSize: int.MaxValue);
                foreach (EmployeeResponse item in response)
                {
                    _employees.Add(new EmployeeInfo
                    {
                        Id = item.Id,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Departments = item.Departments.Select(d => d.DepartmentId).ToList()
                    });
                }

            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        #endregion

        private void AddReviewerReviewee()
        {
            ReviewRequest.ReviewerReviewees.Add(new ReviewerRevieweeInfo());
        }

        private void RemoveReviewerReviewee(int reviewerRevieweeIndex)
        {
            ReviewRequest.ReviewerReviewees.RemoveAt(reviewerRevieweeIndex);
        }

        private async Task SaveReview()
        {
            try
            {
                if (ReviewId > 0)
                {
                    await ReviewApiAccessor.UpdateReviewAsync(ReviewId, ReviewRequest);
                    MatToaster.Add("Updated", MatToastType.Success);
                    NavigationManager.NavigateTo("reviews");
                }
                else
                {
                    await ReviewApiAccessor.CreateReviewAsync(ReviewRequest);
                    MatToaster.Add("Created", MatToastType.Success);
                    NavigationManager.NavigateTo("reviews");
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void UpdateDepartments(DepartmentResponse department)
        {
            department.DisplayMore = !department.DisplayMore;
            if (department.DisplayMore)
            {
                ReviewRequest.DepartmentIds.Add(department.Id);
            }
            else
            {
                ReviewRequest.DepartmentIds.Remove(department.Id);
            }
        }

        private string GetFeedbackUrl(int revieweeId, bool isCompleted)
        {
            if (isCompleted)
            {
                return NavigationManager.ToAbsoluteUri($"feedbacks/{revieweeId}?reviewId={_review.Id}&returnUrl={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}").AbsoluteUri;
            }
            return null;
        }

        private void ChangeExpantionPanel()
        {
            _isPanelOpen = !_isPanelOpen;
        }
    }
}
