using CMDB.API.Interfaces;
using CMDB.Domain.DTOs;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// This is the Mobile controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MobileController : ControllerBase
    {
        private MobileController()
        {
        }
        private readonly IUnitOfWork _uow;
        private readonly string site = "Mobile";
        private HasAdminAccessRequest request;
        /// <summary>
        /// Mobile controller constructor
        /// </summary>
        /// <param name="uow"></param>
        public MobileController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        /// <summary>
        /// This will return all mobiles
        /// </summary>
        /// <returns>List of <see cref="MobileDTO"/></returns>
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
            return Ok(await _uow.MobileRepository.GetAll());
        }
        /// <summary>
        /// This will return all mobiles matchin the search string
        /// </summary>
        /// <param name="searchstr"></param>
        /// <returns>List of <see cref="MobileDTO"/></returns>
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
            return Ok(await _uow.MobileRepository.GetAll(searchstr));
        }
        /// <summary>
        /// This will return mobile by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="MobileDTO"/></returns>
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
            return Ok(await _uow.MobileRepository.GetById(id));
        }
        /// <summary>
        /// This will create a new mobile
        /// </summary>
        /// <param name="mobile">The <see cref="MobileDTO"/></param>
        /// <returns>200 OK</returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(MobileDTO mobile)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.Add
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            mobile = _uow.MobileRepository.Create(mobile);
            await _uow.SaveChangesAsync();
            return Ok(mobile);
        }
        /// <summary>
        /// This will activate an existing mobile
        /// </summary>
        /// <param name="mobile">The <see cref="MobileDTO"/></param>
        /// <returns>200 Ok</returns>
        [HttpPost("Activate"), Authorize]
        public async Task<IActionResult> Activate(MobileDTO mobile)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.Activate
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            mobile = await _uow.MobileRepository.Activate(mobile);
            await _uow.SaveChangesAsync();
            return Ok(mobile);
        }
        /// <summary>
        /// This will deactivate an existing mobile
        /// </summary>
        /// <param name="mobile">The <see cref="MobileDTO"/></param>
        /// <param name="reason">The reason of deactivation</param>
        /// <returns>200 Ok</returns>
        [HttpDelete("{reason}"), Authorize]
        public async Task<IActionResult> Delete(MobileDTO mobile, string reason)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.Delete
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            mobile = await _uow.MobileRepository.Delete(mobile,reason); 
            await _uow.SaveChangesAsync();
            return Ok(mobile);
        }
        /// <summary>
        /// This will update an existing mobile
        /// </summary>
        /// <param name="mobile">The <see cref="MobileDTO"/></param>
        /// <returns>200 Ok</returns>
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(MobileDTO mobile)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.Update
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            mobile = await _uow.MobileRepository.Update(mobile);
            await _uow.SaveChangesAsync();
            return Ok(mobile);
        }
        /// <summary>
        /// This will return all free mobiles
        /// </summary>
        /// <param name="sitePart"></param>
        /// <returns>List of <see cref="MobileDTO"/></returns>
        [HttpGet("ListAllFreeMobiles/{sitePart}"), Authorize]
        public async Task<IActionResult> ListAllFreeMobiles(string sitePart)
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
            return Ok(await _uow.MobileRepository.ListAllFreeMobiles(sitePart));
        }
        /// <summary>
        /// This will assign an identity to a mobile
        /// </summary>
        /// <param name="assignRequest"><see cref="AssignMobileRequest"/></param>
        /// <returns>200 Ok</returns>
        [HttpPost("AssignIdentity"),  Authorize]
        public async Task<IActionResult> AssignIdenity(AssignMobileRequest assignRequest)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.AssignIdentity
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var idenity = await _uow.IdentityRepository.GetById(assignRequest.IdentityId);
            if (idenity is null)
                return BadRequest();
            await _uow.MobileRepository.AssignIdentity(assignRequest);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// This will release an identity from a mobile
        /// </summary>
        /// <param name="assignRequest"></param>
        /// <returns>200 Ok</returns>
        [HttpPost("ReleaseIdentity"), Authorize]
        public async Task<IActionResult> ReleaseIdentity(AssignMobileRequest assignRequest)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.ReleaseIdentity
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var idenity = await _uow.IdentityRepository.GetById(assignRequest.IdentityId);
            if (idenity is null)
                return BadRequest();
            await _uow.MobileRepository.ReleaseIdentity(assignRequest);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// This will assign a subscription to a mobile
        /// </summary>
        /// <param name="assignMobileSubscription"></param>
        /// <returns></returns>
        [HttpPost("AssignSubscription"),Authorize]
        public async Task<IActionResult> AssignSubscription(AssignMobileSubscriptionRequest assignMobileSubscription)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.AssignSubscription
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            await _uow.MobileRepository.AssignSubscription(assignMobileSubscription);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// This will release a subscription from a mobile
        /// </summary>
        /// <param name="assignMobileSubscription"></param>
        /// <returns></returns>
        [HttpPost("ReleaseSubscription"), Authorize]
        public async Task<IActionResult> ReleaseSubscription(AssignMobileSubscriptionRequest assignMobileSubscription)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.ReleaseSubscription
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            await _uow.MobileRepository.ReleaseSubscription(assignMobileSubscription);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// This will check if the mobile already exists
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost("IsExisting"),Authorize]
        public async Task<IActionResult> IsExisting(MobileDTO mobile)
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
            return Ok(await _uow.MobileRepository.IsMobileExisting(mobile));
        }
    }
}
