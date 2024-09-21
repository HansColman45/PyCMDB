using CMDB.API.Services;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public LoggerController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpPost]
        [Authorize]
        [Route("LogPDFFileGenerated/{table}/{id}/{pdfFile}")]
        public async Task<IActionResult> LogPDFFileGenerated(string table,int id,string pdfFile)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            //await _logService.LogPdfFile(table, id, pdfFile);
            return Ok();
        }
        [HttpPost]
        [Authorize]
        [Route("LogAssetPDFFileGenerated/{table}/{assetTag}/{pdfFile}")]
        public async Task<IActionResult> LogPDFFileGenerated(string table, string assetTag, string pdfFile)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            //await _logService.LogPdfFile(table, assetTag, pdfFile);
            return Ok();
        }
    }
}
