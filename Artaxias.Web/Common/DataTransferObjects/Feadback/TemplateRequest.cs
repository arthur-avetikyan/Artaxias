using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class TemplateRequest
    {
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Template Title cannot be empty.")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Template Title must be at least 2 characters long.")]
        public string Title { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CreatedByUserId { get; set; }

        [ValidateComplexType]
        public List<QuestionRequest> Questions { get; set; }
    }
}
