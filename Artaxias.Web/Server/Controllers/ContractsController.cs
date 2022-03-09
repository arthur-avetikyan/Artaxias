using Artaxias.BusinessLogic.Organization.Contracts;
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
    public class ContractsController : ControllerBase
    {
        private readonly IContractManager _contractManager;

        public ContractsController(IContractManager contractManager)
        {
            _contractManager = contractManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ContractTemplateResponse>>> Get([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
        {
            try
            {
                List<ContractTemplateResponse> result = await _contractManager.GetListAsync(pageSize, pageNumber);
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
        public async Task<ActionResult<ContractTemplateResponse>> Get(int id)
        {
            try
            {
                ContractTemplateResponse result = await _contractManager.GetAsync(id);
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
        public async Task<IActionResult> Post([FromBody] ContractTemplateRequest request)
        {
            try
            {
                ContractMappings result = await _contractManager.SaveAsync(request);
                return Created(nameof(Get), result.ContractTemplateId);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _contractManager.DeleteAsync(id);
                return Ok();
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

        [HttpGet("properties")]
        public ActionResult<IEnumerable<string>> GetAvailablePropertiesAsync()
        {
            try
            {
                IEnumerable<string> result = _contractManager.GetAvailablePropertiesAsync();
                if (result == null)
                {
                    return NotFound("Retrival Failed");
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Retrival Failed");
            }
        }

        [HttpGet("properties/{contractTemplateId}")]
        public async Task<ActionResult<ContractMappings>> GetContractMappingsAsync(int contractTemplateId)
        {
            try
            {
                ContractMappings result = await _contractManager.GetContractMappingsAsync(contractTemplateId);
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

        [HttpPut("mappings")]
        public async Task<IActionResult> SaveMappings(ContractMappings request)
        {
            try
            {
                string result = await _contractManager.SaveMappingsAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Failure");
            }
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate(ContractGenerationRequest request)
        {
            try
            {
                Common.DataTransferObjects.File.FileDto result = await _contractManager.GenerateAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Failure");
            }
        }
    }
}
