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
    public class SimCardController : BaseController
    {
        private readonly ISimCardService _service;
        public SimCardController(ISimCardService service)
        {
            _service = service;
        }
        // GET: api/SimCard?page=1&pageSize=10&type=1&search=01754
        [HttpGet(Name = "GetSimCards")]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10, int type = 1, string search = "")
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetAsync(page, pageSize, type, search);
                if (response == null)
                    return StatusCode(StatusCodes.Status204NoContent, "No Simcard Found.");
                return Ok(response);
            });
        }
        // GET: api/SimCard/inactive?page=1&pageSize=10&search=01754
        [HttpGet("inactive", Name = "GetInactiveSimCards")]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10, string search = "")
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetInactiveSimAsync(page, pageSize, search);
                if (response == null)
                    return StatusCode(StatusCodes.Status204NoContent, "No Simcard Found.");
                return Ok(response);
            });
        }
        // POST: api/SimCard
        [HttpPost, Authorize]
        public async Task<IActionResult> Post(SimCardRegistrationDto model)
        {
            return await CheckAndExecuteModel(async () =>
            {
                var response = await _service.InsertAsync(model);
                if (response == null)
                    return StatusCode(StatusCodes.Status409Conflict, "Sim Card already exists.");
                return Ok(response);
            });            
        }
        // POST: api/SimCard/upload
        [HttpPost("upload"), Authorize]
        public async Task<IActionResult> Post([FromForm]SimCardImportDto model)
        {
            return await CheckAndExecuteModel(async () =>
            {
                if (model.File.Length < 1)
                {
                    throw new NullReferenceException();
                }
                else if(!model.File.FileName.EndsWith(".xlsx"))
                {
                    throw new InvalidCastException("Invalid File Type.");
                }
                var response = await _service.InsertFromFileAsync(model);
                if (response.StatusCode == 200)
                {
                    return Ok("Simcard Insertion Succesful.");
                }
                return StatusCode(response.StatusCode, response.Message);
            });
        }
        // PUT: api/SimCard
        [HttpPut, Authorize]
        public async Task<IActionResult> Put(SimCard model)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.UpdateAsync(model);
                if (response == null)
                    return StatusCode(StatusCodes.Status409Conflict, "Sim Card already exists.");
                return Ok(response);
            });
        }
        // Delete:api/SimCard
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(int id, int userId, string username)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.DeleteAsync(id, userId, username);
                if (response == 0)
                    return StatusCode(StatusCodes.Status226IMUsed, "Simcard uses by a device.");
                return Ok(response);
            });
        }
    }
}
