using Artaxias.BusinessLogic.Feedbacks;
using Artaxias.Core.Exceptions;
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
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackManager _feedbackManager;

        public FeedbacksController(IFeedbackManager feedbackManager)
        {
            _feedbackManager = feedbackManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<FeedbackDetails>>> Get([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
        {
            try
            {
                List<FeedbackDetails> result = await _feedbackManager.GetListAsync(pageSize, pageNumber);
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

        [HttpGet("{reviewerUserName}/{reviewerRevieweeId}")]
        public async Task<ActionResult<ProvideFeedbackResponse>> RequestFeedback(string reviewerUserName, int reviewerRevieweeId)
        {
            try
            {
                if (reviewerUserName != User.Identity.Name)
                {
                    return Forbid();
                }

                ProvideFeedbackResponse result = await _feedbackManager.GetFeedbackResponseAsync(reviewerRevieweeId);
                if (result == null)
                {
                    return NotFound("Retrival Failed");
                }

                return result;
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Description);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<List<FeedbackAboutEmployeeResponse>>> GetFeedback(int employeeId, [FromQuery] int reviewId)
        {
            try
            {
                List<FeedbackAboutEmployeeResponse> result = await _feedbackManager.GetFeedbackAboutEmployee(employeeId, reviewId);
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
        public async Task<IActionResult> Post([FromBody] ProvideFeedbackRequest request)
        {
            try
            {
                ProvideFeedbackResponse result = await _feedbackManager.CreateFeedbackAsync(request);
                return Created(nameof(Get), request.ReviewerRevieweeId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Description);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
