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
using QuestPDF.Fluent;

namespace CMDB.Controllers
{
    public class MonitorController : CMDBController
    {
        private new readonly DevicesService service;
        public MonitorController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
            SitePart = "Monitor";
            Table = "screen";
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            ViewData["Title"] = "Monitor overview";
            await BuildMenu();
            var Desktops = await service.ListAll("Monitor");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            ViewData["AssignIdentityAccess"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
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
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
                ViewData["AssignIdentityAccess"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
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
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["backUrl"] = "Admin";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            var monitors = await service.ListScreensByID(id);
            Screen monitor = monitors.FirstOrDefault();
            if (monitor == null)
                return NotFound();
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        if (monitor.IdentityId > 1)
                        {
                            PDFGenerator PDFGenerator = new()
                            {
                                ITEmployee = service.Admin.Account.UserID,
                                Singer = monitor.Identity.Name,
                                UserID = monitor.Identity.UserID,
                                FirstName = monitor.Identity.FirstName,
                                LastName = monitor.Identity.LastName,
                                Language = monitor.Identity.Language.Code,
                                Receiver = monitor.Identity.Name
                            };
                            PDFGenerator.SetAssetInfo(monitor);
                            string pdfFile = PDFGenerator.GeneratePath(_env);
                            PDFGenerator.GeneratePdf(pdfFile);
                            await service.LogPdfFile("identity", monitor.Identity.IdenId, pdfFile);
                            await service.LogPdfFile(Table, monitor.AssetTag, pdfFile);
                            await service.ReleaseIdenity(monitor, monitor.Identity, Table);
                        }
                        await service.Deactivate(monitor, values["reason"], Table);
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
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var monitors = await service.ListScreensByID(id);
            Screen moniror = monitors.FirstOrDefault();
            if (moniror == null)
                return NotFound();
            if (service.HasAdminAccess(service.Admin, SitePart, "Activate"))
            {
                await service.Activate(moniror, Table);
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
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            await BuildMenu();
            ViewBag.Types = service.ListAssetTypes(SitePart);
            ViewData["backUrl"] = "Desktop";
            Screen screen = new();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    screen.AssetTag = values["AssetTag"];
                    screen.SerialNumber = values["SerialNumber"];
                    int Type = Convert.ToInt32(values["Type"]);
                    var AssetType = service.ListAssetTypeById(Type);
                    screen.Type = AssetType;
                    screen.Category = AssetType.Category;
                    if (service.IsDeviceExisting(screen))
                        ModelState.AddModelError("", "Asset already exist");
                    if (ModelState.IsValid)
                    {
                        await service.CreateNewDevice(screen, Table);
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
            var screens = await service.ListScreensByID(id);
            if (screens == null)
                return NotFound();
            Screen screen = screens.FirstOrDefault();
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = "Monitor details";
            await BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["IdentityOverview"] = service.HasAdminAccess(service.Admin, SitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "ReleaseIdentity");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            service.GetLogs(Table, screen.AssetTag, screen);
            service.GetAssignedIdentity(screen);
            return View(screen);
        }
        public async Task<IActionResult> Edit(string id, IFormCollection values)
        {
            log.Debug("Using Edit in {0}", SitePart);
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var screens = await service.ListScreensByID(id);
            if (screens == null)
                return NotFound();
            await BuildMenu();
            Screen screen = screens.FirstOrDefault();
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            ViewData["Title"] = "Edit monitor";
            ViewBag.Types = service.ListAssetTypes(SitePart);
            ViewData["backUrl"] = "Monitor";
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string newSerial = values["SerialNumber"];
                int Type = Convert.ToInt32(values["Type.TypeID"]);
                var AssetType = service.ListAssetTypeById(Type);
                if (ModelState.IsValid)
                {
                    await service.UpdateScreen(screen, newSerial, AssetType, Table);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(screen);
        }
        public async Task<IActionResult> AssignIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Assign identity in {0}", Table);
            ViewData["Title"] = "Assign identity to Monitor";
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
            ViewData["backUrl"] = "Monitor";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var monitors = await service.ListScreensByID(id);
            Screen moniror = monitors.FirstOrDefault();
            if (moniror == null)
                return NotFound();
            ViewBag.Identities = service.ListFreeIdentities();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {   
                    if (!service.IsDeviceFree(moniror))
                        ModelState.AddModelError("", "Monitor can not be assigned to another user");
                    Identity identity = service.GetAssignedIdentity(Int32.Parse(values["Identity"]));
                    if (ModelState.IsValid)
                    {
                        await service.AssignIdentity2Device(identity, moniror, Table);
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
            var monitors = await service.ListScreensByID(id);
            Screen monitor = monitors.FirstOrDefault();
            if (monitor == null)
                return NotFound();
            ViewData["Name"] = monitor.Identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            service.GetAssignedIdentity(monitor);
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid) {
                    PDFGenerator PDFGenerator = new()
                    {
                        ITEmployee = ITPerson,
                        Singer = Employee,
                        UserID = monitor.Identity.UserID,
                        FirstName = monitor.Identity.FirstName,
                        LastName = monitor.Identity.LastName,
                        Language = monitor.Identity.Language.Code,
                        Receiver = monitor.Identity.Name
                    };
                    PDFGenerator.SetAssetInfo(monitor);
                    string pdfFile = PDFGenerator.GeneratePath(_env);
                    await service.LogPdfFile("identity", monitor.Identity.IdenId, pdfFile);
                    await service.LogPdfFile(Table, monitor.AssetTag, pdfFile);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(monitor);
        }
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Release identity in {0}", Table);
            ViewData["Title"] = "Release identity from Monitor";
            ViewData["ReleaseIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "ReleaseIdentity");
            ViewData["backUrl"] = "Monitor";
            ViewData["Action"] = "ReleaseIdentity";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var monitors = await service.ListScreensByID(id);
            Screen monitor = monitors.FirstOrDefault();
            if (monitor == null)
                return NotFound();
            ViewData["Name"] = monitor.Identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                Identity identity = monitor.Identity;
                if (ModelState.IsValid) {
                    await service.ReleaseIdenity(monitor, identity, Table);
                    PDFGenerator PDFGenerator = new()
                    {
                        ITEmployee = ITPerson,
                        Singer = Employee,
                        UserID = identity.UserID,
                        FirstName = identity.FirstName,
                        LastName = identity.LastName,
                        Language = identity.Language.Code,
                        Receiver = identity.Name
                    };
                    PDFGenerator.SetAssetInfo(monitor);
                    string pdfFile = PDFGenerator.GeneratePath(_env);
                    PDFGenerator.GeneratePdf(pdfFile);
                    await service.LogPdfFile("identity", monitor.Identity.IdenId, pdfFile);
                    await service.LogPdfFile(Table, monitor.AssetTag, pdfFile);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(monitor);
        }
    }
}
