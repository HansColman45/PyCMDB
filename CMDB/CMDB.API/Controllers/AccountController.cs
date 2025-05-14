using CMDB.API.Models;
using CMDB.API.Services;
using CMDB.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// This class is used to manage the accounts
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly string site = "Account";
        private readonly ILogger<AccountController> _logger;
        private HasAdminAccessRequest request;
        /// <summary>
        /// Constructor for the AccountController
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="logger"></param>
        public AccountController(IUnitOfWork uow, ILogger<AccountController> logger)
        {
            _uow = uow;
            _logger = logger;
        }
        /// <summary>
        /// This will return all the accounts
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpGet("GetAll"), Authorize]
        public async Task<IActionResult> GetAll() 
        {
            _logger.LogInformation($"Using the GetAll {nameof(AccountController)}");
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
            var accounts = await _uow.AccountRepository.GetAll();
            return Ok(accounts);
        }
        /// <summary>
        /// This will return all the accounts based on the search string
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
            var accounts = await _uow.AccountRepository.GetAll(searchstr);
            return Ok(accounts);
        }
        /// <summary>
        /// This function will return the account based on the id
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
            var account = await _uow.AccountRepository.GetById(id);
            return Ok(account);
        }
        /// <summary>
        /// This function will create a new account
        /// </summary>
        /// <param name="account"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(AccountDTO account)
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
            if (await _uow.AccountRepository.IsExisitng(account)) 
            { 
                ModelState.AddModelError("UserExisits", "User allready exists");
                return NotFound(ModelState);
            }
            try
            {
                var acc = _uow.AccountRepository.Create(account);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This function will delete the account
        /// </summary>
        /// <param name="account"></param>
        /// <param name="reason"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpDelete("{reason}"), Authorize]
        public async Task<IActionResult> Delete(AccountDTO account,string reason)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var level = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "Deactivate"
            };
            var hasAdminAcces =  await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            try
            {
                var acc = await _uow.AccountRepository.DeActivate(account, reason);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This function will activate the account
        /// </summary>
        /// <param name="account"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPost("Activate"), Authorize]
        public async Task<IActionResult> Activate(AccountDTO account)
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
            try
            {
                var acc = await _uow.AccountRepository.Activate(account);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This function will check if the account already exists
        /// </summary>
        /// <param name="account"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPost("IsExisting"), Authorize]
        public async Task<IActionResult> IsExisting(AccountDTO account)
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
            return Ok(await _uow.AccountRepository.IsExisitng(account));
        }
        /// <summary>
        /// This function will assign the identity to the account
        /// </summary>
        /// <param name="idenAccount"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPost("AssingIdentity"), Authorize]
        public async Task<IActionResult> AssignIdentity(IdenAccountDTO idenAccount)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "AssignIdentity"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var accountdto = await _uow.AccountRepository.GetById(idenAccount.Account.AccID);
            var identity = await _uow.IdentityRepository.GetById(idenAccount.Identity.IdenId);
            if (accountdto is null || identity is null)
                return NotFound();
            try
            {
                await _uow.AccountRepository.AssignAccount2Identity(idenAccount);
                await _uow.SaveChangesAsync();
                return Ok(accountdto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This function will release the identity from the account
        /// </summary>
        /// <param name="idenAccount"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPost("ReleaseIdentity"), Authorize]
        public async Task<IActionResult> ReleaseIdentity(IdenAccountDTO idenAccount)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            request = new()
            {
                AdminId = Int32.Parse(userIdClaim),
                Site = site,
                Action = "ReleaseIdentity"
            };
            var hasAdminAcces = await _uow.AdminRepository.HasAdminAccess(request);
            if (!hasAdminAcces)
                return Unauthorized();
            var accountdto = await _uow.AccountRepository.GetById(idenAccount.Account.AccID);
            var identity = await _uow.IdentityRepository.GetById(idenAccount.Identity.IdenId);
            if (accountdto is null || identity is null)
                return NotFound();
            try
            {
                await _uow.AccountRepository.ReleaseAccountFromIdentity(idenAccount);
                await _uow.SaveChangesAsync();
                return Ok(idenAccount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This function will update the account
        /// </summary>
        /// <param name="account"></param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpPut, Authorize]
        public async Task<IActionResult> Update(AccountDTO account)
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
            try
            {
                var acc = await _uow.AccountRepository.Update(account);
                await _uow.SaveChangesAsync();
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// This function will return all the free accounts
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="UnauthorizedResult"/> if the user is not authorized, or <see cref="OkObjectResult"/></returns>
        [HttpGet("ListAllFreeAccounts"), Authorize]
        public async Task<IActionResult> ListAllFreeAccounts()
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
            return Ok(await _uow.AccountRepository.ListAllFreeAccounts());
        }
    }
}
