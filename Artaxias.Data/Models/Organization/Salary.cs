using System;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Data.Models.Organization
{
    public partial class Salary
    {
        [Key]
        public int Id { get; set; }

        public double GrossAmount { get; set; }

        public double NetAmount { get; set; }

        public DateTime AssignmentDate { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}