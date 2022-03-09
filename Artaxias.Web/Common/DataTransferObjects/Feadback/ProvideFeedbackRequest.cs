
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class ProvideFeedbackRequest
    {
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Error because of Id mismatch.")]
        public int ReviewerRevieweeId { get; set; }

        [ValidateComplexType]
        public List<ProvideFeedbackAnswerRequest> FeedbackAnswerRequests { get; set; }
    }
}