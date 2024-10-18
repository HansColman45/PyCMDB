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
    public class MonitorController : CMDBController
    {
        private new readonly DevicesService service;
        private readonly PDFService _PDFservice;
        public MonitorController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            SitePart = "Monitor";
            Table = "screen";
            _PDFservice = new();
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            ViewData["Title"] = "Monitor overview";
            await BuildMenu();
            var Desktops = await service.ListAll("Monitor");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["AssignIdentityAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["actionUrl"] = @"\Monitor\Search";
            return View(Desktops);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["Title"] = "Monitor overview";
                await BuildMenu();
                var Desktops = await service.ListAll(SitePart, search);
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["AssignIdentityAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["actionUrl"] = @"\Monitor\Search";
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
            ViewData["Title"] = "Delete Monitor";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["backUrl"] = "Admin";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            var monitor = await service.GetDeviceById(SitePart, id);
            if (monitor == null)
                return NotFound();
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        if (monitor.Identity.IdenId > 1)
                        {
                            var admin = await service.Admin();
                            await _PDFservice.SetUserinfo(
                                UserId: monitor.Identity.UserID, 
                                ITEmployee: admin.Account.UserID, 
                                Singer: monitor.Identity.Name,
                                FirstName:monitor.Identity.FirstName, 
                                LastName:monitor.Identity.LastName,
                                Language: monitor.Identity.Language.Code,
                                Receiver: monitor.Identity.Name,
                                type: "Release");
                            await _PDFservice.SetDeviceInfo(monitor);
                            await _PDFservice.GenratPDFFile(Table, monitor.AssetTag);
                            await _PDFservice.GenratPDFFile("identity", monitor.Identity.IdenId);
                            await service.ReleaseIdenity(monitor);
                        }
                        await service.Deactivate(monitor, values["reason"]);
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
            return View(monitor);
        }
        public async Task<IActionResult> Activate(string id)
        {
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Laptop";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var moniror = await service.GetDeviceById(SitePart, id);
            if (moniror == null)
                return NotFound();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.Activate(moniror);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug($"Using Create in {SitePart}");
            ViewData["Title"] = "Create monitor";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            await BuildMenu();
            ViewBag.Types = await service.ListAssetTypes(SitePart);
            ViewData["backUrl"] = "Desktop";
            DeviceDTO screen = new();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    screen.AssetTag = values["AssetTag"];
                    screen.SerialNumber = values["SerialNumber"];
                    int Type = Convert.ToInt32(values["AssetType"]);
                    var AssetType = await service.GetAssetTypeById(Type);
                    var category = await service.GetAsstCategoryByCategory("Monitor");
                    screen.AssetType = AssetType;
                    screen.Category = category;
                    if (await service.IsDeviceExisting(screen))
                        ModelState.AddModelError("", "Asset already exist");
                    if (ModelState.IsValid)
                    {
                        await service.CreateNewDevice(screen);
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
            return View(screen);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var screen = await service.GetDeviceById(SitePart, id);
            if (screen == null)
                return NotFound();
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = "Monitor details";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["IdentityOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            return View(screen);
        }
        public async Task<IActionResult> Edit(string id, IFormCollection values)
        {
            log.Debug("Using Edit in {0}", SitePart);
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var screen = await service.GetDeviceById(SitePart, id);
            if (screen == null)
                return NotFound();
            await BuildMenu();
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["Title"] = "Edit monitor";
            ViewBag.Types = await service.ListAssetTypes(SitePart);
            ViewData["backUrl"] = "Monitor";
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string newSerial = values["SerialNumber"];
                int Type = Convert.ToInt32(values["AssetType.TypeID"]);
                var AssetType = await service.GetAssetTypeById(Type);
                if (ModelState.IsValid)
                {
                    await service.UpdateDevice(screen, newSerial, AssetType);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(screen);
        }
        public async Task<IActionResult> AssignIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Assign identity in {0}", Table);
            ViewData["Title"] = "Assign identity to Monitor";
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["backUrl"] = "Monitor";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var moniror = await service.GetDeviceById(SitePart, id);
            if (moniror == null)
                return NotFound();
            ViewBag.Identities = await service.ListFreeIdentities();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {   
                    if (!service.IsDeviceFree(moniror))
                        ModelState.AddModelError("", "Monitor can not be assigned to another user");
                    var identity = await service.GetAssignedIdentity(Int32.Parse(values["Identity"]));
                    if (ModelState.IsValid)
                    {
                        await service.AssignIdentity2Device(identity, moniror);
                        return RedirectToAction("AssignForm", "Monitor", new { id });
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(moniror);
        }
        public async Task<IActionResult> AssignForm(IFormCollection values, string id)
        {
            log.Debug("Using Assign form in {0}", Table);
            ViewData["Title"] = "Assign form";
            ViewData["backUrl"] = "Monitor";
            ViewData["Action"] = "AssignForm";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var monitor = await service.GetDeviceById(SitePart, id);
            if (monitor == null)
                return NotFound();
            ViewData["Name"] = monitor.Identity.Name;
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid) {
                    await _PDFservice.SetUserinfo(
                        UserId: monitor.Identity.UserID,
                        ITEmployee: admin.Account.UserID,
                        Singer: monitor.Identity.Name,
                        FirstName: monitor.Identity.FirstName,
                        LastName: monitor.Identity.LastName,
                        Language: monitor.Identity.Language.Code,
                        Receiver: monitor.Identity.Name);
                    await _PDFservice.SetDeviceInfo(monitor);
                    await _PDFservice.GenratPDFFile(Table, monitor.AssetTag);
                    await _PDFservice.GenratPDFFile("identity", monitor.Identity.IdenId);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(monitor);
        }
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Release identity in {0}", Table);
            ViewData["Title"] = "Release identity from Monitor";
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["backUrl"] = "Monitor";
            ViewData["Action"] = "ReleaseIdentity";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var monitor = await service.GetDeviceById(SitePart, id);
            if (monitor == null)
                return NotFound();
            ViewData["Name"] = monitor.Identity.Name;
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                var identity = monitor.Identity;
                if (ModelState.IsValid) {
                    await _PDFservice.SetUserinfo(
                        UserId: monitor.Identity.UserID,
                        ITEmployee: admin.Account.UserID,
                        Singer: monitor.Identity.Name,
                        FirstName: monitor.Identity.FirstName,
                        LastName: monitor.Identity.LastName,
                        Language: monitor.Identity.Language.Code,
                        Receiver: monitor.Identity.Name,
                        type: "Release");
                    await _PDFservice.SetDeviceInfo(monitor);
                    await _PDFservice.GenratPDFFile(Table, monitor.AssetTag);
                    await _PDFservice.GenratPDFFile("identity", monitor.Identity.IdenId);
                    await service.ReleaseIdenity(monitor);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(monitor);
        }
    }
}
