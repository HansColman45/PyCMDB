using CMDB.API.Interfaces;
using CMDB.API.Models;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Controller for Kensington
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class KensingtonController : ControllerBase 
    {
        private KensingtonController()
        {
        }

        private readonly IUnitOfWork _uow;
        private readonly string site = "Kensington";
        private HasAdminAccessRequest request;
        /// <summary>
        /// Constructor for KensingtonController
        /// </summary>
        /// <param name="uow"></param>
        public KensingtonController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        /// <summary>
        /// This will list all Kensingtons
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
            var accounts = await _uow.KensingtonRepository.ListAll();
            return Ok(accounts);
        }
        /// <summary>
        /// This will return a list of all Kensingtons matching the search string
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
            var accounts = await _uow.KensingtonRepository.ListAll(searchstr);
            return Ok(accounts);
        }
        /// <summary>
        /// This will return a Kensington by Id
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
            return Ok(await _uow.KensingtonRepository.GetById(id));
        }
        /// <summary>
        /// This will create a new Kensington
        /// </summary>
        /// <param name="kensington"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(KensingtonDTO kensington)
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
            var newKensington = _uow.KensingtonRepository.Create(kensington);
            await _uow.SaveChangesAsync();
            return Ok(newKensington);
        }
        /// <summary>
        /// This will delete a Kensington
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [HttpDelete("{reason}"), Authorize]
        public async Task<IActionResult> Delete(KensingtonDTO key, string reason)
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
            var deletedKensington = await _uow.KensingtonRepository.DeActivate(key, reason);
            await _uow.SaveChangesAsync();
            return Ok(deletedKensington);
        }
        /// <summary>
        /// This will update a Kensington
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(KensingtonDTO key)
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
            key = await _uow.KensingtonRepository.Update(key);
            await _uow.SaveChangesAsync();
            return Ok(key);
        }
        /// <summary>
        /// This will activate a Kensington
        /// </summary>
        /// <param name="kensington"></param>
        /// <returns></returns>
        [HttpPost("Activate"), Authorize]
        public async Task<IActionResult> Activate(KensingtonDTO kensington)
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
            var activatedKensington = await _uow.KensingtonRepository.Activate(kensington);
            await _uow.SaveChangesAsync();
            return Ok(activatedKensington);
        }
        /// <summary>
        /// This will assign a Kensington to a device
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost("AssignDevice"), Authorize]
        public async Task<IActionResult> AssignKey2Device(KensingtonDTO key)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.AssignDevice
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            await _uow.KensingtonRepository.AssignDevice(key);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// This will release a Kensington from a device
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost("ReleaseDevice"), Authorize]
        public async Task<IActionResult> ReleaseKeyFromDevice(KensingtonDTO key)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.ReleaseDevice
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            await _uow.KensingtonRepository.ReleaseDevice(key);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// This will list all Kensingtons that are free
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllFreeKeys"), Authorize]
        public async Task<IActionResult> GetAllFreeKeys()
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
            var accounts = await _uow.KensingtonRepository.ListAllFreeKeys();
            return Ok(accounts);
        }
    }
}
