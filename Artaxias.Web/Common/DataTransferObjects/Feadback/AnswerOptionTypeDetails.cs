using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class AnswerOptionTypeDetails
    {
        [Required]
        [Range(minimum: 1, maximum: 5, ErrorMessage = "Id of Answer Option Type is incorrect.")]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 7, ErrorMessage = "Description of Answer Option Type is incorrect.")]
        public string Description { get; set; }
    }
}
