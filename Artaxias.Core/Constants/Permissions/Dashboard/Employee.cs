using Artaxias.Core.Enums;

using System.ComponentModel;

namespace Artaxias.Core.Constants.Permissions.Dashboard
{
    public partial class Dashboard
    {
        public static class Employee
        {
            [Description("Create Employee")]
            public const string Create = nameof(Dashboard) + "." + nameof(Employee) + "." + nameof(EPermissionAction.Create);

            [Description("Read Employee data")]
            public const string Read = nameof(Dashboard) + "." + nameof(Employee) + "." + nameof(EPermissionAction.Read);

            [Description("Edit Employee")]
            public const string Update = nameof(Dashboard) + "." + nameof(Employee) + "." + nameof(EPermissionAction.Update);

            [Description("Delete Employee")]
            public const string Delete = nameof(Dashboard) + "." + nameof(Employee) + "." + nameof(EPermissionAction.Delete);
        }
    }
}
