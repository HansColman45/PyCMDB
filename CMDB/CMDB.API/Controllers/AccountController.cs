using CMDB.API.Models;
using CMDB.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll() 
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Account")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Read")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            return Ok(await _accountService.ListAll());
        }
        [HttpGet]
        [Route("GetAll/{searchstr}")]
        [Authorize]
        public async Task<IActionResult> GetAll(string searchstr)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Account")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Read")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            return Ok(await _accountService.ListAll(searchstr));
        }
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Account")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Read")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            return Ok(await _accountService.GetById(id));
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(AccountDTO account)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Account")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Create")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            if (_accountService.IsAccountExisting(account)) 
            { 
                ModelState.AddModelError("UserExisits", "User allready exists");
                return NotFound(ModelState);
            }
            try
            {
                var admin = await _accountService.CreateNew(account);
                return Ok(admin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
