using Artaxias.BusinessLogic.Organization.Absences;
using Artaxias.Core.Constants.Permissions.Organization;
using Artaxias.Core.Exceptions;
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
    public class AbsencesController : ControllerBase
    {
        private readonly IAbsencesManager _absencesManager;

        public AbsencesController(IAbsencesManager absencesManager)
        {
            _absencesManager = absencesManager;
        }

        [HttpGet]
        [Authorize(Organization.Absence.Read)]
        public async Task<ActionResult<List<AbsenceResponse>>> GetAllAsync([FromQuery] int employeeId, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
        {
            try
            {
                List<AbsenceResponse> result = await _absencesManager.GetListAsync(employeeId, pageSize, pageNumber);
                if (result == null)
                {
                    return NotFound("Retrival Failed");
                }

                return result;
            }
            catch (Exception)
            {
                return BadRequest("Request Failed");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Organization.Absence.Read)]
        public async Task<ActionResult<AbsenceResponse>> GetAsync(int id)
        {
            try
            {
                AbsenceResponse result = await _absencesManager.GetAsync(id);
                if (result == null)
                {
                    return NotFound("Retrival Failed");
                }

                return result;
            }
            catch (Exception)
            {
                return BadRequest("Request Failed");
            }
        }

        [HttpPost]
        [Authorize(Organization.Absence.Create)]
        public async Task<IActionResult> CreateAsync([FromBody] AbsenceRequest request)
        {
            try
            {
                AbsenceResponse response = await _absencesManager.CreateAsync(request);
                return Created(nameof(GetAsync), response.Id);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Description);
            }
            catch (Exception)
            {
                return BadRequest("Request Failed");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Organization.Absence.Update)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] AbsenceRequest request)
        {
            try
            {
                AbsenceResponse result = await _absencesManager.UpdateAsync(id, request);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Request Failed");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Organization.Absence.Delete)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _absencesManager.DeleteAsync(id);
                return Ok("Deleted");
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
                return BadRequest("Request Failed");
            }
        }

        [HttpGet("types")]
        [Authorize(Organization.Absence.Read)]
        public async Task<IActionResult> GetAllTypesAsync([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
        {
            try
            {
                return Ok(await _absencesManager.GetAllTypesAsync(pageSize, pageNumber));
            }
            catch (Exception)
            {
                return BadRequest("Request Failed");
            }
        }

        [HttpPut("{id}/approve/{manageId}")]
        [Authorize(Organization.Absence.Approve)]
        public async Task<IActionResult> ApproveAsync(int id, int manageId)
        {
            try
            {
                await _absencesManager.ApproveAsync(id, manageId);
                return Ok("Approved");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Description);
            }
            catch (Exception)
            {
                return BadRequest("Request Failed");
            }
        }

        [HttpPut("{id}/reject/{manageId}")]
        [Authorize(Organization.Absence.Reject)]
        public async Task<IActionResult> RejectAsync(int id, int manageId)
        {
            try
            {
                await _absencesManager.RejectAsync(id, manageId);
                return Ok("Rejected");
            }
            catch (Exception)
            {
                return BadRequest("Request Failed");
            }
        }
    }
}
