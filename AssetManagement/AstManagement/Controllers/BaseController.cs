using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AstManagement.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
        }
        protected static string uid = "uid";
        protected int UserId
        {
            get
            {
                var claims = GetClaims();
                string userId = claims.First(c => c.Type == uid).Value;
                return Convert.ToInt32(userId);
            }
        }

        private List<Claim> GetClaims()
        {
            return Request.HttpContext.User.Claims.ToList();
        }
        protected async Task<IActionResult> CheckAndExecuteModel(Func<Task<IActionResult>> method)
        {
            return await ExecuteRequest(async () =>
            {
                if (ModelState.IsValid)
                {
                    return await method();
                }
                return BadRequest();
            });
        }
        protected async Task<IActionResult> ExecuteRequest(Func<Task<IActionResult>> methods)
        {
            try
            {
                return await methods();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest("Your request is not accepted. Please try again with valid data.");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Your request is not accepted. Please try again with valid data.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Your request is not accepted. Please try again with valid data.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest("Your request is not accepted. Please try again with valid data.");
            }
            catch (AccessViolationException ex)
            {
                return BadRequest("Your request is not accepted. Please try again with valid data.");
            }
            catch (NullReferenceException ex)
            {
                return BadRequest("Your request is not accepted. Please try again with valid data.");
            }
            catch (IndexOutOfRangeException ex)
            {
                return BadRequest("Your request is not accepted. Please try again with valid data.");
            }
            catch (SystemException ex)
            {
                return BadRequest("Your request is not accepted. Please try again with valid data.");
            }
            catch (Exception ex)
            {
                return BadRequest("Your request is not accepted. Please try again with valid data.");
            }
        }
        public string SerializeObject(dynamic data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}