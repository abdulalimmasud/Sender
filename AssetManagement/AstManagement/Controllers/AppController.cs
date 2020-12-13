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
    public class AppController : BaseController
    {
        private readonly IAppService _service;
        public AppController(IAppService service)
        {
            _service = service;
        }
        // GET: api/App
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetAsync();
                if (response == null)
                    return StatusCode(StatusCodes.Status204NoContent, "No App Found.");
                return Ok(response);
            });
        }

        // GET: api/App/5
        [HttpGet("{id}", Name = "GetApp")]
        public async Task<IActionResult> Get(int id)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetAsync(id);
                if (response == null)
                    return StatusCode(StatusCodes.Status204NoContent, "No App Found.");
                return Ok(response);
            });
        }

        // POST: api/App
        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromBody] Entity dto)
        {
            return await CheckAndExecuteModel(async () =>
            {
                var response = await _service.InsertAsync(dto);
                if (response == null)
                    return StatusCode(StatusCodes.Status409Conflict, "App already exists.");
                return Ok(response);
            });
        }

        // PUT: api/App/5
        [HttpPut, Authorize]
        public async Task<IActionResult> Put([FromBody] App model)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.UpdateAsync(model);
                if (response == null)
                    return StatusCode(StatusCodes.Status409Conflict, "App already exists.");
                return Ok(response);
            });
        }
    }
}
