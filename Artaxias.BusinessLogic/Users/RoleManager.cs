
using Artaxias.Data;
using Artaxias.Models;
using Artaxias.Web.Common.DataTransferObjects.UserManagement;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Artaxias.BusinessLogic.Users
{
    public class RoleManager : IRoleManager
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IRepository<RolePermission> _rolePermissionRepository;

        public RoleManager(IRepository<Role> roleRepository,
                            IRepository<Permission> permissionRepository,
                            IRepository<RolePermission> rolePermissionRepository,
                            RoleManager<Role> roleManager,
                            UserManager<User> userManager)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<RoleResponse> GetAsync(string key)
        {
            RoleResponse role = await _roleRepository.Get(r => r.Name == key)
                                              .Select(s => new RoleResponse
                                              {
                                                  Id = s.Id,
                                                  Name = s.Name,
                                                  Permissions = s.RolePermissions.Select(sm => sm.Permission.Map()).ToList()
                                              })
                                              .SingleOrDefaultAsync();
            return role;
        }

        public async Task<List<RoleResponse>> GetListAsync(int pageSize, int currentPage)
        {
            List<Role> roles = await _roleRepository.Get()
                                             .Skip(currentPage * pageSize)
                                             .Take(pageSize)
                                             .Include(i => i.RolePermissions)
                                             .ThenInclude(t => t.Permission)
                                             .ToListAsync();

            List<RoleResponse> roleDtoList = new();
            foreach (Role role in roles)
            {
                roleDtoList.Add(new RoleResponse
                {
                    Id = role.Id,
                    Name = role.Name,
                    Permissions = role.RolePermissions.Select(s => s.Permission.Map()).ToList()
                });
            }
            return roleDtoList;
        }

        public async Task<RoleResponse> CreateAsync(RoleRequest request)
        {
            if (_roleRepository.Get().Any(r => r.Name == request.Name))
            {
                throw new ArgumentException("Role already exists");
            }

            IdentityResult lResult = await _roleManager.CreateAsync(new Role { Name = request.Name });

            if (!lResult.Succeeded)
            {
                string lErrorMessage = lResult.Errors.Select(x => x.Description).Aggregate((i, j) => i + " - " + j);
                throw new ArgumentException(lErrorMessage);
            }

            Role role = await _roleManager.FindByNameAsync(request.Name);

            foreach (string permission in request.Permissions)
            {
                _rolePermissionRepository.Insert(new RolePermission
                {
                    PermissionId = await _permissionRepository.Get(x => x.Name.Contains(permission)).Select(p => p.Id).FirstOrDefaultAsync(),
                    RoleId = role.Id
                });
            }
            await _rolePermissionRepository.SaveChangesAsync();
            return role.Map();
        }

        public async Task<RoleResponse> UpdateAsync(string key, RoleRequest request)
        {
            Role role = await _roleRepository.Get(r => r.Id == request.Id).Include(i => i.RolePermissions).ThenInclude(rp => rp.Permission).FirstOrDefaultAsync();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role), "This role doesn't exist");
            }

            role.Name = request.Name;
            role.NormalizedName = request.Name.ToUpper();

            IEnumerable<string> newPermissions = request.Permissions.Where(p => !role.RolePermissions.Any(rp => p.Equals(rp.Permission.Name, StringComparison.OrdinalIgnoreCase)));
            IEnumerable<RolePermission> formerPermissions = role.RolePermissions.Where(rp => !request.Permissions.Any(p => rp.Permission.Name.Equals(p, StringComparison.OrdinalIgnoreCase)));

            foreach (RolePermission item in formerPermissions)
            {
                _rolePermissionRepository.Delete(item);
            }

            List<RolePermission> newRolePermissions = new();
            foreach (string item in newPermissions)
            {
                Permission permission = await _permissionRepository.Get(x => x.Name.Equals(item)).FirstOrDefaultAsync();
                if (permission != null)
                {
                    newRolePermissions.Add(new RolePermission { PermissionId = permission.Id });
                }
            }

            foreach (RolePermission item in newRolePermissions)
            {
                role.RolePermissions.Add(item);
            }

            _roleRepository.Update(role);
            await _roleRepository.SaveChangesAsync();
            await _rolePermissionRepository.SaveChangesAsync();

            return role.Map();
        }

        public async Task DeleteAsync(string key)
        {
            IList<User> lUsers = await _userManager.GetUsersInRoleAsync(key);
            if (lUsers.Any())
            {
                throw new ArgumentException("This role is still used by a user, you cannot delete it");
            }

            Role lRole = await _roleManager.FindByNameAsync(key);
            await _roleManager.DeleteAsync(lRole);
        }
    }
}
