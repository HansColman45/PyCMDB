using CMDB.API.Models;
using CMDB.API.Services;
using CMDB.Domain.Requests;
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
        private readonly string site = "Account";
        private HasAdminAccessRequest request;
        public AccountController(IUnitOfWork uow)
        {
            _uow = uow;
        }
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
            var accounts = await _uow.AccountRepository.GetAll();
            return Ok(accounts);
        }
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
            var accounts = await _uow.AccountRepository.GetAll(searchstr);
            return Ok(accounts);
        }
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
            var account = await _uow.AccountRepository.GetById(id);
            return Ok(account);
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(AccountDTO account)
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
            if (await _uow.AccountRepository.IsExisitng(account)) 
            { 
                ModelState.AddModelError("UserExisits", "User allready exists");
                return NotFound(ModelState);
            }
            try
            {
                var acc = _uow.AccountRepository.Create(account);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("{reason}"), Authorize]
        public async Task<IActionResult> Delete(AccountDTO account,string reason)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var level = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "Deactivate"
            };
            var hasAdminAcces =  await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            try
            {
                var acc = await _uow.AccountRepository.DeActivate(account, reason);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("Activate"), Authorize]
        public async Task<IActionResult> Activate(AccountDTO account)
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
                var acc = await _uow.AccountRepository.Activate(account);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("IsExisting"), Authorize]
        public async Task<IActionResult> IsExisting(AccountDTO account)
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
            return Ok(await _uow.AccountRepository.IsExisitng(account));
        }
        [HttpPost("AssingIdentity"), Authorize]
        public async Task<IActionResult> AssignIdentity(IdenAccountDTO idenAccount)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "AssignIdentity"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var accountdto = await _uow.AccountRepository.GetById(idenAccount.Account.AccID);
            var identity = await _uow.IdentityRepository.GetById(idenAccount.Identity.IdenId);
            if (accountdto is null || identity is null)
                return NotFound();
            await _uow.AccountRepository.AssignAccount2Identity(idenAccount);
            await _uow.SaveChangesAsync();
            return Ok(accountdto);
        }
        [HttpPost("ReleaseIdentity"), Authorize]
        public async Task<IActionResult> ReleaseIdentity(IdenAccountDTO idenAccount)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "ReleaseIdentity"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var accountdto = await _uow.AccountRepository.GetById(idenAccount.Account.AccID);
            var identity = await _uow.IdentityRepository.GetById(idenAccount.Identity.IdenId);
            if (accountdto is null || identity is null)
                return NotFound();
            await _uow.AccountRepository.ReleaseAccountFromIdentity(idenAccount);
            await _uow.SaveChangesAsync();
            return Ok(idenAccount);
        }
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(AccountDTO account)
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
                var acc = await _uow.AccountRepository.Update(account);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("ListAllFreeAccounts"), Authorize]
        public async Task<IActionResult> ListAllFreeAccounts()
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            return Ok(await _uow.AccountRepository.ListAllFreeAccounts());
        }
    }
}
