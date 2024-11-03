using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using CMDB.Domain.Entities;
using CMDB.Services;
using System.Threading.Tasks;
using CMDB.Util;
using Identity = CMDB.Domain.Entities.Identity;
using QuestPDF.Fluent;
using CMDB.API.Models;

namespace CMDB.Controllers
{
    public class TokenController : CMDBController
    {
        private new readonly DevicesService service;
        private readonly PDFService _PDFservice;
        public TokenController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            SitePart = "Token";
            Table = "token";
            _PDFservice = new();
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            ViewData["Title"] = "Token overview";
            await BuildMenu();
            var Desktops = await service.ListAll("Token");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["AssignIdentityAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["actionUrl"] = @"\Token\Search";
            ViewData["Controller"] = @"\Token\Create";
            return View(Desktops);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                ViewData["Title"] = "Token overview";
                await BuildMenu();
                var Desktops = await service.ListAll(SitePart, search);
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["AssignIdentityAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
                ViewData["actionUrl"] = @"\Token\Search";
                ViewData["Controller"] = @"\Token\Create";
                return View(Desktops);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> Delete(IFormCollection values, string id)
        {
            log.Debug("Using Delete in {0}", SitePart);
            if (id == null)
                return NotFound();
            ViewData["Title"] = "Delete token";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["backUrl"] = "Admin";
            ViewData["Controller"] = @$"\Token\Delete\{id}";
            var token = await service.GetDeviceById(SitePart, id);
            if (token == null)
                return NotFound();
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        if (token.Identity.IdenId > 1)
                        {
                            var admin = await service.Admin();
                            await _PDFservice.SetUserinfo(
                                UserId: token.Identity.UserID,
                                ITEmployee: admin.Account.UserID,
                                Singer: token.Identity.Name,
                                FirstName: token.Identity.FirstName,
                                LastName: token.Identity.LastName,
                                Language: token.Identity.Language.Code,
                                Receiver: token.Identity.Name,
                                type: "Release");
                            await _PDFservice.SetDeviceInfo(token);
                            await _PDFservice.GenratPDFFile(Table, token.AssetTag);
                            await _PDFservice.GenratPDFFile("identity", token.Identity.IdenId);
                            await service.ReleaseIdenity(token); 
                        }
                        await service.Deactivate(token, values["reason"]);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(token);
        }
        public async Task<IActionResult> Activate(string id)
        {
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate token";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            if (id == null)
                return NotFound();
            var token = await service.GetDeviceById(SitePart, id);
            if (token == null)
                return NotFound();
            await BuildMenu();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.Activate(token);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Details(string id)
        {
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var token = await service.GetDeviceById(SitePart, id);
            if (token == null)
                return NotFound();
            log.Debug($"Using details in {Table}");
            ViewData["Title"] = "Token details";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["IdentityOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            ViewData["Controller"] = @"\Token\Create";
            return View(token);
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug($"Using Create in {SitePart}");
            ViewData["Title"] = "Create token";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            await BuildMenu();
            DeviceDTO token= new();
            ViewBag.Types = await service.ListAssetTypes(SitePart);
            ViewData["Controller"] = @"\Token\Create";
            ViewData["backUrl"] = "Token";
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    token.AssetTag = values["AssetTag"];
                    token.SerialNumber = values["SerialNumber"];
                    int Type = Convert.ToInt32(values["AssetType"]);
                    var AssetType = await  service.GetAssetTypeById(Type);
                    var category = await service.GetAsstCategoryByCategory("Token");
                    token.AssetType = AssetType;
                    token.Category = category;
                    if (await service.IsDeviceExisting(token))
                        ModelState.AddModelError("", "Asset already exist");
                    if (ModelState.IsValid)
                    {
                        await service.CreateNewDevice(token);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(token);
        }
        public async Task<IActionResult> Edit(IFormCollection values, string id)
        {
            log.Debug("Using Edit in {0}", SitePart);
            ViewData["Title"] = "Edit token";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["backUrl"] = "Token";
            ViewData["Controller"] = @$"\Token\Edit\{id}";
            await BuildMenu();
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var token = await service.GetDeviceById(SitePart, id);
            if (token == null)
                return NotFound();
            ViewBag.Types = await service.ListAssetTypes(SitePart);
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newSerialNumber = values["SerialNumber"];
                    int Type = Convert.ToInt32(values["AssetType.TypeID"]);
                    var newAssetType = await service.GetAssetTypeById(Type);
                    if (ModelState.IsValid)
                    {
                        await service.UpdateDevice(token, newSerialNumber, newAssetType);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch(Exception ex)
                {
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(token);
        }
        public async Task<IActionResult> AssignIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Assign identity in {0}", Table);
            ViewData["Title"] = "Assign identity to Token";
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["backUrl"] = "Token";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var token = await service.GetDeviceById(SitePart, id);
            if (token == null)
                return NotFound();
            ViewBag.Identities = await service.ListFreeIdentities();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    if (!service.IsDeviceFree(token))
                        ModelState.AddModelError("", "Desktop can not be assigned to another user");
                    var identity = await service.GetAssignedIdentity(Int32.Parse(values["Identity"]));
                    if (ModelState.IsValid)
                    {
                        await service.AssignIdentity2Device(identity, token);
                        return RedirectToAction("AssignForm", "Token", new { id });
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(token);
        }
        public async Task<IActionResult> AssignForm(IFormCollection values, string id)
        {
            log.Debug("Using Assign form in {0}", Table);
            ViewData["Title"] = "Assign form";
            ViewData["backUrl"] = "Token";
            ViewData["Action"] = "AssignForm";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var token = await service.GetDeviceById(SitePart, id);
            if (token == null)
                return NotFound();
            ViewData["Name"] = token.Identity.Name; 
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid) {
                    await _PDFservice.SetUserinfo(
                        UserId: token.Identity.UserID,
                        ITEmployee: admin.Account.UserID,
                        Singer: token.Identity.Name,
                        FirstName: token.Identity.FirstName,
                        LastName: token.Identity.LastName,
                        Language: token.Identity.Language.Code,
                        Receiver: token.Identity.Name);
                    await _PDFservice.SetDeviceInfo(token);
                    await _PDFservice.GenratPDFFile(Table, token.AssetTag);
                    await _PDFservice.GenratPDFFile("identity", token.Identity.IdenId);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(token);
        }
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Release identity in {0}", Table);
            ViewData["Title"] = "Release identity from Token";
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["backUrl"] = "Token";
            ViewData["Action"] = "ReleaseIdentity";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var token = await service.GetDeviceById(SitePart, id);
            if (token == null)
                return NotFound();
            ViewData["Name"] = token.Identity.Name;
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                var identity = token.Identity;
                if (ModelState.IsValid) {
                    await _PDFservice.SetUserinfo(
                        UserId: token.Identity.UserID,
                        ITEmployee: admin.Account.UserID,
                        Singer: token.Identity.Name,
                        FirstName: token.Identity.FirstName,
                        LastName: token.Identity.LastName,
                        Language: token.Identity.Language.Code,
                        Receiver: token.Identity.Name,
                        type: "Release");
                    await _PDFservice.SetDeviceInfo(token);
                    await _PDFservice.GenratPDFFile(Table, token.AssetTag);
                    await _PDFservice.GenratPDFFile("identity", token.Identity.IdenId);
                    await service.ReleaseIdenity(token);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(token);
        }
    }
}
