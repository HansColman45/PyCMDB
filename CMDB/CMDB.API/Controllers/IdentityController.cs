using CMDB.API.Models;
using CMDB.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public IdentityController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Identity")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Read")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            return Ok(await _uow.IdentityRepository.GetAll());
        }
        [HttpGet]
        [Route("GetAll/{searchstr}")]
        [Authorize]
        public async Task<IActionResult> GetAll(string searchstr)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Identity")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Read")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            return Ok(await _uow.IdentityRepository.GetAll(searchstr));
        }
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Identity")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Read")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            return Ok(await _uow.IdentityRepository.GetById(id));
        }
        [HttpPost("AssignAccount")]
        [Authorize]
        public async Task<IActionResult> AssignAccount(IdenAccountDTO request)
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Identity")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("AssignAccount")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            var Iden = await _uow.IdentityRepository.GetById(request.Identity.IdenId);
            var account = await _uow.AccountRepository.GetById(request.Account.AccID);
            if (Iden is null || account is null)
                return NotFound();
            //ToDO
            return Ok();
        }
        [HttpGet("ListAllFreeIdentities"), Authorize]
        public async Task<IActionResult> ListAllFreeIdentities()
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var role = User.Claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains("Identity")).FirstOrDefault();
            var per = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier && x.Value.Contains("Read")).FirstOrDefault();
            if (role is null && per is null)
                return Unauthorized();
            return Ok(await _uow.IdentityRepository.ListAllFreeIdentities());
        }
    }
}
