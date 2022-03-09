using System.Collections.Generic;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class DepartmentResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<EmployeeInfo> Staff { get; set; }

        public bool DisplayMore { get; set; } = false;
    }
}
