
namespace Artaxias.Models
{
    public partial class RolePermission
    {

        public RolePermission()
        {
            OnCreated();
        }

        public virtual int RoleId { get; set; }

        public virtual int PermissionId { get; set; }

        public virtual Role Role { get; set; }

        public virtual Permission Permission { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
