using System;

namespace Artaxias.Data.Models.Organization
{
    public partial class Bonus
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Comment { get; set; }

        public virtual DateTime CreatedDatetimeUTC { get; set; }

        public virtual DateTime? UpdatedDatetimeUTC { get; set; }

        public int ReceiverId { get; set; }

        public Employee Receiver { get; set; }

        public int ApproverId { get; set; }

        public Employee Approver { get; set; }

        public int RequesterId { get; set; }

        public Employee Requester { get; set; }

        public int DomainStateId { get; set; }

        public virtual DomainState State { get; set; }
    }
}
