using Artaxias.Data;
using Artaxias.Models;
using Artaxias.Web.Common.DataTransferObjects.UserManagement;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Users
{
    public class PermissionManager : IPermissionManager
    {
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IRepository<RolePermission> _rolePermissionRepository;

        public PermissionManager(IRepository<Permission> permissionRepository,
                            IRepository<RolePermission> rolePermissionRepository)
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<PermissionResponse> GetAsync(int key)
        {
            return await _permissionRepository.Get(p => p.Id == key).Map().FirstOrDefaultAsync();
        }

        public async Task<List<PermissionResponse>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            return await _permissionRepository.Get().Map().ToListAsync();
        }

        public async Task<PermissionResponse> CreateAsync(PermissionRequest request)
        {
            if (_permissionRepository.Get().Any(r => r.Name == request.Name || r.Code == request.Code))
            {
                throw new ArgumentException("Permission already exists");
            }

            Permission permission = new Permission { Name = request.Name, Code = request.Code };
            _permissionRepository.Insert(permission);
            await _permissionRepository.SaveChangesAsync();
            return permission.Map();
        }

        public async Task DeleteAsync(int key)
        {
            if (_rolePermissionRepository.Get().Any(p => p.Permission.Id == key))
            {
                throw new ArgumentException("This permission is still used by a role, you cannot delete it");
            }

            Permission lPermission = await _permissionRepository.Get(p => p.Id == key).FirstOrDefaultAsync();
            _permissionRepository.Delete(lPermission);
            await _permissionRepository.SaveChangesAsync();
        }

        public Task<PermissionResponse> UpdateAsync(int key, PermissionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
