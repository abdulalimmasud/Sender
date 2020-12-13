using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AstManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AstManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentLogController : BaseController
    {
        private readonly IAttachmentLogService _service;
        public AttachmentLogController(IAttachmentLogService service)
        {
            _service = service;
        }
        // GET: api/AttachmentLog/{simCardId}
        [HttpGet("{simCardId}", Name = "GetAttachmentLogBySimCard")]
        public async Task<IActionResult> Get(int simCardId)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetAsync(simCardId);
                if (response == null)
                    return StatusCode(StatusCodes.Status204NoContent, "No History Found.");
                return Ok(response);
            });
        }
        // GET: api/AttachmentLog/{assetId}/{appId}
        [HttpGet("{assetId}/{appId}", Name = "GetAttachmentLogByAsset")]
        public async Task<IActionResult> Get(int assetId, int appId)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetAsync(assetId, appId);
                if (response == null)
                    return StatusCode(StatusCodes.Status204NoContent, "No History Found.");
                return Ok(response);
            });
        }
    }
}
