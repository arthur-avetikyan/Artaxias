
using Artaxias.Web.Common.Attributes;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class ProvideFeedbackAnswerRequest
    {
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Question data is invalid.")]
        public int QuestionId { get; set; }

        [ValidateComplexType]
        public AnswerOptionTypeDetails OptionType { get; set; }

        [NullOrNotEmptyList(typeof(List<int>))]
        public List<int> AnswerValueIds { get; set; }

        [StringLength(maximumLength: 1000, MinimumLength = 1, ErrorMessage = "Open text answer can not be empty.")]
        public string OpenTextValue { get; set; }
    }
}