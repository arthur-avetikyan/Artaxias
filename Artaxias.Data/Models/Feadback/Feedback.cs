using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artaxias.Data.Models.Feadback
{
    [Table(nameof(Feedback))]
    public partial class Feedback
    {
        public Feedback()
        {
            FeedbackAnswers = new List<FeedbackAnswer>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime ProvidedAt { get; set; }

        [ForeignKey("ReviewerReviewee")]
        public int ReviewerRevieweeId { get; set; }
        public virtual ReviewerReviewee ReviewerReviewee { get; set; }

        [InverseProperty("Feedback")]
        public virtual IList<FeedbackAnswer> FeedbackAnswers { get; set; }
    }
}
