using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AstManagement.Services;
using AstManagement.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AstManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        // POST: api/user
        [HttpPost]
        public async Task<IActionResult> Post(UserRegistrationDto model)
        {
            return await CheckAndExecuteModel(async () =>
            {
                var response = await _service.Register(model);
                if (response == null)
                    return StatusCode(StatusCodes.Status409Conflict, "Username already exists.");
                return Ok(response);
            });
        }
    }
}