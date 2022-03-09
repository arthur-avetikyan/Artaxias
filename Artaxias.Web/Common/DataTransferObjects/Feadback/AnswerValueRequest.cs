using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class AnswerValueRequest
    {
        [Required(ErrorMessage = "Answer Option cannot be empty.")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Answer Option cannot be empty.")]
        public string Value { get; set; }
    }
}