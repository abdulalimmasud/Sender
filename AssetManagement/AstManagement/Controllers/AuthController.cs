using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AstManagement.AssetDB;
using AstManagement.Configuration;
using AstManagement.Services;
using AstManagement.Util;
using AstManagement.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AstManagement.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ApplicationOptions _applicationOptions;
        public AuthController(IUserService service, IOptions<ApplicationOptions> applicationOptionsAccessor)
        {
            _service = service;
            _applicationOptions = applicationOptionsAccessor.Value;
        }
        /// <summary>
        /// To Login app model must be UrlEncoded
        /// </summary>
        [HttpPost("~/token")] 
        public async Task<IActionResult> Login(AuthenticationDto model)
        {
            if (ModelState.IsValid)
            {
                if(model.GrantType == AuthenticationType.Password)
                {
                    var user = await _service.GetAsync(model.Username);
                    if (user == null)
                        return StatusCode(StatusCodes.Status412PreconditionFailed, "Username/Password doesn't match");
                    bool verified = SecurityManagement.VerifyPassword(model.Password, user.PasswordHash, user.PasswordSalt);
                    if(!verified)
                        return StatusCode(StatusCodes.Status412PreconditionFailed, "Username/Password doesn't match");
                    return await GenerateToken(user, model.GrantType);
                }
            }
            return BadRequest();
        }
        private async Task<IActionResult> GenerateToken(User user, AuthenticationType grantType)
        {
            var claims = new List<Claim>
            {
                new Claim("uid", user.Id.ToString()),
                new Claim("name", user.Name)
            };
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_applicationOptions.ApiKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
            var tokenWrite = new JwtSecurityTokenHandler().WriteToken(token);
            var data = new
            {
                token = tokenWrite,
                name = user.Name
            };
            return Ok(data);
        }
    }
}