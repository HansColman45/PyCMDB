using CMDB.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public LanguageController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet("GetAll"), Authorize]
        public async Task<IActionResult> GetAll()
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var accounts = await _uow.LanguageRepository.GetAll();
            return Ok(accounts);
        }
        [HttpGet("{code:alpha}"), Authorize]
        public async Task<IActionResult> GetByCode(string code) 
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            return Ok(await _uow.LanguageRepository.GetByCode(code));
        }
    }
}
