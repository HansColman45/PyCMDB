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
        private IAdminService adminService;
        public AdminController(IAdminService service)
        {
            adminService = service;
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
            return Ok(await adminService.GetAll());
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
            return Ok(await adminService.GetById(id));
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
            if (adminService.IsExisting(admin))
            {
                ModelState.AddModelError("AdminExisist", "Admin is already existing");
                return BadRequest(ModelState);
            }
            var _admin = await adminService.Create(admin);
            if (_admin == null)
                return NotFound();
            
            return Ok(_admin);
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
            var _admin = await adminService.Update(admin);
            if ( _admin is null)
                return NotFound();
            return Ok(_admin);
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> login(AuthenticateRequest model)
        {
            var response = await adminService.Authenticate(model);

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
            return Ok(await adminService.HasAdminAccess(request));
        }
    }
}
