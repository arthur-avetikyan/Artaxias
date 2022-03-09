
using System.Collections.Generic;

namespace Artaxias.Models
{
    public partial class Permission
    {

        public Permission()
        {
            RolePermissions = new List<RolePermission>();
            OnCreated();
        }

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual IList<RolePermission> RolePermissions { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
