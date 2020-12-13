using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AstManagement.AssetDB;
using AstManagement.Services;
using AstManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AstManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorController : BaseController
    {
        private readonly IMobileOperatorService _service;
        public OperatorController(IMobileOperatorService service)
        {
            _service = service;
        }
        // GET: api/Operator
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetAsync();
                if (response == null)
                    return NoContent();
                return Ok(response);
            });
        }
        // GET: api/Operator/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetAsync(id);
                if (response == null)
                    return NoContent();
                return Ok(response);
            });
        }

        // POST: api/Operator
        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromBody] Entity model)
        {
            return await CheckAndExecuteModel(async () =>
            {
                var response = await _service.InsertAsync(model);
                if (response == null)
                    return StatusCode(StatusCodes.Status409Conflict, "Operator already exists.");
                return Ok(response);
            });
        }
        // PUT: api/Operator
        [HttpPut, Authorize]
        public async Task<IActionResult> Put([FromBody] MobileOperator model)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.UpdateAsync(model);
                if (response == null)
                    return StatusCode(StatusCodes.Status409Conflict, "Operator already exists.");
                return Ok(response);
            });
        }
        // DELETE: api/Operator
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.DeleteAsync(id);
                if (response == 0)
                    return StatusCode(StatusCodes.Status409Conflict, "Operator used by packaged.");
                return Ok(response);
            });
        }
    }
}
