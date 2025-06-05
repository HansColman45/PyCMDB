using CMDB.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CMDB.Domain.Requests;
using CMDB.API.Services;
using QuestPDF.Fluent;
using CMDB.API.Models;
using CMDB.API.Interfaces;

namespace CMDB.API.Controllers
{
    /// <summary>
    /// Controller for PDF generation
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PDFGeneratorController : ControllerBase
    {
        private PDFGeneratorController()
        {
        }

        private readonly IUnitOfWork _uow;
        private static readonly PDFGenerator PDFGenerator = new();
        private readonly IWebHostEnvironment _env;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uow"><see cref="IUnitOfWork"/></param>
        /// <param name="env"></param>
        public PDFGeneratorController(IUnitOfWork uow, IWebHostEnvironment env)
        {
            _uow = uow;
            _env = env;
        }
        /// <summary>
        /// Set user information for PDF generation
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost("AddUserInfo"), Authorize]
        public IActionResult AddUserInfo(PDFInformation info)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            PDFGenerator.SetPDFInfo(info.Language,info.Receiver,info.FirstName,info.LastName,info.UserID,info.Singer,info.ITEmployee,info.Type);
            return Ok();
        }
        /// <summary>
        /// Set device information for PDF generation
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [HttpPost("AddAssetInfo"), Authorize]
        public IActionResult AddAssetInfo(DeviceDTO device)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            PDFGenerator.SetAssetInfo(device);
            return Ok();
        }
        /// <summary>
        /// Set Mobile information for PDF generation
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost("AddMobileInfo"), Authorize]
        public IActionResult AddMobileInfo(MobileDTO mobile) 
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            PDFGenerator.SetMobileInfo(mobile);
            return Ok();
        }
        /// <summary>
        /// Set the account information for PDF generation
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Set the subscription information for PDF generation
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        [HttpPost("AddSubscriptionInfo"), Authorize]
        public IActionResult AddSubscriptionInfo(SubscriptionDTO subscription) 
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            PDFGenerator.SetSubscriptionInfo(subscription);
            return Ok();
        }
        /// <summary>
        /// Set the kensington information for PDF generation
        /// </summary>
        /// <param name="kensington"></param>
        /// <returns></returns>
        [HttpPost("AddKeyInfo"), Authorize]
        public IActionResult AddKeyInfo(KensingtonDTO kensington)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            if (userIdClaim == null)
                return Unauthorized();
            PDFGenerator.SetKensingtonInfo(kensington);
            return Ok();
        }
        /// <summary>
        /// Generate the PDF file and log it in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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
                case "subscription":
                    await _uow.SubscriptionRepository.LogPdfFile(pdfFile, id);
                    break;
                case "mobile":
                    await _uow.MobileRepository.LogPdfFile(pdfFile, id);
                    break;
                case "kensington":
                    await _uow.KensingtonRepository.LogPdfFile(pdfFile, id);
                    break;
                default:
                    throw new NotImplementedException($"The {entity} is not implemented");
            }
            await _uow.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Generate the PDF file and log it in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="assetTag"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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
