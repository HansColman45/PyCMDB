using CMDB.API.Interfaces;
using CMDB.API.Models;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Controller for IdentityType
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityTypeController : ControllerBase
    {
        private IdentityTypeController()
        {
        }
        private readonly IUnitOfWork _uow;
        private readonly string site = "Identity Type";
        private HasAdminAccessRequest request;
        /// <summary>
        /// Constructor for IdentityTypeController
        /// </summary>
        /// <param name="uow"></param>
        public IdentityTypeController(IUnitOfWork uow)
        {
                _uow = uow;
        }
        /// <summary>
        /// This will list all IdentityTypes
        /// </summary>
        /// <returns>list of <see cref="TypeDTO"/></returns>
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
            return Ok(await _uow.IdentityTypeRepository.GetAll());
        }
        /// <summary>
        /// This will return a list of all IdentityTypes matching the search string
        /// </summary>
        /// <param name="searchstr"></param>
        /// <returns>list of <see cref="TypeDTO"/></returns>
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
            return Ok(await _uow.IdentityTypeRepository.GetAll(searchstr));
        }
        /// <summary>
        /// This will return a IdentityType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="TypeDTO"/></returns>
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
            return Ok(await _uow.IdentityTypeRepository.GetById(id));
        }
        /// <summary>
        /// This will create a new IdentityType
        /// </summary>
        /// <param name="type"><see cref="TypeDTO"/></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(TypeDTO type)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "Create"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            try
            {
                var acc = _uow.IdentityTypeRepository.Create(type);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This will deactivate a IdentityType
        /// </summary>
        /// <param name="type"><see cref="TypeDTO"/></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [HttpDelete("{reason}"), Authorize]
        public async Task<IActionResult> Delete(TypeDTO type, string reason)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "Deactivate"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            try
            {
                var acc = await _uow.IdentityTypeRepository.DeActivate(type, reason);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This will activate a IdentityType
        /// </summary>
        /// <param name="type"><see cref="TypeDTO"/></param>
        /// <returns></returns>
        [HttpPost("Activate"), Authorize]
        public async Task<IActionResult> Activate(TypeDTO type)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "Activate"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            try
            {
                var acc = await _uow.IdentityTypeRepository.Activate(type);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This will update a IdentityType
        /// </summary>
        /// <param name="type"><see cref="TypeDTO"/></param>
        /// <returns></returns>
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(TypeDTO type)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "Edit"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            try
            {
                var acc = await _uow.IdentityTypeRepository.Update(type);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This will check if a IdentityType already exists
        /// </summary>
        /// <param name="type"><see cref="TypeDTO"/></param>
        /// <returns></returns>
        [HttpPost("IsExisting"), Authorize]
        public async Task<IActionResult> IsTypeExisting(TypeDTO type)
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
            return Ok(await _uow.IdentityTypeRepository.IsExisitng(type));
        }
    }
}