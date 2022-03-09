
using Artaxias.BusinessLogic.Users;
using Artaxias.Core.Constants.Permissions.Dashboard;
using Artaxias.Web.Common.DataTransferObjects.Account;
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
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Get a user based on UserName
        /// </summary>
        /// <returns></returns>
        [HttpGet("{userName}")]
        [Authorize(Dashboard.User.Read)]
        public async Task<ActionResult<UserDetails>> Get(string userName)
        {
            try
            {
                var result = await _userManager.GetAsync(userName);
                if (result == null)
                    return NotFound("Failed");

                return result;
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

        [HttpGet("{userName}/userInfo")]
        public async Task<ActionResult<UserInfo>> GetUserInfo(string userName)
        {
            try
            {
                var result = await _userManager.GetUserInfoAsync(userName);
                if (result == null)
                    return NotFound("Failed");

                return result;
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

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Dashboard.User.Read)]
        public async Task<ActionResult<List<UserDetails>>> Get([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0, [FromQuery] bool unemployeed = false)
        {
            try
            {
                if (!unemployeed)
                {
                    var result = await _userManager.GetListAsync(pageSize, pageNumber);
                    if (result == null)
                        return NotFound("Failed");

                    return result;
                }
                else
                {
                    var result = await _userManager.GetUnemployeedUsers();
                    if (result == null)
                        return NotFound("Failed");

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="registerParameters"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Dashboard.User.Create)]
        public async Task<IActionResult> Create([FromBody] RegisterParameters registerParameters)
        {
            try
            {
                var result = await _userManager.CreateAsync(registerParameters);
                return Created(nameof(Get), result.Id);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Failed");
            }
        }

        /// <summary>
        /// Update a user role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Dashboard.User.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] UserDetails userDetails)
        {
            if (id != userDetails.Id)
                return new StatusCodeResult(409);

            try
            {
                var result = await _userManager.UpdateAsync(userDetails);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Failed");
            }
        }

        /// <summary>
        /// Change a user password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPut("{userName}/changePassword")]
        [Authorize(Dashboard.User.Update)]
        public async Task<IActionResult> ChangePassword(string userName, [FromBody] ChangePasswordParameters parameters)
        {
            if (userName != parameters.UserName)
                return new StatusCodeResult(409);

            try
            {
                await _userManager.ChangePassword(parameters);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Failed");
            }
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpDelete("{userName}")]
        [Authorize(Dashboard.User.Delete)]
        public async Task<IActionResult> Delete(string userName)
        {
            try
            {
                await _userManager.DeleteAsync(userName);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Failed");
            }
        }
    }
}
