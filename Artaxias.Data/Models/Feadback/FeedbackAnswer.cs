using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artaxias.Data.Models.Feadback
{
    [Table(nameof(FeedbackAnswer))]
    public partial class FeedbackAnswer
    {
        public FeedbackAnswer()
        {
            FeedbackAnswerValues = new List<FeedbackAnswerValue>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Feedback")]
        public int FeedbackId { get; set; }
        [Required]
        public Feedback Feedback { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        [Required]
        public virtual Question Question { get; set; }

        [InverseProperty("FeedbackAnswer")]
        public virtual IList<FeedbackAnswerValue> FeedbackAnswerValues { get; set; }

        public string OpenTextValue { get; set; }
    }
}
