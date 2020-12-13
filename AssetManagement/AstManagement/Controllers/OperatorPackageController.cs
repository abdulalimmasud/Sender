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
    public class OperatorPackageController : BaseController
    {
        private readonly IMobileOperatorPackagesService _service;
        public OperatorPackageController(IMobileOperatorPackagesService service)
        {
            _service = service;
        }
        // GET: api/OperatorPackage
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetAsync();
                if (response == null)
                    return StatusCode(StatusCodes.Status204NoContent, "No Package Found.");
                return Ok(response);
            });
        }
        // GET: api/OperatorPackage/5
        [HttpGet("{operatorId}", Name = "GetOperatorPackages")]
        public async Task<IActionResult> Get(int operatorId)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetByOperatorAsync(operatorId);
                if (response == null)
                    return StatusCode(StatusCodes.Status204NoContent, "No Package Found.");
                return Ok(response);
            });
        }
        // GET: api/OperatorPackage/5/test
        [HttpGet("{operatorId}/{packageName}")]
        public async Task<IActionResult> Get(int operatorId, string packageName)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.GetByOperatorAsync(operatorId, packageName);
                if (response == null)
                    return StatusCode(StatusCodes.Status204NoContent, "No Package Found.");
                return Ok(response);
            });
        }
        // POST: api/OperatorPackage
        [HttpPost, Authorize]
        public async Task<IActionResult> Post(OperatorPackageInsertionDto model)
        {
            return await CheckAndExecuteModel(async () =>
            {
                var response = await _service.InsertAsync(model);
                if (response == null)
                    return StatusCode(StatusCodes.Status409Conflict, "Package Name already exists.");
                return Ok(response);
            });            
        }

        // PUT: api/OperatorPackage
        [HttpPut, Authorize]
        public async Task<IActionResult> Put(MobileOperatorPackage model)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.UpdateAsync(model);
                if (response == null)
                    return StatusCode(StatusCodes.Status409Conflict, "Package Name already exists.");
                return Ok(response);
            });
        }
        // Delete:api/OperatorPackage
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return await ExecuteRequest(async () =>
            {
                var response = await _service.DeleteAsync(id);
                if (response == 0)
                    return StatusCode(StatusCodes.Status226IMUsed, "Package use by simcard.");
                return Ok(response);
            });
        }
    }
}
