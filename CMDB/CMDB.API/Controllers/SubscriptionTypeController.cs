using CMDB.API.Models;
using CMDB.API.Services;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing subscription types, including retrieval, creation, activation, deactivation,
    /// and updates.
    /// </summary>
    /// <remarks>This controller requires authorized access and validates administrative permissions for all
    /// actions.  It interacts with the subscription type repository to perform CRUD operations and other related
    /// tasks.</remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionTypeController : ControllerBase
    {
        private SubscriptionTypeController()
        {
        }

        private readonly IUnitOfWork _uow;
        private readonly string site = "Subscription Type";
        private HasAdminAccessRequest request;
        /// <summary>
        /// Construvtor
        /// </summary>
        /// <param name="uow"><see cref="IUnitOfWork"/></param>
        public SubscriptionTypeController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        /// <summary>
        /// This will get all SubscriptionTypes
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
                Action = "Read"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.SubscriptionTypeRepository.GetAll());
        }
        /// <summary>
        /// This function will get all SubscriptionTypes with a search string
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
                Action = "Read"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.SubscriptionTypeRepository.GetAll(searchstr));
        }
        /// <summary>
        /// This function will get a SubscriptionType by ID
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
                Action = "Read"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            return Ok(await _uow.SubscriptionTypeRepository.GetById(id));
        }
        /// <summary>
        /// This function will create a new SubscriptionType
        /// </summary>
        /// <param name="type"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(SubscriptionTypeDTO type)
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
            type = _uow.SubscriptionTypeRepository.Create(type);
            await _uow.SaveChangesAsync();
            return Ok(type);
        }
        /// <summary>
        /// This function will deacticate a SubscriptionType with a reason
        /// </summary>
        /// <param name="type">The <see cref="SubscriptionTypeDTO"/></param>
        /// <param name="reason"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpDelete("{reason}"), Authorize]
        public async Task<IActionResult> Delete(SubscriptionTypeDTO type,string reason)
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
            type = await _uow.SubscriptionTypeRepository.Deactivate(type,reason);
            await _uow.SaveChangesAsync();
            return Ok(type);
        }
        /// <summary>
        /// This function will activate a SubscriptionType
        /// </summary>
        /// <param name="typeDTO">The <see cref="SubscriptionTypeDTO"/></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPost("Activate"), Authorize]
        public async Task<IActionResult> Activate(SubscriptionTypeDTO typeDTO)
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
            typeDTO = await _uow.SubscriptionTypeRepository.Activate(typeDTO);
            await _uow.SaveChangesAsync();
            return Ok(typeDTO);
        }
        /// <summary>
        /// This will update a given SubscriptionType
        /// </summary>
        /// <param name="typeDTO"><see cref="SubscriptionTypeDTO"/></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(SubscriptionTypeDTO typeDTO)
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
            typeDTO = await _uow.SubscriptionTypeRepository.Update(typeDTO);
            await _uow.SaveChangesAsync();
            return Ok(typeDTO);
        }
        /// <summary>
        /// The function will check if the SubscriptionType already exists
        /// </summary>
        /// <param name="subscriptionType"><see cref="SubscriptionTypeDTO"/></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPost("IsExisting"),Authorize]
        public async Task<IActionResult> IsExisting(SubscriptionTypeDTO subscriptionType)
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
            return Ok(await _uow.SubscriptionTypeRepository.IsExisting(subscriptionType));
        }
    }
}
