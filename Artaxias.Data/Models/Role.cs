
using System;
using System.Collections.Generic;

namespace Artaxias.Models
{
    public partial class Role
    {

        public Role()
        {
            RolePermissions = new List<RolePermission>();
            UserRoles = new List<UserRole>();
            OnCreated();
        }

        public virtual int? CreatedByUserId { get; set; }

        public virtual DateTime CreatedDatetimeUTC { get; set; }

        public virtual int? UpdatedByUserId { get; set; }

        public virtual DateTime? UpdatedDatetimeUTC { get; set; }

        public virtual User User_CreatedByUserId { get; set; }

        public virtual User User_UpdatedByUserId { get; set; }

        public virtual IList<RolePermission> RolePermissions { get; set; }

        public virtual IList<UserRole> UserRoles { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
