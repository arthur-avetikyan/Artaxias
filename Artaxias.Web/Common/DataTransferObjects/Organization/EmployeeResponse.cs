using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class EmployeeResponse
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Position { get; set; }

        [DisplayFormat(DataFormatString = "MMMM dd, yyyy")]
        public DateTime ContractStart { get; set; }

        [DisplayFormat(DataFormatString = "MMMM dd, yyyy")]
        public DateTime? ContractEnd { get; set; }

        public List<DepartmentInfo> Departments { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public bool DisplayMore { get; set; } = false;
    }
}
