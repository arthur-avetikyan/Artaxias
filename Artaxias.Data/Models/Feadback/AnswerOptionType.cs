using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artaxias.Data.Models.Feadback
{
    [Table(nameof(AnswerOptionType))]
    public class AnswerOptionType
    {
        public AnswerOptionType()
        {
            AnswerOptions = new List<AnswerOption>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [InverseProperty("OptionType")]
        public virtual IList<AnswerOption> AnswerOptions { get; set; }
    }
}
