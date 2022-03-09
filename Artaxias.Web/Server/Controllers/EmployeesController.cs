using Artaxias.BusinessLogic.Organization.Employees;
using Artaxias.Core.Constants.Permissions.Dashboard;
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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeManager _employeeManager;

        public EmployeesController(IEmployeeManager employeeManager)
        {
            _employeeManager = employeeManager;
        }

        [HttpGet]
        [Authorize(Dashboard.Employee.Read)]
        public async Task<ActionResult<List<EmployeeResponse>>> Get([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
        {
            try
            {
                List<EmployeeResponse> result = await _employeeManager.GetListAsync(pageSize, pageNumber);
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


        [HttpGet("{id}")]
        [Authorize(Dashboard.Employee.Read)]

        public async Task<ActionResult<EmployeeResponse>> Get(int id)
        {
            try
            {
                EmployeeResponse result = await _employeeManager.GetAsync(id);
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
        [Authorize(Dashboard.Employee.Create)]
        public async Task<IActionResult> Post([FromBody] EmployeeRequest request)
        {
            try
            {
                EmployeeResponse result = await _employeeManager.CreateAsync(request);
                return Created(nameof(Get), result.Id);
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

        [HttpPut("{id}")]
        [Authorize(Dashboard.Employee.Update)]
        public async Task<IActionResult> Put(int id, [FromBody] EmployeeRequest request)
        {
            if (id != request.Id)
            {
                return new StatusCodeResult(409);
            }

            try
            {
                EmployeeResponse result = await _employeeManager.UpdateAsync(id, request);
                return Ok(result);
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

        [HttpPut("{id}/endContract")]
        [Authorize(Dashboard.Employee.Delete)]
        public async Task<IActionResult> EndContract(int id, [FromBody] EndContractRequest request)
        {
            if (id != request.EmployeeId)
            {
                return new StatusCodeResult(409);
            }

            try
            {
                EmployeeResponse result = await _employeeManager.EndContract(request);
                return Ok(result);
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

        [HttpDelete("{id}")]
        [Authorize(Dashboard.Employee.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _employeeManager.DeleteAsync(id);
                return NoContent();
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
