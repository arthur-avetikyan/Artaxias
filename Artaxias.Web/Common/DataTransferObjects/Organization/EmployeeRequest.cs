
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class EmployeeRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Employee must be selected.")]
        public int Id { get; set; }

        public List<int> DepartmentIds { get; set; } = new List<int>();

        [DisplayName("Position")]
        [StringLength(100, MinimumLength = 2)]
        public string Position { get; set; }

        [Required]
        [DisplayName("Contract Start Date")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/1990", "1/1/2100", ErrorMessage = "Date input is invalid")]
        public DateTime ContractStart { get; set; }
    }
}
