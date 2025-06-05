using CMDB.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Controller for Language
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private LanguageController()
        {
        }
        private readonly IUnitOfWork _uow;
        /// <summary>
        /// Constructor for LanguageController
        /// </summary>
        /// <param name="uow"></param>
        public LanguageController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        /// <summary>
        /// This will list all Languages
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// This will return a list of all Languages matching the code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
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
