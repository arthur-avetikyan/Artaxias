
using Artaxias.Data.Models.Feadback;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Data.Models.Organization
{
    public partial class Department
    {
        public Department()
        {
            Staff = new List<EmployeeDepartment>();
            Reviews = new List<ReviewDepartment>();
        }

        [Key]
        public int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<EmployeeDepartment> Staff { get; set; }

        public virtual IList<ReviewDepartment> Reviews { get; set; }
    }
}