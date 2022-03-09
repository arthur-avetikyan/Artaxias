using Artaxias.BusinessLogic.Organization.Departments;
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
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentManager _departmentManager;

        public DepartmentsController(IDepartmentManager departmentManager)
        {
            _departmentManager = departmentManager;
        }

        /// <summary>
        /// Get all departments
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Dashboard.Department.Read)]
        public async Task<ActionResult<List<DepartmentResponse>>> Get([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
        {
            try
            {
                List<DepartmentResponse> result = await _departmentManager.GetListAsync(pageSize, pageNumber);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Dashboard.Department.Read)]
        public async Task<ActionResult<DepartmentResponse>> Get(int id)
        {
            try
            {
                DepartmentResponse result = await _departmentManager.GetAsync(id);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [Authorize(Dashboard.Department.Create)]
        public async Task<IActionResult> Post([FromBody] DepartmentRequest department)
        {
            try
            {
                DepartmentResponse result = await _departmentManager.CreateAsync(department);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        [Authorize(Dashboard.Department.Update)]
        public async Task<IActionResult> Put(int id, [FromBody] DepartmentRequest request)
        {
            if (id != request.Id)
            {
                return new StatusCodeResult(409);
            }

            try
            {
                DepartmentResponse result = await _departmentManager.UpdateAsync(id, request);
                return Ok(result);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [Authorize(Dashboard.Department.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _departmentManager.DeleteAsync(id);
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
