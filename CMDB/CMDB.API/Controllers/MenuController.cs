using CMDB.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Menu controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private MenuController()
        {
        }
        private readonly IUnitOfWork _uow;
        /// <summary>
        /// Menu controller constructor
        /// </summary>
        /// <param name="uow"></param>
        public MenuController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        /// <summary>
        /// Retrieves all menu items associated with the current user.
        /// </summary>
        /// <remarks>This method requires the caller to be authenticated and authorized.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of menu items if the user is authorized;  otherwise,
        /// an unauthorized response.</returns>
        [HttpGet("GetAll"), Authorize]
        public async Task<IActionResult> GetAll()
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var accounts = await _uow.MenuRepository.GetAll();
            return Ok(accounts);
        }
        /// <summary>
        /// Get first level menu
        /// </summary>
        /// <returns></returns>
        [HttpGet("FirstLevel"), Authorize]
        public async Task<ActionResult> GetFirstLevelMenu()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            return Ok(await _uow.MenuRepository.GetFirstLevel());
        }
        /// <summary>
        /// Get second level menu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("SecondLevel/{id:int}"), Authorize]
        public async Task<ActionResult> GetSecondLevel(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            return Ok(await _uow.MenuRepository.GetSecondLevel(id));
        }
        /// <summary>
        /// Get third level menu and personal level
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpGet("PersonalMenu/{menuId:int}"), Authorize]
        public async Task<IActionResult> GetPersonalMenu(int menuId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            int level = Int32.Parse(User.Claims.First(x => x.Type == ClaimTypes.Name).Value);
            return Ok(await _uow.MenuRepository.GetPestonalMenu(menuId, level));
        }
        /// <summary>
        /// Retrieves a menu item by its unique identifier.
        /// </summary>
        /// <remarks>This method requires the caller to be authenticated and authorized. The user's
        /// identity is determined from the claims provided in the HTTP context.</remarks>
        /// <param name="id">The unique identifier of the menu item to retrieve. Must be a positive integer.</param>
        /// <returns>An <see cref="IActionResult"/> containing the menu item if found, or an appropriate HTTP response: 
        /// <list type="bullet"> <item><description><see langword="Unauthorized"/> if the user is not
        /// authenticated.</description></item> <item><description><see langword="NotFound"/> if no menu item exists
        /// with the specified identifier.</description></item> <item><description><see langword="Ok"/> with the menu
        /// item if retrieval is successful.</description></item> </list></returns>
        [HttpGet("{id:int}"), Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var menu = await _uow.MenuRepository.GetById(id);
            if (menu == null)
                return NotFound();
            return Ok(menu);
        }
    }
}
