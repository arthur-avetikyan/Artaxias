using Artaxias.Core.Constants.Permissions.Dashboard;
using Artaxias.Core.Security.Authorization;
using Artaxias.Data;
using Artaxias.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Artaxias.Web.Server.Handlers
{
    public class SelfActionPermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<User> _userRepository;

        public SelfActionPermissionRequirementHandler(IRepository<User> userRepository,
                                                      IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                return;
            }

            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                return;
            }

            var user = await _userRepository.Get(u => u.UserName == context.User.Identity.Name)
                                            .Select(u => new
                                            {
                                                UserId = u.Id,
                                                EmployeeId = u.Employee == null ? 0 : u.Employee.Id
                                            })
                                            .FirstOrDefaultAsync();
            if (user == null)
            {
                return;
            }

            HttpRequest request = _httpContextAccessor.HttpContext.Request;

            if (requirement.Permission == Dashboard.Employee.Update)
            {
                string routeValue = request.RouteValues["id"] as string;
                if (int.TryParse(routeValue, out int employeeId) && request.RouteValues.Count == 3)
                {
                    if (user.EmployeeId == employeeId)
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return;
        }
    }
}
