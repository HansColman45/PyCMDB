using CMDB.API.Services;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// This is the controller fom the permission API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ControllerBase
    {
        private RolePermissionController()
        {
        }
        private readonly IUnitOfWork _uow;
        private readonly string site = "Permission";
        private readonly ILogger<RolePermissionController> _logger;
        private HasAdminAccessRequest request;
        /// <summary>
        /// The constructor for the permission controller.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="logger"></param>
        public RolePermissionController(IUnitOfWork uow, ILogger<RolePermissionController> logger)
        {
            _logger = logger;
            _uow = uow;
        }
        /// <summary>
        /// This will return all the Permissions in the system.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpGet("GetAll"), Authorize]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation($"Using the GetAll {nameof(AccountController)}");
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "Read"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.RolePermissionRepository.GetAll());
        }
    }
}
