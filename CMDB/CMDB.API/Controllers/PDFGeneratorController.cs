using CMDB.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CMDB.Domain.Requests;
using CMDB.API.Services;
using QuestPDF.Fluent;
using CMDB.API.Models;

namespace CMDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFGeneratorController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private static readonly PDFGenerator PDFGenerator = new();
        private readonly IWebHostEnvironment _env;
        public PDFGeneratorController(IUnitOfWork uow, IWebHostEnvironment env)
        {
            _uow = uow;
            _env = env;
        }
        [HttpPost("AddUserInfo"), Authorize]
        public IActionResult AddUserInfo(PDFInformation info)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            PDFGenerator.SetPDFInfo(info.Language,info.Receiver,info.FirstName,info.LastName,info.UserID,info.Singer,info.ITEmployee,info.Type);
            return Ok();
        }
        [HttpPost("AddAssetInfo"), Authorize]
        public IActionResult AddAssetInfo(DeviceDTO device)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            PDFGenerator.SetAssetInfo(device);
            return Ok();
        }
        [HttpPost("AddMobileInfo"), Authorize]
        public IActionResult AddMobileInfo(Mobile mobile) 
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            PDFGenerator.SetMobileInfo(mobile);
            return Ok();
        }
        [HttpPost("AddAccountInfo"), Authorize]
        public async Task<IActionResult> AddAccountInfo(IdenAccountDTO account)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            var idenacc = await _uow.IdenAccountRepository.GetIdenAccountById(account.Id);
            PDFGenerator.SetAccontInfo(idenacc);
            return Ok();
        }
        [HttpPost("AddSubscriptionInfo"), Authorize]
        public IActionResult AddSubscriptionInfo(Subscription subscription) 
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            PDFGenerator.SetSubscriptionInfo(subscription);
            return Ok();
        }
        [HttpGet("{entity:alpha}/{id:int}"), Authorize]
        public async Task<IActionResult> GenertatePDF(string entity, int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            string pdfFile = PDFGenerator.GeneratePath(_env);
            PDFGenerator.GeneratePdf(pdfFile);
            switch (entity)
            {
                case "identity":
                    await _uow.IdentityRepository.LogPdfFile(pdfFile,id);
                    break;
                case "account":
                    await _uow.AccountRepository.LogPdfFile(pdfFile, id);
                    break;
            }
            await _uow.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("{entity:alpha}/{assetTag}"),Authorize]
        public async Task<IActionResult> GeneratePDF(string entity, string assetTag)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            string pdfFile = PDFGenerator.GeneratePath(_env);
            PDFGenerator.GeneratePdf(pdfFile);
            switch (entity) 
            {
                case "monitor":
                case "screen":
                case "laptop":
                case "desktop":
                case "docking":
                case "docking station":
                case "token":
                    await _uow.DeviceRepository.LogPdfFile(entity, pdfFile, assetTag);
                    break;
                default:
                    throw new NotImplementedException($"The {entity} is not implemented");
            }
            await _uow.SaveChangesAsync();
            return Ok();
        }
    }
}
