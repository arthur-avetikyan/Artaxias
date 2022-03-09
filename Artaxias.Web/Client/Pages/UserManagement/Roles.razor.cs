using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.UserManagement;

using MatBlazor;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Pages.UserManagement
{
    public partial class Roles
    {
        [Inject] private IRoleApiAccessor RoleApiAccessor { get; set; }
        [Inject] private IMatToaster MatToaster { get; set; }

        private int PageSize { get; set; } = 10;
        private int CurrentPage { get; set; } = 0;

        private int _currentRoleId = 0;
        private string _currentRoleName = "";
        private readonly bool _isCurrentRoleReadOnly = false;
        private bool _isDeleteDialogOpen = false;
        private bool _isUpsertDialogOpen = false;
        private readonly List<PermissionSelection> _permissionsSelections = new List<PermissionSelection>();

        private bool _isInsertOperation;
        private string _labelUpsertDialogTitle;
        private List<RoleResponse> _roles;

        public class PermissionSelection
        {
            public bool IsSelected { get; set; }
            public string Name { get; set; }
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await InitializeRolesListAsync();
            SortData(null);
        }

        private async Task InitializeRolesListAsync()
        {
            try
            {
                _roles = await RoleApiAccessor.GetRolesAsync(PageSize, CurrentPage);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void SortData(MatSortChangedEvent sort)
        {
            if (!(sort == null || sort.Direction == MatSortDirection.None || string.IsNullOrEmpty(sort.SortId)))
            {
                Comparison<RoleResponse> comparison = null;
                switch (sort.SortId)
                {
                    case "role":
                        comparison = (s1, s2) => string.Compare(s1.Name, s2.Name, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "permissions":
                        comparison = (s1, s2) => s1.Permissions.Count.CompareTo(s2.Permissions.Count);
                        break;
                }
                if (comparison != null)
                {
                    if (sort.Direction == MatSortDirection.Desc)
                    {
                        _roles.Sort((s1, s2) => -1 * comparison(s1, s2));
                    }
                    else
                    {
                        _roles.Sort(comparison);
                    }
                }
            }
        }

        private async Task OpenUpsertRoleDialog(int roleId = 0, string roleName = "")
        {
            try
            {
                _currentRoleName = roleName;
                _currentRoleId = roleId;

                _isInsertOperation = string.IsNullOrWhiteSpace(roleName) && roleId == 0;

                if (_isInsertOperation)
                {
                    _labelUpsertDialogTitle = "Create Role";
                }
                else
                {
                    _labelUpsertDialogTitle = "Edit Role";
                }

                RoleResponse role = null;
                if (!_isInsertOperation)
                {
                    role = await RoleApiAccessor.GetRoleAsync(_currentRoleName);
                }

                List<PermissionResponse> premissions = await RoleApiAccessor.GetPremissions();
                _permissionsSelections.Clear();
                foreach (PermissionResponse permission in premissions)
                {
                    PermissionSelection newPermissionsSelection = new PermissionSelection
                    {
                        Name = permission.Name,
                        IsSelected = role != null && role.Permissions.Any(p => p.Name.Equals(permission.Name))
                    };
                    _permissionsSelections.Add(newPermissionsSelection);
                }
                _isUpsertDialogOpen = true;
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void OpenDeleteDialog(string roleName)
        {
            _currentRoleName = roleName;
            _isDeleteDialogOpen = true;
        }

        private async Task UpsertRole()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_currentRoleName) && _currentRoleId > 0)
                {
                    MatToaster.Add("Role Creation Error", MatToastType.Danger, "Enter in a Role Name");
                    return;
                }

                RoleRequest request = new RoleRequest
                {
                    Id = _currentRoleId,
                    Name = _currentRoleName,
                    Permissions = new List<string>()
                };

                foreach (PermissionSelection status in _permissionsSelections)
                {
                    if (status.IsSelected)
                    {
                        request.Permissions.Add(status.Name);
                    }
                }

                if (_isInsertOperation)
                {
                    await CreateRoleAsync(request);
                }
                else
                {
                    await UpdateRoleAsync(request);
                }

                await OnInitializedAsync();
                _isUpsertDialogOpen = false;
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task CreateRoleAsync(RoleRequest request)
        {
            try
            {
                string response = await RoleApiAccessor.CreateRoleAsync(request);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task UpdateRoleAsync(RoleRequest request)
        {
            try
            {
                RoleResponse response = await RoleApiAccessor.UpdateRoleAsync(request.Id, request);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private async Task DeleteRoleAsync()
        {
            try
            {
                await RoleApiAccessor.DeleteRoleAsync(_currentRoleName);
                MatToaster.Add("Role Deleted", MatToastType.Success);
                await OnInitializedAsync();
                _isDeleteDialogOpen = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.Message, MatToastType.Danger);
            }
        }

        private void ShowMore(int roleId)
        {
            RoleResponse role = _roles.FirstOrDefault(r => r.Id == roleId);
            role.DisplayMore = !role.DisplayMore;
            StateHasChanged();
        }
    }
}
