using Artaxias.Core.Constants.Domain;
using Artaxias.Web.Client.Extensions;
using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Feadback;
using Artaxias.Web.Common.DataTransferObjects.Organization;
using Artaxias.Web.Common.Utilities;

using MatBlazor;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.Feadback
{
    public partial class Template
    {
        [Inject] private IMatToaster MatToaster { get; set; }
        [Inject] private IDepartmentApiAccessor DepartmentApiAccessor { get; set; }
        [Inject] private IUsersApiAccessor UsersApiAccessor { get; set; }
        [Inject] private ITemplateApiAccessor TemplateApiAccessor { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; }
        [Parameter] public int TemplateId { get; set; }

        private TemplateRequest TemplateRequest { get; set; }
        private bool _isEditDisabled;
        private string _returnUrl;
        private string _currentUserName;
        private TemplateResponse _feadbackTemplate;
        private List<DepartmentResponse> _departments;
        private List<AnswerOptionTypeDetails> _answerOptionTypes;
        private bool _isInsertOperation;
        private string _actionLabel;
        private bool _panelOpenState;

        public Template() : base()
        {
            TemplateRequest = new TemplateRequest { Questions = new List<QuestionRequest>(1) };
            _departments = new List<DepartmentResponse>();
            _panelOpenState = true;
            _isEditDisabled = false;
        }

        protected override async Task OnInitializedAsync()
        {
            string query = new Uri(NavigationManager.Uri).Query;
            _isEditDisabled = query.GetBoolFromQuery("isEditDisabled");
            _returnUrl = query.GetReturnUrlFromQuery();

            _currentUserName = (await AuthenticationState).User.Identity.Name;
            _answerOptionTypes = await TemplateApiAccessor.GetAnswerOptionTypes();

            if (TemplateId == 0)
            {
                await SetUpInsertOperation();
            }
            else
            {
                await SetUpEditOperation();
            }

            await InitializeDepartmentsAsync();
        }

        private async Task InitializeDepartmentsAsync()
        {
            try
            {
                _departments = await DepartmentApiAccessor.GetDepartmentListAsync(pageSize: int.MaxValue);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "Department Retrieval Error");
            }
        }

        private async Task SetUpInsertOperation()
        {
            _actionLabel = "Create";
            _feadbackTemplate = new TemplateResponse();
            TemplateRequest = new TemplateRequest
            {
                CreatedByUserId = (await UsersApiAccessor.GetAsync(_currentUserName)).Id,
                Questions = new List<QuestionRequest>
                {
                    new QuestionRequest
                    {
                        Answer = new AnswerRequest
                        {
                            OptionType = _answerOptionTypes.FirstOrDefault(t=>t.Description==AnswerOptionTypes.OpenText),
                            Values = new List<AnswerValueRequest>()
                        }
                    }
                }
            };
            _isInsertOperation = true;
        }

        private async Task SetUpEditOperation()
        {
            _actionLabel = "Edit";
            _feadbackTemplate = await TemplateApiAccessor.GetFeadbackTemplateAsync(TemplateId);
            TemplateRequest = _feadbackTemplate.Map();
            _isInsertOperation = false;
        }

        private async Task SaveTemplate()
        {
            try
            {
                if (_isInsertOperation)
                {
                    await TemplateApiAccessor.CreateFeadbackTemplateAsync(TemplateRequest);
                    MatToaster.Add("Success", MatToastType.Success);
                    NavigationManager.NavigateTo("templates");
                }
                else
                {
                    await TemplateApiAccessor.UpdateFeadBackTemplateAsync(TemplateRequest);
                    MatToaster.Add("Success", MatToastType.Success);
                    NavigationManager.NavigateTo("templates");
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger, "Failed");
            }
        }

        private void AddQuestion()
        {
            TemplateRequest.Questions.Add(new QuestionRequest
            {
                Answer = new AnswerRequest
                {
                    OptionType = _answerOptionTypes.FirstOrDefault(t => t.Description == AnswerOptionTypes.OpenText),
                    Values = new List<AnswerValueRequest>()
                }
            });
            StateHasChanged();
        }

        private void RemoveQuestion(int questionIndex)
        {
            if (questionIndex >= 0 && questionIndex < TemplateRequest.Questions.Count)
            {
                TemplateRequest.Questions.RemoveAt(questionIndex);
            }
        }

        private void SetInistialOptionCount(int optionTypeId, QuestionRequest questionRequest)
        {
            questionRequest.Answer.OptionType = _answerOptionTypes.FirstOrDefault(t => t.Id == optionTypeId);

            if (questionRequest.Answer.OptionType.Description != AnswerOptionTypes.OpenText && questionRequest.Answer.Values?.Count < 2)
            {
                questionRequest.Answer.Values = new List<AnswerValueRequest> { new AnswerValueRequest { Value = "" }, new AnswerValueRequest { Value = "" } };
            }
            else if (questionRequest.Answer.OptionType.Description == AnswerOptionTypes.OpenText)
            {
                questionRequest.Answer.Values = new List<AnswerValueRequest>();
            }
        }

        private void ChangeAnswerValue(int optionIndex, string option, QuestionRequest questionRequest)
        {
            questionRequest.Answer.Values[optionIndex].Value = option;
        }

        private void AddOption(QuestionRequest questionRequest)
        {
            questionRequest.Answer.Values.Add(new AnswerValueRequest { Value = "" });
        }

        private void RemoveOption(int optionIndex, QuestionRequest questionRequest)
        {
            if (optionIndex >= 0 && optionIndex < questionRequest.Answer.Values.Count)
            {
                questionRequest.Answer.Values.RemoveAt(optionIndex);
            }
        }
    }
}