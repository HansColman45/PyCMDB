using CMDB.API.Interfaces;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Controller for managing configurations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private ConfigurationController()
        {
        }
        private readonly IUnitOfWork _uow;
        /// <summary>
        /// Constructor for the ConfigurationController
        /// </summary>
        /// <param name="uow"></param>
        public ConfigurationController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        /// <summary>
        /// This will return the configuration
        /// </summary>
        /// <param name="request"><see cref="ConfigurationRequest"/></param>
        /// <returns><see cref="Domain.Entities.Configuration"/></returns>
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
