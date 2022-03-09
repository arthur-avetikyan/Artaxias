using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Collections.Generic;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class FeedbackAboutEmployeeResponse
    {
        public int Id { get; set; }

        public int ReviewerRevieweeId { get; set; }

        public EmployeeInfo Reviewee { get; set; }

        public EmployeeInfo Reviewer { get; set; }

        public List<FeedbackQuestionAnswerResponse> QuestionAnswers { get; set; }
    }
}
