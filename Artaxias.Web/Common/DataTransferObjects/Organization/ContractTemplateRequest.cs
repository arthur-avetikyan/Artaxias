using Artaxias.Web.Common.DataTransferObjects.File;

using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class ContractTemplateRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Creator must be selected.")]
        public int CreatorId { get; set; }

        [Required]
        public FileDto Document { get; set; } = new FileDto();
    }
}
