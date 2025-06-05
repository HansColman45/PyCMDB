using CMDB.API.Interfaces;
using CMDB.API.Models;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Controller for managing account types
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTypeController : ControllerBase
    {
        private AccountTypeController()
        {
        }
        private readonly IUnitOfWork _uow;
        private readonly string site = "Account Type";
        private HasAdminAccessRequest request;
        /// <summary>
        /// Constructor for the AccountTypeController
        /// </summary>
        /// <param name="uow"></param>
        public AccountTypeController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        /// <summary>
        /// This will return all the account types
        /// </summary>
        /// <returns>200 with a List of <see cref="TypeDTO"/></returns>
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
            return Ok(await _uow.AccountTypeRepository.GetAll());
        }
        /// <summary>
        /// This will return all the account types matching the search string
        /// </summary>
        /// <param name="searchstr"></param>
        /// <returns>200 with a List of <see cref="TypeDTO"/></returns>
        [HttpGet, Authorize]
        [Route("GetAll/{searchstr}")]
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
            return Ok(await _uow.AccountTypeRepository.GetAll(searchstr));
        }
        /// <summary>
        /// This will return the account type matching the id
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
            return Ok(await _uow.AccountTypeRepository.GetById(id));
        }
        /// <summary>
        /// This will create a new account type
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(TypeDTO account)
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
                var acc = _uow.AccountTypeRepository.Create(account);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This will delete the account type
        /// </summary>
        /// <param name="account"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [HttpDelete("{reason}"), Authorize]
        public async Task<IActionResult> Delete(TypeDTO account, string reason)
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
                var acc = await _uow.AccountTypeRepository.DeActivate(account, reason);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This will activate the account type
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost("Activate"), Authorize]
        public async Task<IActionResult> Activate(TypeDTO account)
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
                var acc = await _uow.AccountTypeRepository.Activate(account);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This will update the account type
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(TypeDTO account)
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
                var acc = await _uow.AccountTypeRepository.Update(account);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This will check if the account type already exists
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost("IsExisting"), Authorize]
        public async Task<IActionResult> IsExisting(TypeDTO type)
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
            return Ok(await _uow.AccountTypeRepository.IsExisitng(type));
        }
    }
}
