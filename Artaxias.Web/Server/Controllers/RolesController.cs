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
    public class RolesController : ControllerBase
    {
        private readonly IRoleManager _roleManager;

        public RolesController(IRoleManager adminManager)
        {
            _roleManager = adminManager;
        }

        /// <summary>
        /// Get all user roles
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Dashboard.Role.Read)]
        public async Task<ActionResult<List<RoleResponse>>> Get([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
        {
            try
            {
                List<RoleResponse> result = await _roleManager.GetListAsync(pageSize, pageNumber);
                if (result == null)
                {
                    return NotFound("Retrival Failed");
                }

                return result;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get user role by name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet("{roleName}")]
        [Authorize(Dashboard.Role.Read)]
        public async Task<ActionResult<RoleResponse>> Get(string roleName)
        {
            try
            {
                RoleResponse result = await _roleManager.GetAsync(roleName);
                if (result == null)
                {
                    return NotFound("Retrival Failed");
                }

                return result;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Create a new user role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Dashboard.Role.Create)]
        public async Task<IActionResult> Create([FromBody] RoleRequest role)
        {
            try
            {
                RoleResponse result = await _roleManager.CreateAsync(role);
                if (result == null)
                {
                    return BadRequest();
                }

                return Created(nameof(Get), result.Id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Update a user role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Dashboard.Role.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] RoleRequest request)
        {
            if (id != request.Id)
            {
                return new StatusCodeResult(409);
            }

            try
            {
                RoleResponse result = await _roleManager.UpdateAsync(request.Name, request);
                if (result == null)
                {
                    return BadRequest();
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete a user role
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("{roleName}")]
        [Authorize(Dashboard.Role.Delete)]
        public async Task<IActionResult> Delete(string roleName)
        {
            try
            {
                await _roleManager.DeleteAsync(roleName);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Failed");
            }
        }
    }
}
