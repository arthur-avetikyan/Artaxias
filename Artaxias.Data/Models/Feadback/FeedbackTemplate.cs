using Artaxias.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artaxias.Data.Models.Feadback
{
    [Table(nameof(FeedbackTemplate))]
    public partial class FeedbackTemplate
    {
        public FeedbackTemplate()
        {
            Questions = new List<Question>();
            Reviews = new List<Review>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [InverseProperty("User")]
        public int CreatedByUserId { get; set; }

        public virtual User CreatedByUser { get; set; }

        [InverseProperty("FeedbackTemplate")]
        [Required]
        public virtual IList<Question> Questions { get; set; }

        [InverseProperty("FeedbackTemplate")]
        public virtual IList<Review> Reviews { get; set; }
    }
}
