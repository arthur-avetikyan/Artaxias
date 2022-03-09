using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artaxias.Data.Models.Feadback
{
    [Table(nameof(AnswerValue))]
    public partial class AnswerValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Value { get; set; }

        [Required]
        public int AnswerOptionId { get; set; }

        [Required]
        public virtual AnswerOption AnswerOption { get; set; }

        [InverseProperty("AnswerValue")]
        public virtual IList<FeedbackAnswerValue> FeedbackAnswerValues { get; set; }
    }
}
