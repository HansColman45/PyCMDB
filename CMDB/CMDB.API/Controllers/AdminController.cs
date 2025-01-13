using CMDB.API.Services;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private HasAdminAccessRequest request;
        private readonly string site = "Admin";
        public AdminController(IUnitOfWork unitOfWork, JwtService jwtService)
        {
            _uow = unitOfWork;
        }
        [HttpGet, Authorize]
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
            var admins = await _uow.AdminRepository.GetAll();
            return Ok(admins);
        }
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
                Action = "Read"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.AdminRepository.GetById(id));
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(Admin admin)
        {
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
            if (await _uow.AdminRepository.IsExisting(admin))
            {
                ModelState.AddModelError("AdminExisist", "Admin is already existing");
                return BadRequest(ModelState);
            }
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
        [HttpPut("{id:int}"), Authorize]
        public async Task<IActionResult> Update(int id,Admin admin)
        {
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
                var ad = await _uow.AdminRepository.Update(admin, id);
                await _uow.SaveChangesAsync();
                return Ok(ad);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex);
            }
        }
        [HttpPost("Login"), AllowAnonymous]
        public async Task<IActionResult> Login(AuthenticateRequest model)
        {
            var response = await _uow.AdminRepository.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
        [HttpPost("HasAdminAccess"), Authorize]
        public async Task<IActionResult> HasAdminAccess(HasAdminAccessRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            int level = Int32.Parse(User.Claims.First(x => x.Type == ClaimTypes.Name).Value);
            return Ok(await _uow.AdminRepository.HasAdminAccess(request));
        }
        [HttpPost("IsExisting"), Authorize]
        public async Task<IActionResult> IsExisting(Admin admin)
        {
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
            return Ok(await _uow.AdminRepository.IsExisting(admin));
        }
    }
}
