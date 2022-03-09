using Artaxias.BusinessLogic.Organization.Bonuses;
using Artaxias.Web.Common.DataTransferObjects.Organization;

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
    public class BonusesController : ControllerBase
    {
        private readonly IBonusManager _bonusManager;

        public BonusesController(IBonusManager bonusManager)
        {
            _bonusManager = bonusManager;
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<List<BonusResponse>>> GetEmployeeBonuses(int employeeId, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
        {
            try
            {
                List<BonusResponse> result = await _bonusManager.GetBonusesListForEmployeeAsync(employeeId, pageSize, pageNumber);
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

        [HttpGet("{bonusId}")]
        public async Task<ActionResult<BonusResponse>> Get(int bonusId)
        {
            try
            {
                BonusResponse result = await _bonusManager.GetAsync(bonusId);
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BonusRequest request)
        {
            try
            {
                BonusResponse result = await _bonusManager.CreateAsync(request);
                return Created(nameof(Get), result.Id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Request Failed");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BonusRequest request)
        {
            if (id != request.Id)
            {
                return new StatusCodeResult(409);
            }

            try
            {
                BonusResponse result = await _bonusManager.UpdateAsync(id, request);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Request Failed");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bonusManager.DeleteAsync(id);
                return Ok("Deleted");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Request Failed");
            }
        }
    }
}
