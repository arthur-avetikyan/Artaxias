using System;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class SalaryResponse
    {
        public int Id { get; set; }

        public double NetAmount { get; set; }

        public double GrossAmount { get; set; }

        public DateTime AssignmentDate { get; set; }
    }
}
