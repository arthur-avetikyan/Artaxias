using Artaxias.Web.Common.DataTransferObjects.Feadback;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Collections.Generic;

namespace Artaxias.Web.Common.Utilities
{
    public static class MappingExtensions
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

        public static AbsenceRequest Map(this AbsenceResponse response)
        {
            return new AbsenceRequest
            {
                ReceiverId = response.Receiver.Id,
                Reason = response.Reason,
                StartDate = response.StartDate,
                EndDate = response.EndDate,
                ApproverId = response.Approver.Id,
                TypeId = response.TypeId
            };
        }
    }
}
