using CMDB.API.Interfaces;
using CMDB.API.Models;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Controller for managing asset types
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AssetTypeController : ControllerBase
    {
        private AssetTypeController()
        {
        }   
        private readonly IUnitOfWork _uow;
        private readonly string site = "Asset Type";
        private HasAdminAccessRequest request;
        /// <summary>
        /// The constructor for the AssetTypeController
        /// </summary>
        /// <param name="uow"></param>
        public AssetTypeController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        /// <summary>
        /// This will return all the asset types
        /// </summary>
        /// <returns>List of <see cref="AssetTypeDTO"/></returns>
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
            return Ok(await _uow.AssetTypeRepository.GetAll());
        }
        /// <summary>
        /// This will return all the asset types matching the search string
        /// </summary>
        /// <param name="searchstr"></param>
        /// <returns>List of <see cref="AssetTypeDTO"/></returns>
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
            return Ok(await _uow.AssetTypeRepository.GetAll(searchstr));
        }
        /// <summary>
        /// This will return all the asset types matching the category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>List of <see cref="AssetTypeDTO"/></returns>
        [HttpGet("GetByCategory/{category}"), Authorize]
        public async Task<IActionResult> GetByAssetCategory(string category)
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
            return Ok(await _uow.AssetTypeRepository.GetByCategory(category));
        }
        /// <summary>
        /// This will create a new asset type
        /// </summary>
        /// <param name="assetTypeDTO"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(AssetTypeDTO assetTypeDTO)
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
            assetTypeDTO = _uow.AssetTypeRepository.Create(assetTypeDTO);
            await _uow.SaveChangesAsync();
            return Ok(assetTypeDTO);
        }
        /// <summary>
        /// This will update an existing asset type
        /// </summary>
        /// <param name="assetTypeDTO"></param>
        /// <returns></returns>
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(AssetTypeDTO assetTypeDTO)
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
            assetTypeDTO = await _uow.AssetTypeRepository.Update(assetTypeDTO);
            await _uow.SaveChangesAsync();
            return Ok(assetTypeDTO);
        }
        /// <summary>
        /// This will deactivate an existing asset type
        /// </summary>
        /// <param name="assetTypeDTO"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [HttpDelete("{reason}"), Authorize]
        public async Task<IActionResult> Deactivate(AssetTypeDTO assetTypeDTO, string reason)
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
            assetTypeDTO = await _uow.AssetTypeRepository.Deactivate(assetTypeDTO, reason);
            await _uow.SaveChangesAsync();
            return Ok(assetTypeDTO);
        }
        /// <summary>
        /// This will return an asset type by ID
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
                Action = "Read"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.AssetTypeRepository.GetById(id));
        }
        /// <summary>
        /// This will activate an existing asset type
        /// </summary>
        /// <param name="assetTypeDTO"></param>
        /// <returns></returns>
        [HttpPost("Activate"), Authorize]
        public async Task<IActionResult> Activate(AssetTypeDTO assetTypeDTO)
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
            assetTypeDTO = await _uow.AssetTypeRepository.Activate(assetTypeDTO);
            await _uow.SaveChangesAsync();
            return Ok(assetTypeDTO);
        }
    }
}
