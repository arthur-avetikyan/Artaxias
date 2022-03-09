
using System;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class AbsenceResponse
    {
        public int Id { get; set; }

        public EmployeeInfo Receiver { get; set; }

        public string Reason { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public EmployeeInfo Approver { get; set; }

        public int TypeId { get; set; }

        public string TypeDescription { get; set; }

        public int StateId { get; set; }
    }
}
