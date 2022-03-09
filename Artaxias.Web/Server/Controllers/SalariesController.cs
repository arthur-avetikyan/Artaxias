using Artaxias.BusinessLogic.Organization.Salaries;
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
    public class SalariesController : ControllerBase
    {
        private readonly ISalaryManager _salaryManager;

        public SalariesController(ISalaryManager salaryManager)
        {
            _salaryManager = salaryManager;
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<List<SalaryResponse>>> Get(int employeeId, [FromQuery] int pageSize = 10, int currentPage = 0)
        {
            try
            {
                List<SalaryResponse> result = await _salaryManager.GetListAsync(employeeId, pageSize, currentPage);
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
        public async Task<IActionResult> Post([FromBody] SalaryRequest request)
        {
            try
            {
                SalaryResponse result = await _salaryManager.CreateAsync(request);
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
    }
}
