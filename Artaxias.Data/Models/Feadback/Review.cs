
using Artaxias.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artaxias.Data.Models.Feadback
{
    [Table(nameof(Review))]
    public partial class Review
    {
        public Review()
        {
            ReviewerReviewees = new List<ReviewerReviewee>();
            Departments = new List<ReviewDepartment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [ForeignKey("User")]
        public int CreatedByUserId { get; set; }

        public virtual User CreatedByUser { get; set; }

        [ForeignKey("FeadbackTemplate")]
        public int FeedbackTemplateId { get; set; }

        public virtual FeedbackTemplate FeedbackTemplate { get; set; }

        [Required]
        public DateTime CreatedDateTimeUTC { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public virtual IList<ReviewerReviewee> ReviewerReviewees { get; set; }

        public virtual IList<ReviewDepartment> Departments { get; set; }
    }
}
