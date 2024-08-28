using CMDB.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
       private IApplicationService applicationService;
        public ApplicationController(IApplicationService service)
        {
            applicationService = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Application")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Read")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            return Ok(await applicationService.GetAll());
        }
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Application")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Read")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            return Ok(await applicationService.GetById(id));
        }
    }
}
