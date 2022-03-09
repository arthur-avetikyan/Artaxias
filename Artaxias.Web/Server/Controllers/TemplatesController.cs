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
    public class TemplatesController : ControllerBase
    {
        private readonly ITemplateManager _templateManager;

        public TemplatesController(ITemplateManager templateManager)
        {
            _templateManager = templateManager;
        }

        [HttpGet]
        [Authorize(Feedback.Template.Read)]
        public async Task<ActionResult<List<TemplateResponse>>> Get(int pageSize = 10, int currentPage = 0)
        {
            try
            {
                List<TemplateResponse> result = await _templateManager.GetListAsync(pageSize, currentPage);
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
        [Authorize(Feedback.Template.Read)]
        public async Task<ActionResult<TemplateResponse>> Get(int id)
        {
            try
            {
                TemplateResponse result = await _templateManager.GetAsync(id);
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

        [HttpGet("types")]
        [Authorize(Feedback.Template.Read)]
        public async Task<ActionResult<List<AnswerOptionTypeDetails>>> GetAllAnswerOptionTypes()
        {
            try
            {
                List<AnswerOptionTypeDetails> result = await _templateManager.GetAllAnswerOptionTypes();
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
        [Authorize(Feedback.Template.Create)]
        public async Task<IActionResult> Post([FromBody] TemplateRequest request)
        {
            try
            {
                TemplateResponse result = await _templateManager.CreateAsync(request);
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
        [Authorize(Feedback.Template.Update)]
        public async Task<IActionResult> Put(int id, [FromBody] TemplateRequest request)
        {
            if (id != request.Id)
            {
                return new StatusCodeResult(409);
            }

            try
            {
                TemplateResponse result = await _templateManager.UpdateAsync(id, request);
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

        [HttpDelete("{id}")]
        [Authorize(Feedback.Template.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _templateManager.DeleteAsync(id);
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
