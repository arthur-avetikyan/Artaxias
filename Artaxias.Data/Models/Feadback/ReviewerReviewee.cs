using Artaxias.Data.Models.Organization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artaxias.Data.Models.Feadback
{
    [Table(nameof(ReviewerReviewee))]
    public partial class ReviewerReviewee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Reviewer")]
        public int ReviewerId { get; set; }
        [InverseProperty("Reviewers")]
        public virtual Employee Reviewer { get; set; }

        [ForeignKey("Reviewee")]
        public int RevieweeId { get; set; }
        [InverseProperty("Reviewees")]
        public virtual Employee Reviewee { get; set; }

        [ForeignKey("Review")]
        public int ReviewId { get; set; }
        public virtual Review Review { get; set; }

        [ForeignKey("Feedback")]
        public int? FeedbackId { get; set; }
        public virtual Feedback Feadback { get; set; }

        [ForeignKey("DomainState")]
        public int DomainStateId { get; set; }
        [InverseProperty("ReviewerReviewees")]
        public virtual DomainState DomainState { get; set; }
    }
}
