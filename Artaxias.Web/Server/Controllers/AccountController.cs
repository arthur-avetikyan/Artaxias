
using Artaxias.BusinessLogic.Account;
using Artaxias.Core.Exceptions;
using Artaxias.Web.Common.DataTransferObjects.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace Artaxias.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager _accountManager;

        public AccountController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterParameters parameters)
        {
            try
            {
                UserDetails result = await _accountManager.Register(parameters);
                return Created(nameof(UsersController.Get), result.Id);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Description);
            }
            catch (Exception)
            {
                return BadRequest("Failed");
            }
        }

        [HttpPost("confirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> Confirm(ConfirmEmailRequest confirmEmailRequest)
        {
            try
            {
                await _accountManager.ConfirmEmail(confirmEmailRequest);
                return Ok("Account has been Approved and Activated");
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            catch (DomainException e)
            {
                return BadRequest(e.Description);
            }
            catch (Exception)
            {
                return BadRequest("Failed");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest parameters)
        {
            try
            {
                AuthenticationResult result = await _accountManager.Login(parameters);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
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

        [HttpPost("refreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(AuthenticationResult authenticationRequest)
        {
            try
            {
                AuthenticationResult result = await _accountManager.Refresh(authenticationRequest);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
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

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _accountManager.Revoke(User.Identity.Name);
                return Ok();
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
