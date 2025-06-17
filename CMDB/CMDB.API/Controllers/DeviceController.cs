using CMDB.API.Interfaces;
using CMDB.Domain.DTOs;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Controller for managing devices
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private DeviceController()
        {
        }
        private IUnitOfWork _uow;

        private HasAdminAccessRequest request;
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="uow"></param>
        public DeviceController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        /// <summary>
        /// This will return all the devices of a specific category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>List of <see cref="DeviceDTO"/></returns>
        [HttpGet("{category}/GetAll"), Authorize]
        public async Task<IActionResult> GetAll(string category)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = category.ToLower(),
                Permission = Permission.Read
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.GetAll(category));
        }
        /// <summary>
        /// This will return all the devices of a specific category matching the search string
        /// </summary>
        /// <param name="category"></param>
        /// <param name="searchstr"></param>
        /// <returns>List of <see cref="DeviceDTO"/></returns>
        [HttpGet("{category}/GetAll/{searchstr}"), Authorize]
        public async Task<IActionResult> GetAll(string category, string searchstr)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = category.ToLower(),
                Permission = Permission.Read
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.GetAll(category, searchstr));
        }
        /// <summary>
        /// This will return a device by its asset tag
        /// </summary>
        /// <param name="category"></param>
        /// <param name="assetTag"></param>
        /// <returns><see cref="DeviceDTO"/></returns>
        [HttpGet("{category}/{assetTag}"), Authorize]
        public async Task<IActionResult> GetByAssetTag(string category, string assetTag)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = category.ToLower(),
                Permission = Permission.Read
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var device = await _uow.DeviceRepository.GetByAssetTag(category, assetTag);
            return Ok(device);
        }
        /// <summary>
        /// This will return a device by its asset tag
        /// </summary>
        /// <param name="assetTag"></param>
        /// <returns><see cref="DeviceDTO"/></returns>
        [HttpGet("{assetTag}"),Authorize]
        public async Task<IActionResult> GetByAssetTag(string assetTag)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.GetByAssetTag(assetTag));
        }
        /// <summary>
        /// This will create a new device
        /// </summary>
        /// <param name="device"><see cref="DeviceDTO"/></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(DeviceDTO device)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = device.Category.Category.ToLower(),
                Permission = Permission.Add,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            device =_uow.DeviceRepository.Create(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
        /// <summary>
        /// This will update a device
        /// </summary>
        /// <param name="device"><see cref="DeviceDTO"/></param>
        /// <returns></returns>
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(DeviceDTO device)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = device.Category.Category.ToLower(),
                Permission = Permission.Update,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            device = await _uow.DeviceRepository.Update(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
        /// <summary>
        /// This will deactivate a device
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="device"><see cref="DeviceDTO"/></param>
        /// <returns></returns>
        [HttpDelete("{reason}"),Authorize]
        public async Task<IActionResult> Deactivate(string reason, DeviceDTO device)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = device.Category.Category.ToLower(),
                Permission = Permission.Delete,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            device = await _uow.DeviceRepository.Deactivate(device, reason);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
        /// <summary>
        /// This will activate a device
        /// </summary>
        /// <param name="device"><see cref="DeviceDTO"/></param>
        /// <returns></returns>
        [HttpPost("Activate"), Authorize]
        public async Task<IActionResult> Activate(DeviceDTO device)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = device.Category.Category.ToLower(),
                Permission = Permission.Activate,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            device = await _uow.DeviceRepository.Activate(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
        /// <summary>
        /// This will return a list of all know RAMs
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllRams"), Authorize]
        public async Task<IActionResult> GetAllRams()
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.GetAllRams());
        }
        /// <summary>
        /// This will assign an identity to a device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [HttpPost("AssignIdentity"), Authorize]
        public async Task<IActionResult> AssignIdentity(DeviceDTO device)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = device.Category.Category.ToLower(),
                Permission = Permission.AssignIdentity,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            device = await _uow.DeviceRepository.AssignIdentity(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
        /// <summary>
        /// This will release an identity from a device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [HttpPost("ReleaseIdentity"),Authorize]
        public async Task<IActionResult> ReleaseIdentity(DeviceDTO device)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = device.Category.Category.ToLower(),
                Permission = Permission.ReleaseIdentity,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            device = await _uow.DeviceRepository.ReleaseIdentity(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
        /// <summary>
        /// This will assign a kensington to a device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [HttpPost("AssignKensington"),Authorize]
        public async Task<IActionResult> AssignKensington(DeviceDTO device)
        {
            if (device is null)
                return BadRequest("Device is required");
            if (device.Kensington == null)
                return BadRequest("Kensington is required");
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = device.Category.Category.ToLower(),
                Permission = Permission.AssignKensington,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            await _uow.DeviceRepository.AssignKensington(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
        /// <summary>
        /// This will release a kensington from a device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [HttpPost("ReleaseKensington"), Authorize]
        public async Task<IActionResult> ReleaseKensington(DeviceDTO device)
        {
            if (device is null)
                return BadRequest("Device is required");
            if (device.Kensington == null)
                return BadRequest("Kensington is required");
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = device.Category.Category.ToLower(),
                Permission = Permission.ReleaseKensington,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            await _uow.DeviceRepository.ReleaseKensington(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
        /// <summary>
        /// This will check if a device is existing
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [HttpPost("IsExisting"), Authorize]
        public async Task<IActionResult> IsDeviceExisting(DeviceDTO device)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = device.Category.Category.ToLower(),
                Permission = Permission.Read,
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.IsDeviceExising(device));
        }
        /// <summary>
        /// This will return all the free devices
        /// </summary>
        /// <returns></returns>
        [HttpGet("AllFreeDevices"), Authorize]
        public async Task<IActionResult> ListAllFreeDevices()
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.ListAllFreeDevices());
        }
        /// <summary>
        /// This will return all the free devices of a specific category
        /// </summary>
        /// <param name="sitePart"></param>
        /// <returns></returns>
        [HttpGet("AllFreeDevices/{sitePart}"), Authorize]
        public async Task<IActionResult> ListAllFreeDevices(string sitePart)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = sitePart,
                Permission = Permission.Read
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.ListAllFreeDevices(sitePart));
        }
    }
}
