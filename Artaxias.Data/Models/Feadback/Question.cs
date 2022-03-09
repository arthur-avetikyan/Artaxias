using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artaxias.Data.Models.Feadback
{
    [Table(nameof(Question))]
    public partial class Question
    {
        public Question()
        {
            AnswerOption = new AnswerOption();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [InverseProperty("Questions")]
        public int FeedbackTemplateId { get; set; }

        [Required]
        public virtual FeedbackTemplate FeedbackTemplate { get; set; }

        [InverseProperty("Question")]
        public virtual AnswerOption AnswerOption { get; set; }
    }
}
