using System.Collections.Generic;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class DepartmentInfo
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public List<int> Employees { get; set; }
    }
}
