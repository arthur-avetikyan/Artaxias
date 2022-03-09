using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class EndContractRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int EmployeeId { get; set; }

        [DisplayName("Contract End Date")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/1980", "1/1/2100", ErrorMessage = "Date input is invalid")]
        [Required]
        public DateTime ContractEndDate { get; set; }
    }
}
