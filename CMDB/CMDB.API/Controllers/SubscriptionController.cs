using CMDB.API.Interfaces;
using CMDB.Domain.DTOs;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Provides endpoints for managing subscriptions, including retrieval, creation, updates, activation, deactivation,
    /// and assignment of subscription identities. Access to these endpoints is restricted to authorized users with 
    /// appropriate administrative permissions.
    /// </summary>
    /// <remarks>This controller handles various subscription-related operations, such as retrieving all
    /// subscriptions,  searching subscriptions, managing subscription states (e.g., activation, deactivation), and
    /// assigning or  releasing subscription identities. Each action requires the user to have valid administrative
    /// access,  which is verified through claims and the associated admin repository.  The controller uses dependency
    /// injection to access the unit of work pattern for interacting with repositories.  All changes are persisted using
    /// the unit of work's save mechanism.  Authorization is enforced on all endpoints, and unauthorized access will
    /// result in a 401 Unauthorized response.</remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private SubscriptionController()
        {
        }

        private readonly IUnitOfWork _uow;
        private readonly string site = "Subscription";
        private HasAdminAccessRequest request;
        /// <summary>
        /// Constructor for SubscriptionController
        /// </summary>
        /// <param name="uow"><see cref="IUnitOfWork"/></param>
        public SubscriptionController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        /// <summary>
        /// Will return all subscriptions
        /// </summary>
        /// <returns>List of <see cref="SubscriptionDTO"/></returns>
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
            return Ok(await _uow.SubscriptionRepository.GetAll());
        }
        /// <summary>
        /// Retrieves all subscriptions that match the specified search string.
        /// </summary>
        /// <remarks>This method requires the caller to be authorized and have administrative access. If
        /// the user does not have the necessary permissions, the method returns an unauthorized response.</remarks>
        /// <param name="searchstr">The search string used to filter subscriptions. Can be null or empty to retrieve all subscriptions.</param>
        /// <returns>An <see cref="OkResult"/> containing a collection of subscriptions that match the search criteria.
        /// Returns <see cref="UnauthorizedResult"/> if the user is not authorized.</returns>
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
            return Ok(await _uow.SubscriptionRepository.GetAll(searchstr));
        }
        /// <summary>
        /// Retrieves a subscription by its unique identifier.
        /// </summary>
        /// <remarks>This method requires the caller to be authorized and have administrative access. The
        /// user's administrative access is validated based on their claims and permissions. If the user does not have
        /// the required access, the method returns an unauthorized response.</remarks>
        /// <param name="id">The unique identifier of the subscription to retrieve. Must be a positive integer.</param>
        /// <returns>An <see cref="OkResult"/> containing the subscription details if the operation is successful. Returns
        /// <see cref="UnauthorizedResult"/> if the user is not authorized to access the resource.</returns>
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
            return Ok(await _uow.SubscriptionRepository.GetById(id));
        }
        /// <summary>
        /// Creates a new subscription for the authenticated user.
        /// </summary>
        /// <param name="subscription">The subscription details to be created.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authenticated or lacks the necessary permissions.  Returns
        /// <see cref="OkResult"/> if the subscription is successfully created.</returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(SubscriptionDTO subscription)
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
            subscription = _uow.SubscriptionRepository.Create(subscription);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Deletes a subscription and deactivates it with the specified reason.
        /// </summary>
        /// <remarks>This method requires the user to be authorized and have administrative access.</remarks>
        /// <param name="subscription">The subscription to be deleted.</param>
        /// <param name="reason">The reason for deactivating the subscription.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized,  or <see cref="OkObjectResult"/> containing the
        /// deleted subscription if the operation succeeds.</returns>
        [HttpDelete("{reason}"), Authorize]
        public async Task<IActionResult> Delete(SubscriptionDTO subscription, string reason)
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
            subscription = await _uow.SubscriptionRepository.Delete(subscription, reason);
            await _uow.SaveChangesAsync();
            return Ok(subscription);
        }
        /// <summary>
        /// This method activates a subscription
        /// </summary>
        /// <param name="subscription"><see cref="SubscriptionDTO"/></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized,  or <see cref="OkObjectResult"/></returns>
        [HttpPost("Activate"), Authorize]
        public async Task<IActionResult> Activate(SubscriptionDTO subscription)
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
            subscription = await _uow.SubscriptionRepository.Activate(subscription);
            await _uow.SaveChangesAsync();
            return Ok(subscription);
        }
        /// <summary>
        /// This will ipdate the given subscription
        /// </summary>
        /// <param name="subscription"><see cref="SubscriptionDTO"/> containing the updated fields</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(SubscriptionDTO subscription)
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
            subscription = await _uow.SubscriptionRepository.Update(subscription);
            await _uow.SaveChangesAsync();
            return Ok(subscription);
        }
        /// <summary>
        /// This method checks if a subscription already exists in the system.
        /// </summary>
        /// <param name="subscription">The <see cref="SubscriptionDTO"/></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/> with true or false</returns>
        [HttpPost("IsExisting"), Authorize]
        public async Task<IActionResult> IsExisting(SubscriptionDTO subscription)
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
            return Ok(await _uow.SubscriptionRepository.IsExisting(subscription));
        }
        /// <summary>
        /// This method retrieves all free subscriptions for a given category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpGet("ListAllFreeSubscriptions/{category}"), Authorize]
        public async Task<IActionResult> ListAllFreeSubscriptions(string category)
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
            var subs = await _uow.SubscriptionRepository.ListAllFreeSubscriptions(category);
            return Ok(subs);
        }
        /// <summary>
        /// Assigns an identity to an internet subscription based on the provided request.
        /// </summary>
        /// <remarks>This method requires the caller to be authorized and have appropriate admin access.
        /// The user's identity is retrieved from the claims, and the operation is performed only if the user has the
        /// necessary permissions.</remarks>
        /// <param name="internetSubscriptionRequest">The request containing the details of the internet subscription to which the identity will be assigned.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkResult"/> if the operation is
        /// successful.</returns>
        [HttpPost("AssignIdentity"), Authorize]
        public async Task<IActionResult> AssignIdentity(AssignInternetSubscriptionRequest internetSubscriptionRequest)
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
            await _uow.SubscriptionRepository.AssignIdentity(internetSubscriptionRequest);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// This method releases an identity from an internet subscription based on the provided request.
        /// </summary>
        /// <param name="internetSubscriptionRequest">The <see cref="AssignInternetSubscriptionRequest"/></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPost("ReleaseIdentity"), Authorize]
        public async Task<IActionResult> ReleaseIdentity(AssignInternetSubscriptionRequest internetSubscriptionRequest)
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
            await _uow.SubscriptionRepository.ReleaseIdentity(internetSubscriptionRequest);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// This method assigns a mobile subscription based on the provided request.
        /// </summary>
        /// <param name="assignMobile"><see cref="AssignMobileSubscriptionRequest"/></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPost("AssignMobile"), Authorize]
        public async Task<IActionResult> AssignMobile(AssignMobileSubscriptionRequest assignMobile)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.AssignMobile
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            await _uow.SubscriptionRepository.AssignMobile(assignMobile);
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// This method releases a mobile subscription based on the provided request.
        /// </summary>
        /// <param name="assignMobile">The <see cref="AssignMobileSubscriptionRequest"/></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPost("ReleaseMobile"),Authorize]
        public async Task<IActionResult> ReleaseMobile(AssignMobileSubscriptionRequest assignMobile)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Permission = Permission.ReleaseMobile
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            await _uow.SubscriptionRepository.ReleaseMobile(assignMobile);
            await _uow.SaveChangesAsync();
            return Ok();
        }
    }
}
