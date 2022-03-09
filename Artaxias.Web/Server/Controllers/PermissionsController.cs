using Artaxias.BusinessLogic.Users;
using Artaxias.Core.Constants.Permissions.Dashboard;
using Artaxias.Web.Common.DataTransferObjects.UserManagement;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //[Produces("application/json")]
    //[Consumes("application/json")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionManager _permissionManager;

        public PermissionsController(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        /// <summary>
        /// Get all permissions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Dashboard.Role.Read)]
        public async Task<ActionResult<List<PermissionResponse>>> Get()
        {
            try
            {
                List<PermissionResponse> result = await _permissionManager.GetListAsync(int.MaxValue, 0);
                if (result == null)
                {
                    return NotFound("Retrival Failed");
                }

                return result;
            }
            catch (Exception)
            {
                return BadRequest("Retrival Failed");
            }
        }
    }
}
