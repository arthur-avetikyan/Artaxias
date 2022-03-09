using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class ContractMappings
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Contract Template is not selected.")]
        public int ContractTemplateId { get; set; }

        public string Title { get; set; }

        public Dictionary<string, string> Mappings { get; set; } = new Dictionary<string, string>();
    }
}