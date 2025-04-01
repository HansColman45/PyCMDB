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
    public class DeviceController : ControllerBase
    {
        private IUnitOfWork _uow;

        private HasAdminAccessRequest request;
        public DeviceController(IUnitOfWork uow)
        {
            _uow = uow;
        }
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
                Action = "Read"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.GetAll(category));
        }
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
                Action = "Read"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.GetAll(category, searchstr));
        }
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
                Action = "Read"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var device = await _uow.DeviceRepository.GetByAssetTag(category, assetTag);
            return Ok(device);
        }
        [HttpGet("{assetTag}"),Authorize]
        public async Task<IActionResult> GetByAssetTag(string assetTag)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.GetByAssetTag(assetTag));
        }
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
                Action = "Create"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            device =_uow.DeviceRepository.Create(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
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
                Action = "Edit"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            device = await _uow.DeviceRepository.Update(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
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
                Action = "Deactivate"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            device = await _uow.DeviceRepository.Deactivate(device, reason);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
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
                Action = "Activate"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            device = await _uow.DeviceRepository.Activate(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
        [HttpGet("GetAllRams"), Authorize]
        public async Task<IActionResult> GetAllRams()
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.GetAllRams());
        }
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
                Action = "AssignIdentity"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            device = await _uow.DeviceRepository.AssignIdentity(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
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
                Action = "ReleaseIdentity"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            device = await _uow.DeviceRepository.ReleaseIdentity(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
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
                Action = "AssignKensington"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            await _uow.DeviceRepository.AssignKensington(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
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
                Action = "ReleaseKensington"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            await _uow.DeviceRepository.ReleaseKensington(device);
            await _uow.SaveChangesAsync();
            return Ok(device);
        }
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
                Action = "Read"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.IsDeviceExising(device));
        }
        [HttpGet("AllFreeDevices"), Authorize]
        public async Task<IActionResult> ListAllFreeDevices()
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.ListAllFreeDevices());
        }
        [HttpGet("AllFreeDevices/{sitePart}"), Authorize]
        public async Task<IActionResult> ListAllFreeDevices(string sitePart)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            return Ok(await _uow.DeviceRepository.ListAllFreeDevices(sitePart));
        }
    }
}
