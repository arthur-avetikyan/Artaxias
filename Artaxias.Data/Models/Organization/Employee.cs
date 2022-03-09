using Artaxias.Data.Models.Feadback;
using Artaxias.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artaxias.Data.Models.Organization
{
    public partial class Employee
    {
        public Employee()
        {
            Salaries = new List<Salary>();
            AbsencesAssigned = new List<Absence>();
            Reviewers = new List<ReviewerReviewee>();
            Reviewees = new List<ReviewerReviewee>();
            Departments = new List<EmployeeDepartment>();
        }

        [Key]
        public int Id { get; set; }

        public string Position { get; set; }

        public DateTime ContractStart { get; set; }

        public DateTime? ContractEnd { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public virtual IList<EmployeeDepartment> Departments { get; set; }

        public virtual IList<Salary> Salaries { get; set; }

        public virtual IList<Absence> AbsencesAssigned { get; set; }

        public virtual IList<Absence> AbsencesToApprove { get; set; }

        public virtual IList<Bonus> BonusesAssigned { get; set; }

        public virtual IList<Bonus> BonusesToApprove { get; set; }

        public virtual IList<Bonus> BonusesRequested { get; set; }

        [InverseProperty("Reviewer")]
        public virtual IList<ReviewerReviewee> Reviewers { get; set; }

        [InverseProperty("Reviewee")]
        public virtual IList<ReviewerReviewee> Reviewees { get; set; }
    }
}
