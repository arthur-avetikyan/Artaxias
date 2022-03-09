using System.Collections.Generic;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class FeedbackQuestionAnswerResponse
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public List<string> AnswerValues { get; set; }
    }
}