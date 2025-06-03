using CMDB.API.Services;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Permission controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionControllor: ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly string site = "Account";
        private readonly ILogger<PermissionControllor> _logger;
        private HasAdminAccessRequest request;

        private PermissionControllor()
        {
        }
        /// <summary>
        /// Constructor for the permission controller.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        public PermissionControllor(IUnitOfWork unitOfWork, ILogger<PermissionControllor> logger)
        {
            _uow = unitOfWork;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll"), Authorize]
        public async Task<IActionResult> GetAll()
        {
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
            return Ok();
        }
    }
}
