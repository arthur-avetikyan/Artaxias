using System.ComponentModel.DataAnnotations.Schema;

namespace Artaxias.Data.Models.Feadback
{
    public class FeedbackAnswerValue
    {
        [ForeignKey("FeedbackAnswer")]
        public int FeedbackAnswerId { get; set; }

        public virtual FeedbackAnswer FeedbackAnswer { get; set; }

        [ForeignKey("AnswerValue")]
        public int AnswerValueId { get; set; }

        public virtual AnswerValue AnswerValue { get; set; }
    }
}
