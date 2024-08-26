using CMDB.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.Second;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;   
        }
        [HttpGet("FirstLevel")]
        [AllowAnonymous]
        public async Task<ActionResult> GetFirstLevelMenu()
        {
            return Ok(await _menuService.ListFirstMenuLevel());
        }
        [HttpGet]
        [Route("SecondLevel/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetSecondLevel(int id)
        {
            return Ok(await _menuService.ListSecondMenuLevel(id));
        }
        [HttpGet()]
        [Route("PersonalMenu/{menuId:int}")]
        [Authorize]
        public async Task<IActionResult> GetPersonalMenu(int menuId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            int level = Int32.Parse(User.Claims.First(x => x.Type == ClaimTypes.Name).Value);
            return Ok(await _menuService.ListPersonalMenu(level,menuId));
        }
    }
}
