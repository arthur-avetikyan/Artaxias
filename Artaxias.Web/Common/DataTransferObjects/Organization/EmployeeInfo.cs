using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class EmployeeInfo
    {
        [Range(1, int.MaxValue, ErrorMessage = "Employee is not selected.")]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<int> Departments { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}