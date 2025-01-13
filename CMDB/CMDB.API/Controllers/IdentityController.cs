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
    public class IdentityController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly string site = "Identity";
        private HasAdminAccessRequest request;
        public IdentityController(IUnitOfWork uow)
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
            return Ok(await _uow.IdentityRepository.GetAll());
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
            return Ok(await _uow.IdentityRepository.GetAll(searchstr));
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
            return Ok(await _uow.IdentityRepository.GetById(id));
        }
        [HttpPost("AssignAccount"), Authorize]
        public async Task<IActionResult> AssignAccount(IdenAccountDTO idenAccount)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "AssignAccount"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var Iden = await _uow.IdentityRepository.GetById(idenAccount.Identity.IdenId);
            var account = await _uow.AccountRepository.GetById(idenAccount.Account.AccID);
            if (Iden is null || account is null)
                return NotFound();
            await _uow.IdentityRepository.AssignAccount(idenAccount);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("ReleaseAccount"), Authorize]
        public async Task<IActionResult> ReleaseAccount(IdenAccountDTO idenAccount)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "ReleaseAccount"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var Iden = await _uow.IdentityRepository.GetById(idenAccount.Identity.IdenId);
            var account = await _uow.AccountRepository.GetById(idenAccount.Account.AccID);
            if (Iden is null || account is null)
                return NotFound();
            await _uow.IdentityRepository.ReleaseAccount(idenAccount);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(IdentityDTO identity)
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
            _uow.IdentityRepository.Create(identity);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{reason}"), Authorize]
        public async Task<IActionResult> Delete(IdentityDTO identity, string reason)
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
            var iden = await _uow.IdentityRepository.Deactivate(identity,reason);
            await _uow.SaveChangesAsync();
            return Ok(iden);
        }
        [HttpPost("Activate"), Authorize]
        public async Task<IActionResult> Activate(IdentityDTO identity)
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
            var iden = await _uow.IdentityRepository.Activate(identity);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(IdentityDTO identity)
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
            var acc = await _uow.IdentityRepository.Update(identity);
            await _uow.SaveChangesAsync();
            return Ok(acc);
        }
        [HttpGet("ListAllFreeIdentities"), Authorize]
        public async Task<IActionResult> ListAllFreeIdentities()
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
            return Ok(await _uow.IdentityRepository.ListAllFreeIdentities());
        }
        [HttpPost("IsExisting"), Authorize]
        public async Task<IActionResult> IsIdentityExisting(IdentityDTO identity)
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
            return Ok(await _uow.IdentityRepository.IsExisting(identity));
        }
        [HttpPost("AssignDevices"), Authorize]
        public async Task<IActionResult> AssignDevices(AssignDeviceRequest assignDevice)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "AssignDevice"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var iden = await _uow.IdentityRepository.GetById(assignDevice.IdentityId);
            if (iden == null)
                return NotFound();
            await _uow.IdentityRepository.AssignDevices(iden, assignDevice.AssetTags);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("ReleaseDevices"), Authorize]
        public async Task<IActionResult> ReleaseDevices(AssignDeviceRequest assignDevice)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "ReleaseDevice"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var iden = await _uow.IdentityRepository.GetById(assignDevice.IdentityId);
            if (iden == null)
                return NotFound();
            await _uow.IdentityRepository.ReleaseDevices(iden, assignDevice.AssetTags);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("AssignMobile"),Authorize]
        public async Task<IActionResult> AssignMobile(AssignMobileRequest assignMobile)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "AssignDevice"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var iden = await _uow.IdentityRepository.GetById(assignMobile.IdentityId);
            if (iden == null)
                return NotFound();
            await _uow.IdentityRepository.AssignMobile(iden,assignMobile.MobileIds);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("ReleaseMobile"), Authorize]
        public async Task<IActionResult> ReleaseMobile(AssignMobileRequest assignMobile)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "ReleaseMobile"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var iden = await _uow.IdentityRepository.GetById(assignMobile.IdentityId);
            if (iden == null)
                return NotFound();
            await _uow.IdentityRepository.ReleaseMobile(iden,assignMobile.MobileIds);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("AssignSubscription"), Authorize]
        public async Task<IActionResult> AssignSubscription(AssignSubscriptionRequest assignSubscription)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "AssignSubscription"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var iden = await _uow.IdentityRepository.GetById(assignSubscription.IdentityId);
            if (iden == null)
                return NotFound();
            await _uow.IdentityRepository.AssignSubscription(iden,assignSubscription.SubscriptionIds);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("ReleaseSubscription"), Authorize]
        public async Task<IActionResult> ReleaseSubscription(AssignSubscriptionRequest assignSubscription)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "ReleaseSubscription"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var iden = await _uow.IdentityRepository.GetById(assignSubscription.IdentityId);
            if (iden == null)
                return NotFound();
            await _uow.IdentityRepository.ReleaseSubscription(iden, assignSubscription.SubscriptionIds);
            await _uow.SaveChangesAsync();
            return Ok();
        }
    }
}
