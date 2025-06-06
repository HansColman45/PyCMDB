using CMDB.API.Interfaces;
using CMDB.API.Models;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Controller for managing asset categories
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AssetCategoryController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly string site = "Asset Category";
        private HasAdminAccessRequest request;
        private AssetCategoryController()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uow"><see cref="IUnitOfWork"/></param>
        public AssetCategoryController(IUnitOfWork uow)
        {
                _uow = uow;
        }
        /// <summary>
        /// Will get all asset categories
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
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
            return Ok(await _uow.AssetCategoryRepository.GetById(id));
        }
        /// <summary>
        /// Will get all asset categories
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
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
            return Ok(await _uow.AssetCategoryRepository.GetAll());
        }
        /// <summary>
        /// Will get all asset categories with a search string
        /// </summary>
        /// <param name="searchstr"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
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
            return Ok(await _uow.AssetCategoryRepository.GetAll(searchstr));
        }
        /// <summary>
        /// Will get all asset categories with a category string
        /// </summary>
        /// <param name="category"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpGet("{category}"),Authorize]
        public async Task<IActionResult> GetBycategory(string category)
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
            return Ok(await _uow.AssetCategoryRepository.GetByCategory(category));
        }
        /// <summary>
        /// Will create a new asset category
        /// </summary>
        /// <param name="assetCategory"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(AssetCategoryDTO assetCategory)
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
            assetCategory = _uow.AssetCategoryRepository.Create(assetCategory);
            await _uow.SaveChangesAsync();
            return Ok(assetCategory);
        }
        /// <summary>
        /// Will update an existing asset category
        /// </summary>
        /// <param name="assetCategory"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(AssetCategoryDTO assetCategory)
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
            assetCategory = await _uow.AssetCategoryRepository.Update(assetCategory);
            await _uow.SaveChangesAsync();
            return Ok(assetCategory);
        }
        /// <summary>
        /// Will deactivate an existing asset category
        /// </summary>
        /// <param name="assetCategory"><see cref="AssetCategoryDTO"/></param>
        /// <param name="reason">The reason of terminaton</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpDelete("{reason}")]
        public async Task<IActionResult> Delete(AssetCategoryDTO assetCategory, string reason)
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
            assetCategory = await _uow.AssetCategoryRepository.Delete(assetCategory, reason);
            await _uow.SaveChangesAsync();
            return Ok(assetCategory);
        }
        /// <summary>
        /// Will activate an existing asset category
        /// </summary>
        /// <param name="assetCategory"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPost("Activate"), Authorize]
        public async Task<IActionResult> Activate(AssetCategoryDTO assetCategory)
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
            assetCategory = await _uow.AssetCategoryRepository.Activate(assetCategory);
            await _uow.SaveChangesAsync();
            return Ok(assetCategory);
        }
        /// <summary>
        /// Will check if the asset category exists
        /// </summary>
        /// <param name="assetCategory"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpGet("IsExisting"),Authorize]
        public async Task<IActionResult> IsCategoryExisting(AssetCategoryDTO assetCategory) 
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
            return Ok(await _uow.AssetCategoryRepository.IsCategoryExisting(assetCategory));
        }
    }
}
