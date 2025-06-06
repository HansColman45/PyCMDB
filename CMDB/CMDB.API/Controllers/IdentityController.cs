using CMDB.API.Interfaces;
using CMDB.API.Models;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Controller for managing identities
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<IdentityController> _logger;
        private readonly string site = "Identity";
        private HasAdminAccessRequest request;
        /// <summary>
        /// Constructor for the IdentityController
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="logger"></param>
        public IdentityController(IUnitOfWork uow, ILogger<IdentityController> logger)
        {
            _uow = uow;
            _logger = logger;
        }
        /// <summary>
        /// This will return all the identities
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
                Permission = Permission.Read
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.IdentityRepository.GetAll());
        }
        /// <summary>
        /// This will return all the identities matching the search string
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
                Permission = Permission.Read
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.IdentityRepository.GetAll(searchstr));
        }
        /// <summary>
        /// This will return the identity matching the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                Permission = Permission.Read
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.IdentityRepository.GetById(id));
        }
        /// <summary>
        /// This will assign an account to an identity
        /// </summary>
        /// <param name="idenAccount"></param>
        /// <returns></returns>
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
                Permission = Permission.AssignKensington,
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
        /// <summary>
        /// This will release an account from an identity
        /// </summary>
        /// <param name="idenAccount"></param>
        /// <returns></returns>
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
                Permission = Permission.ReleaseAccount,
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
        /// <summary>
        /// This will create a new identity
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
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
                Permission = Permission.Add,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            _uow.IdentityRepository.Create(identity);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// This will deactivate an identity
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
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
                Permission = Permission.Delete,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var iden = await _uow.IdentityRepository.Deactivate(identity,reason);
            await _uow.SaveChangesAsync();
            return Ok(iden);
        }
        /// <summary>
        /// This will activate an identity
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
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
                Permission = Permission.Activate,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var iden = await _uow.IdentityRepository.Activate(identity);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// This will update an identity
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
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
                Permission = Permission.Update,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var acc = await _uow.IdentityRepository.Update(identity);
            await _uow.SaveChangesAsync();
            return Ok(acc);
        }
        /// <summary>
        /// This will return all the free identities
        /// </summary>
        /// <param name="sitePart"></param>
        /// <returns></returns>
        [HttpGet("ListAllFreeIdentities/{sitePart}"), Authorize]
        public async Task<IActionResult> ListAllFreeIdentities(string sitePart)
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
            _logger.LogTrace($"The sitePart is: {sitePart}");
            return Ok(await _uow.IdentityRepository.ListAllFreeIdentities(sitePart));
        }
        /// <summary>
        /// This will check if the identity already exists
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
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
                Permission = Permission.Read,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.IdentityRepository.IsExisting(identity));
        }
        /// <summary>
        /// This will assign devices to an identity
        /// </summary>
        /// <param name="assignDevice"></param>
        /// <returns></returns>
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
                Permission = Permission.AssignDevice,
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
        /// <summary>
        /// This will release devices from an identity
        /// </summary>
        /// <param name="assignDevice"></param>
        /// <returns></returns>
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
                Permission = Permission.ReleaseDevice,
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
        /// <summary>
        /// This will assign mobiles to an identity
        /// </summary>
        /// <param name="assignMobile"></param>
        /// <returns></returns>
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
                Permission = Permission.AssignMobile,
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
        /// <summary>
        /// This will release mobiles from an identity
        /// </summary>
        /// <param name="assignMobile"></param>
        /// <returns></returns>
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
                Permission = Permission.ReleaseMobile,
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
        /// <summary>
        /// This will assign subscriptions to an identity
        /// </summary>
        /// <param name="assignSubscription"></param>
        /// <returns></returns>
        [HttpPost("AssignSubscription"), Authorize]
        public async Task<IActionResult> AssignSubscription(AssignInternetSubscriptionRequest assignSubscription)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.AssignSubscription,
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
        /// <summary>
        /// This will release subscriptions from an identity
        /// </summary>
        /// <param name="assignSubscription"></param>
        /// <returns></returns>
        [HttpPost("ReleaseSubscription"), Authorize]
        public async Task<IActionResult> ReleaseSubscription(AssignInternetSubscriptionRequest assignSubscription)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.ReleaseSubscription,
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
