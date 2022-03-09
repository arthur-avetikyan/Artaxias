using Artaxias.Data.Models.Organization;

namespace Artaxias.Data.Models.Feadback
{
    public partial class ReviewDepartment
    {
        public int ReviewId { get; set; }

        public virtual Review Review { get; set; }

        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }
    }
}