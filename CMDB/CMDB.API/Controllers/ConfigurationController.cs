using CMDB.API.Services;
using CMDB.Domain.Requests;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public ConfigurationController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> GetConfiguration(ConfigurationRequest request)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            return Ok(await _uow.ConfigurationRepository.GetConfiguration(request));
        }
    }
}
