using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class SalaryRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int EmployeeId { get; set; }

        [Required]
        [DisplayName("NET Salary")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue)]
        public double NetSalary { get; set; }

        [Required]
        [DisplayName("Gross Salary")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue)]
        public double GrossSalary { get; set; }

        [Required]
        [DisplayName("Assignment Date")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/1990", "1/1/2100", ErrorMessage = "Date input is invalid")]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "yyyy-MM-dd")]
        public DateTime AssignmentDate { get; set; }
    }
}
