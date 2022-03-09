using Artaxias.Models;
using Artaxias.Web.Common.DataTransferObjects.Account;
using Artaxias.Web.Common.DataTransferObjects.UserManagement;

using System.Collections.Generic;
using System.Linq;

namespace Artaxias.BusinessLogic.Users
{
    internal static class UsersMappingExtentions
    {
        internal static PermissionResponse Map(this Permission entity)
        {
            return new PermissionResponse
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name
            };
        }

        internal static IQueryable<PermissionResponse> Map(this IQueryable<Permission> entity)
        {
            return entity.Select(s => new PermissionResponse { Id = s.Id, Name = s.Name, Code = s.Code });
        }

        internal static RoleResponse Map(this Role entity)
        {
            RoleResponse role = new RoleResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Permissions = new List<PermissionResponse>()
            };

            foreach (RolePermission permission in entity.RolePermissions)
            {
                role.Permissions.Add(permission.Permission.Map());
            }

            return role;
        }

        public static UserDetails Map(this User entity)
        {
            return new UserDetails
            {
                UserName = entity.UserName,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName
            };
        }

        public static User Map(this RegisterParameters dto)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Email = dto.Email
            };
        }
    }
}
