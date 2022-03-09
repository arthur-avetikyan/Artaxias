
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class DepartmentRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 1000, MinimumLength = 1, ErrorMessage = "Department Name must have at least 1 charactor")]
        public string Name { get; set; }

        public List<int> Staff { get; set; }
    }
}
