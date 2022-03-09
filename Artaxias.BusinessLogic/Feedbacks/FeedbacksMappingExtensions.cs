using Artaxias.Web.Common.DataTransferObjects.Feadback;

using System.Collections.Generic;

namespace Artaxias.BusinessLogic.Feedbacks
{
    internal static class FeedbacksMappingExtensions
    {
        public static TemplateRequest Map(this TemplateResponse response)
        {
            TemplateRequest request = new TemplateRequest
            {
                Id = response.Id,
                Title = response.Title,
                CreatedByUserId = response.CreatedByUserId,
                Questions = new List<QuestionRequest>()
            };
            foreach (QuestionResponse item in response.Questions)
            {
                request.Questions.Add(item.Map());
            }
            return request;
        }

        public static QuestionRequest Map(this QuestionResponse response)
        {
            QuestionRequest questionRequest = new QuestionRequest
            {
                Id = response.Id,
                Title = response.Title,
                Description = response.Description,
                Answer = new AnswerRequest
                {
                    Id = response.Answer.Id,
                    OptionType = response.Answer.OptionType,
                    Values = new List<AnswerValueRequest>()
                }
            };
            foreach (AnswerValueResponse item in response.Answer.Values)
            {
                questionRequest.Answer.Values.Add(new AnswerValueRequest { Value = item.Value });
            }
            return questionRequest;
        }
    }
}
