using Artaxias.Core.Constants.Domain;
using Artaxias.Web.Client.Extensions;
using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Feadback;

using MatBlazor;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Feadback
{
    public partial class Feedback
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IFeedbackApiAccessor FeedbackApiAccessor { get; set; }
        [Inject] private IAuthorizationApiAccessor AuthorizationApiAccessor { get; set; }

        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Parameter] public int ReviewerRevieweeId { get; set; }
        [Parameter] public string ReviewerUserName { get; set; }

        private ProvideFeedbackRequest FeedbackRequest { get; set; }

        private ClaimsPrincipal _currentUser;
        private ProvideFeedbackResponse _feedbackResponse;
        private readonly List<MultiSelection> _multiSelections;
        private int currentIndex = 0;
        private ProvideFeedbackAnswerRequest userAnswer;
        private QuestionResponse currentQuestion;
        private string _returnUrl;

        public Feedback()
        {
            FeedbackRequest = new ProvideFeedbackRequest { FeedbackAnswerRequests = new List<ProvideFeedbackAnswerRequest>() };
            _multiSelections = new List<MultiSelection>();
            currentIndex = 0;
        }

        public class MultiSelection
        {
            public int QuestionId { get; set; }
            public List<AnswerSelection> AnswerSelections { get; set; }

            public class AnswerSelection
            {
                public int Id { get; set; }
                public string Value { get; set; }
                public bool IsSelected { get; set; }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await VerifyUserCredentialsAsync();
            await GetFeedbackDetails();

            string query = new Uri(NavigationManager.Uri).Query;
            _returnUrl = query.GetReturnUrlFromQuery();

            InitializeRequest();
        }

        private async Task VerifyUserCredentialsAsync()
        {
            try
            {
                _currentUser = (await AuthenticationStateTask).User;
                if (ReviewerUserName != _currentUser.Identity.Name)
                {
                    if (_currentUser.Identity.IsAuthenticated)
                    {
                        await AuthorizationApiAccessor.Logout();
                    }

                    NavigationManager.NavigateTo($"/account/login?returnUrl={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}");
                    return;
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
                NavigationManager.NavigateTo("/account/login");
            }
        }

        private async Task GetFeedbackDetails()
        {
            try
            {
                _feedbackResponse = await FeedbackApiAccessor.GetProvideFeedbackAsync(ReviewerUserName, ReviewerRevieweeId);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void InitializeRequest()
        {
            FeedbackRequest.ReviewerRevieweeId = _feedbackResponse.Id;
            foreach (QuestionResponse question in _feedbackResponse.Template.Questions)
            {
                ProvideFeedbackAnswerRequest feedbackAnswerRequest = new ProvideFeedbackAnswerRequest
                {
                    QuestionId = question.Id,
                    OptionType = question.Answer.OptionType,
                };
                if (question.Answer.OptionType.Description != AnswerOptionTypes.OpenText)
                {
                    feedbackAnswerRequest.AnswerValueIds = new List<int>();
                    MultiSelection selection = new MultiSelection
                    {
                        QuestionId = question.Id,
                        AnswerSelections = new List<MultiSelection.AnswerSelection>()
                    };

                    foreach (AnswerValueResponse answerValue in question.Answer.Values)
                    {
                        MultiSelection.AnswerSelection answerSelection = new MultiSelection.AnswerSelection
                        {
                            Id = answerValue.Id,
                            Value = answerValue.Value,
                            IsSelected = false
                        };
                        selection.AnswerSelections.Add(answerSelection);
                    }
                    _multiSelections.Add(selection);
                }
                else
                {
                    feedbackAnswerRequest.OpenTextValue = "";
                }
                FeedbackRequest.FeedbackAnswerRequests.Add(feedbackAnswerRequest);
            }
            userAnswer = FeedbackRequest.FeedbackAnswerRequests[currentIndex];
            currentQuestion = _feedbackResponse.Template.Questions.FirstOrDefault(q => q.Id == userAnswer.QuestionId);
        }

        private async Task ProvideFeedback()
        {
            try
            {
                string message = await FeedbackApiAccessor.ProvideFeedbackAsync(FeedbackRequest);
                MatToaster.Add("Thank you for your feedback", MatToastType.Success, message);
                NavigationManager.NavigateTo("/");
            }
            catch (Exception ex)
            {
                MatToaster.Add("Sorry, we were not able to regiter your feedback.", MatToastType.Danger, ex.Message);
                NavigationManager.NavigateTo("/");
            }
        }

        private void SelectCheckBoxSingleAnswer(int answerId, QuestionResponse question)
        {
            ProvideFeedbackAnswerRequest feedbackAnswerRequest = FeedbackRequest.FeedbackAnswerRequests.FirstOrDefault(f => f.QuestionId == question.Id);
            feedbackAnswerRequest.AnswerValueIds.Clear();
            feedbackAnswerRequest.AnswerValueIds.Add(answerId);
        }

        private bool SelectCheckBoxMultipleAnswer(MultiSelection.AnswerSelection answerSelection, QuestionResponse question)
        {
            //TO CHECK CORRECTNESS
            bool current = answerSelection.IsSelected = !answerSelection.IsSelected;
            ProvideFeedbackAnswerRequest feedbackAnswerRequest = FeedbackRequest.FeedbackAnswerRequests.FirstOrDefault(f => f.QuestionId == question.Id);
            if (current)
            {
                feedbackAnswerRequest.AnswerValueIds.Add(answerSelection.Id);
            }
            else
            {
                feedbackAnswerRequest.AnswerValueIds.Remove(answerSelection.Id);
            }

            return current;
        }

        private void GoToPreviousQuestion()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
            }

            userAnswer = FeedbackRequest.FeedbackAnswerRequests[currentIndex];
            currentQuestion = _feedbackResponse.Template.Questions.FirstOrDefault(q => q.Id == userAnswer.QuestionId);
            StateHasChanged();
        }

        private void GoToNextQuestion()
        {
            if (currentIndex < FeedbackRequest.FeedbackAnswerRequests.Count)
            {
                currentIndex++;
            }

            userAnswer = FeedbackRequest.FeedbackAnswerRequests[currentIndex];
            currentQuestion = _feedbackResponse.Template.Questions.FirstOrDefault(q => q.Id == userAnswer.QuestionId);
            StateHasChanged();
        }
    }
}
