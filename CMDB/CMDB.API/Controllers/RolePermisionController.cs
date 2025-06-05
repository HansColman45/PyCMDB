using CMDB.API.Interfaces;
using CMDB.API.Models;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// This is the controller fom the RolePermission API.
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
        /// <summary>
        /// Retrieves all role permissions that match the specified search string.
        /// </summary>
        /// <remarks>This method requires the caller to be authorized and have administrative access. If
        /// the caller does not  have the necessary permissions, an unauthorized response is returned.</remarks>
        /// <param name="searchstr">The search string used to filter role permissions. If empty or null, all role permissions are returned.</param>
        /// <returns>An <see cref="IActionResult"/> containing the filtered list of role permissions if the user is authorized; 
        /// otherwise, an unauthorized response.</returns>
        [HttpGet("GetAll/{searchstr}"), Authorize]
        public async Task<IActionResult> GetAll(string searchstr)
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
            return Ok(await _uow.RolePermissionRepository.GetAll(searchstr));
        }
        /// <summary>
        /// Retrieves a role permission by its unique identifier.
        /// </summary>
        /// <remarks>This method requires the caller to be authorized and have administrative access. If
        /// the caller does not have the necessary permissions, the method will return <see langword="401
        /// Unauthorized"/>.</remarks>
        /// <param name="id">The unique identifier of the role permission to retrieve. Must be a positive integer.</param>
        /// <returns>An <see cref="IActionResult"/> containing the role permission data if found, or an appropriate HTTP status
        /// code: <list type="bullet"> <item><description><see langword="200 OK"/> if the role permission is
        /// successfully retrieved.</description></item> <item><description><see langword="401 Unauthorized"/> if the
        /// user is not authorized to access the resource.</description></item> </list></returns>
        [HttpGet("{id:int}"), Authorize]
        public async Task<IActionResult> GetById(int id)
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
            return Ok(await _uow.RolePermissionRepository.GetById(id));
        }
        /// <summary>
        /// Creates a new role permission and persists it to the database.
        /// </summary>
        /// <remarks>This method requires the caller to be authorized and have administrative access.</remarks>
        /// <param name="rolePermission">The data transfer object containing the details of the role permission to be created.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns: <list type="bullet">
        /// <item><see cref="UnauthorizedResult"/> if the user is not authorized to perform the action.</item>
        /// <item><see cref="BadRequestObjectResult"/> if the creation of the role permission fails.</item> <item><see
        /// cref="CreatedAtActionResult"/> if the role permission is successfully created, including the URI of the
        /// created resource and its details.</item> </list></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(RolePermissionDTO rolePermission)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "Add"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var result = _uow.RolePermissionRepository.Create(rolePermission);
            await _uow.SaveChangesAsync();
            if (result == null)
                return BadRequest("Failed to create role permission.");
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        /// <summary>
        /// Updates the role permissions for a specified role.
        /// </summary>
        /// <remarks>This method requires the caller to be authorized and have administrative access.</remarks>
        /// <param name="rolePermission">An object containing the role and its associated permissions to be updated. The object must not be null and
        /// should contain valid role and permission data.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see langword="Ok"/> with the
        /// updated role permissions if the update is successful.
        /// Returns <see langword="Unauthorized"/> if the user does not have the necessary permissions.</returns>
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(RolePermissionDTO rolePermission)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "Update"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            _uow.RolePermissionRepository.Update(rolePermission);
            await _uow.SaveChangesAsync();
            return Ok();
        }
    }
}
