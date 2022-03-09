
using System;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class BonusResponse
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Comment { get; set; }

        public EmployeeInfo Receiver { get; set; }

        public EmployeeInfo Approver { get; set; }

        public EmployeeInfo Requester { get; set; }

        public int DomainStateId { get; set; }
    }
}
