namespace Artaxias.Data.Models.Organization
{
    public partial class EmployeeDepartment
    {
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }
    }
}