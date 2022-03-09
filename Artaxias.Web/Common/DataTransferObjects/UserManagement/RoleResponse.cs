using System.Collections.Generic;

namespace Artaxias.Web.Common.DataTransferObjects.UserManagement
{
    public class RoleResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<PermissionResponse> Permissions { get; set; }

        public bool DisplayMore { get; set; } = false;
    }
}
