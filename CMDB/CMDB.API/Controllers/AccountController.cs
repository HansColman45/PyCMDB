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
        private readonly IUnitOfWork _uow;
        public AccountController(IUnitOfWork uow)
        {
            _uow = uow;
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
            var accounts = await _uow.AccountRepository.GetAll();
            return Ok(accounts);
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
            var accounts = await _uow.AccountRepository.GetAll(searchstr);
            return Ok(accounts);
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
            var account = await _uow.AccountRepository.GetById(id);
            return Ok(account);
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
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Edit")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            if (await _uow.AccountRepository.IsExisitng(account)) 
            { 
                ModelState.AddModelError("UserExisits", "User allready exists");
                return NotFound(ModelState);
            }
            try
            {
                var acc = await _uow.AccountRepository.Create(account);
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("{reason:alpha}")] 
        [Authorize]
        public async Task<IActionResult> Delete(AccountDTO account,string reason)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Account")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Deactivate")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            try
            {
                var acc = await _uow.AccountRepository.DeActivate(account, reason);
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("Activate")]
        [Authorize]
        public async Task<IActionResult> Activate(AccountDTO account)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Account")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Activate")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            try
            {
                var acc = await _uow.AccountRepository.Activate(account);
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("IsAccountExisting")]
        [Authorize]
        public async Task<IActionResult> IsAccountExisting(AccountDTO account)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Account")).FirstOrDefault();
            if (role is null)
                return Unauthorized(); 
            return Ok(await _uow.AccountRepository.IsExisitng(account));
        }
        [HttpPost("AssingIdentity")]
        [Authorize]
        public async Task<IActionResult> AssignIdentity(IdenAccountDTO request)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Account")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("AssignIdentity")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            var accountdto = await _uow.AccountRepository.GetById(request.Account.AccID);
            var idenityt = await _uow.IdentityRepository.GetById(request.Identity.IdenId);
            if (accountdto is null || idenityt is null)
                return NotFound();
            await _uow.AccountRepository.AssignAccount2Identity(request);
            await _uow.SaveChangesAsync();
            return Ok(accountdto);
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(AccountDTO account)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Account")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Edit")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            try
            {
                var acc = await _uow.AccountRepository.Update(account);
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
