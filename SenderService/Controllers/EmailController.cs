using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SenderService.Services;
using SenderService.ViewModel;

namespace SenderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _service;
        public EmailController(IEmailService service)
        {
            _service = service;
        }
        // POST: api/Email
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmailDto dto)
        {
            var response = await _service.Send(dto);
            return StatusCode(response);
        }
        // POST: api/Email/bulk
        [HttpPost("bulk")]
        public async Task<IActionResult> Post([FromBody] BulkEmailDto dto)
        {
            await _service.Send(dto);
            return Ok();
        }
    }
}
