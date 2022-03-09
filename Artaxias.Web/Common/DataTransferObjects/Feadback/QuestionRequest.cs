using System;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class QuestionRequest
    {
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Question Title cannot be empty.")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Question Title must be at least 2 characters long.")]
        public string Title { get; set; }

        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Question Description must be at least 2 characters long.")]
        public string Description { get; set; }

        [ValidateComplexType]
        public AnswerRequest Answer { get; set; } = new AnswerRequest();
    }
}
