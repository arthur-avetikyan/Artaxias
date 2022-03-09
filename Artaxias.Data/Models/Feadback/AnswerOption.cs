using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artaxias.Data.Models.Feadback
{
    [Table(nameof(AnswerOption))]
    public partial class AnswerOption
    {
        public AnswerOption()
        {
            Values = new List<AnswerValue>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("AnswerOptionType")]
        public int AnswerOptionTypeId { get; set; }
        [InverseProperty("AnswerOptions")]
        public virtual AnswerOptionType OptionType { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public virtual Question Question { get; set; }

        [InverseProperty("AnswerOption")]
        public virtual IList<AnswerValue> Values { get; set; }
    }
}
