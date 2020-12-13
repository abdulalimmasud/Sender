using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AstManagement.Services;
using AstManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AstManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : BaseController
    {
        private readonly IAttachmentService _service;
        public AttachmentController(IAttachmentService service)
        {
            _service = service;
        }
        // GET: api/Attachment/{mobileNumber}
        [HttpGet("{mobileNumber}", Name = "GetSimIfAttachmentOrNot")]
        public async Task<IActionResult> Get(string mobileNumber)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetAsync(mobileNumber);
                if(response == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                return Ok(response);
            });
        }
        // GET: api/Attachment/{assetId}/{appId}
        [HttpGet("{assetId}/{appId}")]
        public async Task<IActionResult> Get(int assetId, int appId)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetAsync(assetId, appId);
                return Ok(response);
            });
        }
        // GET: api/Attachment
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10, string search = "")
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetAsync(page, pageSize, search);
                if (response == null)
                    return StatusCode(StatusCodes.Status204NoContent, "No Attachment Found.");
                return Ok(response);
            });
        }

        // POST: api/Attachment
        [HttpPost, Authorize]
        public async Task<IActionResult> Post(AttachmentInsertionDto model)
        {
            return await CheckAndExecuteModel(async () =>
            {
                var response = await _service.InsertAsync(model);
                return StatusCode(response.StatusCode, response.Message);
            });
        }

        // PUT: api/Attachment/5
        [HttpPut, Authorize]
        public async Task<IActionResult> Put(AttachmentInsertionDto model)
        {
            return await CheckAndExecuteModel(async () =>
            {
                var response = await _service.InsertOrUpdateAsync(model);
                return Ok(response);
            });
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{simCardId}"), Authorize]
        public async Task<IActionResult> Delete(int simCardId, int userId, string username)
        {
            return await ExecuteRequest(async () =>
            {
                if (string.IsNullOrEmpty(username))
                {
                    return BadRequest();
                }
                var response = await _service.DeleteAsync(simCardId, userId, username);
                if (response < 1)
                    return StatusCode(StatusCodes.Status403Forbidden, "Simcard already detuched or something occured.");
                return Ok(response);
            });
        }
    }
}
