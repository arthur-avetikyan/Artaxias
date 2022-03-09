using System;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Data.Models.Organization
{
    public partial class Absence
    {
        [Key]
        public int Id { get; set; }

        public string Reason { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TypeId { get; set; }

        public AbsenceType Type { get; set; }

        public int ReceiverId { get; set; }

        public Employee Receiver { get; set; }

        // ReSharper disable once IdentifierTypo
        public int ApproverId { get; set; }

        public virtual DateTime CreatedDatetimeUTC { get; set; }

        public virtual DateTime? UpdatedDatetimeUTC { get; set; }

        // ReSharper disable once IdentifierTypo
        public Employee Approver { get; set; }

        public int DomainStateId { get; set; }

        public virtual DomainState State { get; set; }
    }
}
