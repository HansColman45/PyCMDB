using CMDB.API.Interfaces;
using CMDB.API.Models;
using CMDB.Domain.Requests;
using CMDB.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Admin controller for managing admin accounts
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private HasAdminAccessRequest request;
        private readonly string site = "Admin";
        private AdminController(){}
        /// <summary>
        /// Constructor for the AdminController
        /// </summary>
        /// <param name="unitOfWork"></param>
        public AdminController(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        /// <summary>
        /// Gets all admin accounts
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
                Permission = Permission.Read,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var admins = await _uow.AdminRepository.GetAll();
            return Ok(admins);
        }
        /// <summary>
        /// Gets all admin accounts with a search string
        /// </summary>
        /// <param name="searchstr"></param>
        /// <returns></returns>
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
                Permission = Permission.Read,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var admins = await _uow.AdminRepository.GetAll(searchstr);
            return Ok(admins);
        }
        /// <summary>
        /// Gets an admin account by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}"), Authorize]
        public async Task<IActionResult> Get(int id)
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
            return Ok(await _uow.AdminRepository.GetById(id));
        }
        /// <summary>
        /// Creates a new admin account
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(AdminDTO admin)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.Add,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            try
            {
                var ad = _uow.AdminRepository.Add(admin);
                await _uow.SaveChangesAsync();
                return Ok(ad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This will update the admin account
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPut(), Authorize]
        public async Task<IActionResult> Update(AdminDTO admin)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.Update,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            try 
            {
                var ad = await _uow.AdminRepository.Update(admin);
                await _uow.SaveChangesAsync();
                return Ok(ad);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This will do the authentication for the admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns><see cref="AuthenticateResponse"/></returns>
        [HttpPost("Login"), AllowAnonymous]
        public async Task<IActionResult> Login(AuthenticateRequest model)
        {
            var response = await _uow.AdminRepository.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
        /// <summary>
        /// Checks if the admin has access to a site
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("HasAdminAccess"), Authorize]
        public async Task<IActionResult> HasAdminAccess(HasAdminAccessRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            try
            {
                return Ok(await _uow.AdminRepository.HasAdminAccess(request));
            }
            catch (Exception e)
            {
                BadRequest(e);
                throw;
            }
        }
        /// <summary>
        /// Checks if the admin account already exists
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPost("IsExisting"), Authorize]
        public async Task<IActionResult> IsExisting(AdminDTO admin)
        {
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
            return Ok(await _uow.AdminRepository.IsExisting(admin));
        }
        /// <summary>
        /// Deactivates the admin account
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [HttpDelete("{reason}")]
        public async Task<IActionResult> Delete(AdminDTO dto, string reason)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.Delete,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            try
            {
                var ad = await _uow.AdminRepository.DeActivate(dto, reason);
                await _uow.SaveChangesAsync();
                return Ok(ad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// Activates the admin account
        /// </summary>
        /// <param name="dTO"></param>
        /// <returns></returns>
        [HttpPost("Activate"), Authorize]
        public async Task<IActionResult> Activate(AdminDTO dTO)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.Activate,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            try
            {
                var ad = await _uow.AdminRepository.Activate(dTO);
                await _uow.SaveChangesAsync();
                return Ok(ad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
