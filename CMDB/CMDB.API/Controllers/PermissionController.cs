﻿using CMDB.API.Interfaces;
using CMDB.Domain.DTOs;
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
    public class PermissionController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly string site = "Permissions";
        private readonly ILogger<PermissionController> _logger;
        private HasAdminAccessRequest request;

        private PermissionController()
        {
        }
        /// <summary>
        /// Constructor for the permission controller.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        public PermissionController(IUnitOfWork unitOfWork, ILogger<PermissionController> logger)
        {
            _uow = unitOfWork;
            _logger = logger;
            _logger.LogInformation("PermissionController initialized");
        }
        /// <summary>
        /// This will return a list of all permissions.
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
                Permission = Permission.Read
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.PermissionRepository.GetAll());
        }
        /// <summary>
        /// Retrieves all permissions that match the specified search string.
        /// </summary>
        /// <remarks>This endpoint requires the caller to be authenticated and authorized as an
        /// administrator. If the user does not have administrative access, the response will be <see
        /// langword="Unauthorized"/>.</remarks>
        /// <param name="searchstr">The search string used to filter permissions. This parameter is case-insensitive and may be empty or null to
        /// retrieve all permissions.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of permissions that match the search criteria.
        /// Returns <see langword="Unauthorized"/> if the user is not authenticated or does not have administrative
        /// access.</returns>
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
                Permission = Permission.Read
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.PermissionRepository.GetAll(searchstr));
        }
        /// <summary>
        /// Retrieves a permission by its unique identifier.
        /// </summary>
        /// <remarks>This method requires the caller to be authenticated and authorized. The user's
        /// administrative access is verified before retrieving the requested permission.</remarks>
        /// <param name="id">The unique identifier of the permission to retrieve. Must be a positive integer.</param>
        /// <returns>An <see cref="IActionResult"/> containing the permission data if the operation is successful. Returns <see
        /// langword="Unauthorized"/> if the user is not authorized to access the resource.</returns>
        [HttpGet("{id:int}"), Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Using GetID in PermissionController using Id: {0}", id);
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.Read
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.PermissionRepository.GetById(id));
        }
        /// <summary>
        /// This method creates a new permission in the system.
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(PermissionDTO permission)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.Add
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            _uow.PermissionRepository.Create(permission);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Updates the specified permission in the system.
        /// </summary>
        /// <remarks>This method requires the user to be authorized and have administrative access.</remarks>
        /// <param name="permission">The permission data to update, represented as a <see cref="PermissionDTO"/> object.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. 
        /// Returns <see cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkResult"/> if the update is
        /// successful.</returns>
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(PermissionDTO permission)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.Update
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            _uow.PermissionRepository.Update(permission);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// This method will return a list of RolePermissions for a specific Permission.
        /// </summary>
        /// <param name="id">The Id of the permission</param>
        /// <returns></returns>
        [HttpGet("RorePermOverview/{id:int}"), Authorize]
        public async Task<IActionResult> RolePermOverview(int id)
        {
            _logger.LogInformation("Using RolePermOverview in PermissionController using Id: {0}", id);
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.RorePermOverview
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.PermissionRepository.GetRolePermissionInfo(id));
        }
        /// <summary>
        /// Deletes a resource identified by the specified ID.
        /// </summary>
        /// <remarks>This action requires the user to be authorized and have the appropriate
        /// administrative permissions.</remarks>
        /// <param name="permission">The unique identifier of the resource to delete. Must be a positive integer.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkResult"/> if the deletion is
        /// successful.</returns>
        [HttpDelete, Authorize]
        public async Task<IActionResult> Delete(PermissionDTO permission)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.Delete
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            _uow.PermissionRepository.Delete(permission);
            await _uow.SaveChangesAsync();
            return Ok();
        }
    }
}
