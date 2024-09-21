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
        public AdminController(IUnitOfWork unitOfWork, JwtService jwtService)
        {
            _uow = unitOfWork;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null) 
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Admin")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Read")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            var admins = await _uow.AdminRepository.GetAll();
            return Ok(admins);
        }
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Admin")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Read")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            var admin = await _uow.AdminRepository.GetById(id);
            if (admin is null)
                return NotFound();
            return Ok(admin);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Admin admin)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Admin")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Add")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            if (_uow.AdminRepository.IsExisting(admin))
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
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,Admin admin)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Admin")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Add")).FirstOrDefault();
            if (role is null && per is null)
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
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> login(AuthenticateRequest model)
        {
            var response = await _uow.AdminRepository.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
        [HttpPost("HasAdminAccess")]
        [Authorize]
        public async Task<IActionResult> HasAdminAccess(HasAdminAccessRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            int level = Int32.Parse(User.Claims.First(x => x.Type == ClaimTypes.Name).Value);
            return Ok(await _uow.AdminRepository.HasAdminAccess(request));
        }
    }
}
