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
    public class SmsController : ControllerBase
    {
        private readonly ISmsService _service;
        public SmsController(ISmsService service)
        {
            _service = service;
        }
        // POST: api/Sms
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SmsDto dto)
        {
            var response = await _service.Send(dto);
            return StatusCode(response);
        }
    }
}
