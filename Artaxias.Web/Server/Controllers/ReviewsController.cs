using Artaxias.BusinessLogic.Feedbacks;
using Artaxias.Core.Constants.Permissions.Feedback;
using Artaxias.Web.Common.DataTransferObjects.Feadback;

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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewManager _reviewManager;

        public ReviewsController(IReviewManager reviewManager)
        {
            _reviewManager = reviewManager;
        }

        [HttpGet]
        [Authorize(Feedback.Review.Read)]
        public async Task<ActionResult<List<ReviewResponse>>> Get(int pageSize = 10, int currentPage = 0)
        {
            try
            {
                List<ReviewResponse> result = await _reviewManager.GetListAsync(pageSize, currentPage);
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
        [Authorize(Feedback.Review.Read)]
        public async Task<ActionResult<ReviewResponse>> Get(int id)
        {
            try
            {
                ReviewResponse result = await _reviewManager.GetAsync(id);
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

        [HttpGet("about/{employeeId}")]
        [Authorize(Feedback.Review.Read)]
        public async Task<ActionResult<List<ReviewAboutEmployeeResponse>>> GetReviewsAboutEmployee(int employeeId)
        {
            try
            {
                List<ReviewAboutEmployeeResponse> result = await _reviewManager.GetReviewsAboutEmployee(employeeId);
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
        [Authorize(Feedback.Review.Create)]
        public async Task<IActionResult> Post([FromBody] ReviewRequest request)
        {
            try
            {
                ReviewResponse result = await _reviewManager.CreateAsync(request, User.Identity.Name);
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
        [Authorize(Feedback.Review.Update)]
        public async Task<IActionResult> Put(int id, [FromBody] ReviewRequest request)
        {
            try
            {
                ReviewResponse result = await _reviewManager.UpdateAsync(id, request);
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
        [Authorize(Feedback.Review.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _reviewManager.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (AccessViolationException ex)
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
