using Artaxias.Core.Enums;

using System.ComponentModel;

namespace Artaxias.Core.Constants.Permissions.Dashboard
{
    public static partial class Dashboard
    {
        public static class Department
        {
            [Description("Create Department")]
            public const string Create = nameof(Dashboard) + "." + nameof(Department) + "." + nameof(EPermissionAction.Create);

            [Description("Read Department data")]
            public const string Read = nameof(Dashboard) + "." + nameof(Department) + "." + nameof(EPermissionAction.Read);

            [Description("Edit Department")]
            public const string Update = nameof(Dashboard) + "." + nameof(Department) + "." + nameof(EPermissionAction.Update);

            [Description("Delete Department")]
            public const string Delete = nameof(Dashboard) + "." + nameof(Department) + "." + nameof(EPermissionAction.Delete);
        }
    }
}
