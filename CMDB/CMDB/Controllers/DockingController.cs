using CMDB.API.Models;
using CMDB.Infrastructure;
using CMDB.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    public class DockingController : CMDBController
    {
        private readonly DevicesService service;
        private readonly PDFService _PDFservice;
        public DockingController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            SitePart = "Docking station";
            Table = "docking";
            _PDFservice = new ();
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            ViewData["Title"] = "Docking station overview";
            await BuildMenu();
            var Desktops = await service.ListAll(SitePart);
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["AssignIdentityAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["AssignKeyAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignKensington");
            ViewData["actionUrl"] = @"\Docking\Search";
            ViewData["Controller"] = @"\Docking\Create";
            return View(Desktops);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            if (!string.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                ViewData["Title"] = "Docking station overview";
                await BuildMenu();
                var Desktops = await service.ListAll(SitePart, search);
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["AssignIdentityAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
                ViewData["AssignKeyAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignKensington");
                ViewData["actionUrl"] = @"\Docking\Search";
                ViewData["Controller"] = @"\Docking\Create";
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
            var docking = await service.GetDeviceById(SitePart, id);
            if (docking == null)
                return NotFound();
            ViewData["Title"] = "Delete docking station";
            ViewData["Controller"] = @$"\Docking\Delete\{id}";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["backUrl"] = "Docking";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        if (docking.Identity.IdenId > 1)
                        {
                            var admin = await service.Admin();
                            await _PDFservice.SetUserinfo(
                                UserId: docking.Identity.UserID,
                                ITEmployee: admin.Account.UserID,
                                Singer: docking.Identity.Name,
                                FirstName: docking.Identity.FirstName,
                                LastName: docking.Identity.LastName,
                                Language: docking.Identity.Language.Code,
                                Receiver: docking.Identity.Name,
                                type: "Release");
                            await _PDFservice.SetDeviceInfo(docking);
                            if (docking.Kensington != null)
                            {
                                await _PDFservice.SetKeyInfo(docking.Kensington);
                                await service.ReleaseKensington(docking);
                            }
                            await _PDFservice.GenratePDFFile(Table, docking.AssetTag);
                            await _PDFservice.GenratePDFFile("identity", docking.Identity.IdenId);
                            await service.ReleaseIdenity(docking);
                        }
                        await service.Deactivate(docking, values["reason"]);
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
            return View(docking);
        }
        public async Task<IActionResult> Activate(string id)
        {
            log.Debug("Using Activate in {0}", Table);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var docking = await service.GetDeviceById(SitePart, id);
            if (docking == null)
                return NotFound();
            ViewData["Title"] = "Activate docking station";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();            
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.Activate(docking);
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
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var docking = await service.GetDeviceById(SitePart, id);
            if (docking == null)
                return NotFound();
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = "Docking station details";
            ViewData["Controller"] = @"\Docking\Create";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["IdentityOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["KeyOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "KeyOverview");
            ViewData["ReleaseKensington"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseKensington");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            return View(docking);
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug($"Using Create in {SitePart}");
            ViewData["Title"] = "Create docking station";
            ViewData["Controller"] = @"\Docking\Create";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            await BuildMenu();
            DeviceDTO docking = new();
            ViewBag.Types = await service.ListAssetTypes(SitePart);
            ViewData["backUrl"] = "Docking";
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    docking.AssetTag = values["AssetTag"];
                    docking.SerialNumber = values["SerialNumber"];
                    int Type = Convert.ToInt32(values["AssetType"]);
                    var AssetType = await service.GetAssetTypeById(Type);
                    var category = await service.GetAsstCategoryByCategory("Docking station");
                    docking.AssetType = AssetType;
                    docking.Category = category;
                    if (await service.IsDeviceExisting(docking))
                        ModelState.AddModelError("", "Asset already exist");
                    if (ModelState.IsValid)
                    {
                        await service.CreateNewDevice(docking);
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
            return View(docking);
        }
        public async Task<IActionResult> Edit(string id, IFormCollection values)
        {
            log.Debug("Using Edit in {0}", SitePart);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var docking = await service.GetDeviceById(SitePart, id);
            if (docking == null)
                return NotFound();
            await BuildMenu();
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["Title"] = "Edit docking station";
            ViewBag.Types = await service.ListAssetTypes(SitePart);
            ViewData["backUrl"] = "Docking";
            string FormSubmit = values["form-submitted"];
            ViewData["Controller"] = @$"\Docking\Edit\{id}";
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                string newSerial = values["SerialNumber"];
                int Type = Convert.ToInt32(values["AssetType.TypeID"]);
                var AssetType = await service.GetAssetTypeById(Type);
                if (ModelState.IsValid)
                {
                    await service.UpdateDevice(docking, newSerial, AssetType);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(docking);
        }
        public async Task<IActionResult> AssignIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Assign identity in {0}", Table);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var docking = await service.GetDeviceById(SitePart, id);
            if (docking == null)
                return NotFound();
            ViewData["Title"] = "Assign identity to docking";
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["backUrl"] = "Laptop";
            ViewData["Controller"] = @$"\Docking\AssignIdentity\{id}";
            await BuildMenu();
            ViewBag.Identities = await service.ListFreeIdentities(Table);
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    if (!service.IsDeviceFree(docking))
                        ModelState.AddModelError("", "Docking station can not be assigned to another user");
                    var identity = await service.GetAssignedIdentity(Int32.Parse(values["Identity"]));
                    if (ModelState.IsValid)
                    {
                        await service.AssignIdentity2Device(identity, docking);
                        return RedirectToAction("AssignForm", "Docking", new { id });
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(docking);
        }
        public async Task<IActionResult> AssignForm(IFormCollection values, string id)
        {
            log.Debug("Using Assign form in {0}", Table);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var docking = await service.GetDeviceById(SitePart, id);
            if (docking == null)
                return NotFound();
            ViewData["Title"] = "Assign form";
            ViewData["backUrl"] = "Docking";
            ViewData["Action"] = "AssignForm";
            await BuildMenu();
            ViewData["Name"] = docking.Identity.Name;
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    await _PDFservice.SetUserinfo(
                        UserId: docking.Identity.UserID,
                        ITEmployee: admin.Account.UserID,
                        Singer: docking.Identity.Name,
                        FirstName: docking.Identity.FirstName,
                        LastName: docking.Identity.LastName,
                        Language: docking.Identity.Language.Code,
                        Receiver: docking.Identity.Name);
                    await _PDFservice.SetDeviceInfo(docking);
                    await _PDFservice.GenratePDFFile(Table, docking.AssetTag);
                    await _PDFservice.GenratePDFFile("identity", docking.Identity.IdenId);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(docking);
        }
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Release identity in {0}", Table);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var docking = await service.GetDeviceById(SitePart, id);
            if (docking == null)
                return NotFound();
            ViewData["Title"] = "Release identity from Docking";
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["backUrl"] = "Docking";
            ViewData["Action"] = "ReleaseIdentity";
            ViewData["Controller"] = @$"\Docking\ReleaseIdentity\{id}";
            await BuildMenu();
            ViewData["Name"] = docking.Identity.Name;
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                var identity = docking.Identity;
                if (ModelState.IsValid)
                {
                    await _PDFservice.SetUserinfo(
                        UserId: docking.Identity.UserID,
                        ITEmployee: admin.Account.UserID,
                        Singer: docking.Identity.Name,
                        FirstName: docking.Identity.FirstName,
                        LastName: docking.Identity.LastName,
                        Language: docking.Identity.Language.Code,
                        Receiver: docking.Identity.Name,
                        type: "Release");
                    await _PDFservice.SetDeviceInfo(docking);
                    await _PDFservice.GenratePDFFile(Table, docking.AssetTag);
                    await _PDFservice.GenratePDFFile("identity", docking.Identity.IdenId);
                    await service.ReleaseIdenity(docking);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(docking);
        }
    }
}
