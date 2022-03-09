using System.Collections.Generic;

namespace Artaxias.Web.Common.DataTransferObjects.Account
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int EmployeeId { get; set; }

        public IEnumerable<int> DeparmentIds { get; set; }

        public IEnumerable<string> Role { get; set; }
    }
}
