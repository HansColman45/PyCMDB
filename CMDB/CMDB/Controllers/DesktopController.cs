using System;
using System.Linq;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using CMDB.Domain.Entities;
using CMDB.Services;
using System.Threading.Tasks;
using CMDB.Util;

namespace CMDB.Controllers
{
    public class DesktopController : CMDBController
    {
        private new readonly DevicesService service;
        public DesktopController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
            SitePart = "Desktop";
            Table = "desktop";
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            ViewData["Title"] = "Desktop overview";
            await BuildMenu();
            var Desktops = await service.ListAll(SitePart);
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            ViewData["AssignIdentityAccess"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            ViewData["actionUrl"] = @"\Desktop\Search";
            return View(Desktops);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            await BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["Title"] = "Desktop overview";
                var Desktops = await service.ListAll(SitePart, search);
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
                ViewData["AssignIdentityAccess"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
                ViewData["actionUrl"] = @"\Desktop\Search";
                return View(Desktops);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", SitePart);
            ViewData["Title"] = "Create Desktop";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            await BuildMenu();
            Desktop desktop = new();
            ViewBag.Types = service.ListAssetTypes(SitePart);
            ViewBag.Rams = service.ListRams();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    desktop.AssetTag = values["AssetTag"];
                    desktop.SerialNumber = values["SerialNumber"];
                    desktop.RAM = values["RAM"];
                    int Type = Convert.ToInt32(values["Type"]);
                    var AssetType = service.ListAssetTypeById(Type);
                    desktop.Type = AssetType;
                    desktop.Category = AssetType.Category;
                    desktop.MAC = values["MAC"];
                    if (service.IsDeviceExisting(desktop))
                        ModelState.AddModelError("", "Asset already exist");
                    if (ModelState.IsValid)
                    {
                        await service.CreateNewDevice(desktop, Table);
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
            return View(desktop);
        }
        public async Task<IActionResult> Edit(IFormCollection values, string id)
        {
            log.Debug("Using Edit in {0}", SitePart);
            ViewData["Title"] = "Edit Desktop";
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            await BuildMenu();
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var desktops = await service.ListDekstopByID(id);
            Desktop desktop = desktops.FirstOrDefault();
            if (desktop == null)
                return NotFound();
            ViewBag.AssetTypes = service.ListAssetTypes(SitePart);
            ViewBag.Rams = service.ListRams();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newSerialNumber = values["SerialNumber"];
                    string newRam = values["RAM"];
                    int Type = Convert.ToInt32(values["Type.TypeID"]);
                    var newAssetType = service.ListAssetTypeById(Type);
                    string newMAC = values["MAC"];
                    if (ModelState.IsValid)
                    {
                        await service.UpdateDesktop(desktop, newRam, newMAC, newAssetType, newSerialNumber, Table);
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
            return View(desktop);
        }
        public async Task<IActionResult> Details(string id)
        {
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = "Desktop details";
            await BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["IdentityOverview"] = service.HasAdminAccess(service.Admin, SitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "ReleaseIdentity");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var desktops = await service.ListDekstopByID(id);
            Desktop desktop = desktops.FirstOrDefault();
            if (desktop == null)
                return NotFound();
            service.GetLogs(Table, desktop.AssetTag, desktop);
            service.GetAssignedIdentity(desktop);
            return View(desktop);
        }
        public async Task<IActionResult> Delete(IFormCollection values, string id)
        {
            log.Debug("Using Delete in {0}", SitePart);
            if (String.IsNullOrEmpty(id))
                return NotFound();
            ViewData["Title"] = "Delete Desktop";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["backUrl"] = "Desktop";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            var desktops = await service.ListDekstopByID(id);
            Desktop desktop = desktops.FirstOrDefault();
            if (desktop == null)
                return NotFound();
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        if (desktop.IdentityId > 1)
                        {
                            PDFGenerator PDFGenerator = new()
                            {
                                ITEmployee = service.Admin.Account.UserID,
                                Singer = desktop.Identity.Name,
                                UserID = desktop.Identity.UserID,
                                FirstName = desktop.Identity.FirstName,
                                LastName = desktop.Identity.LastName,
                                Language = desktop.Identity.Language.Code,
                                Receiver = desktop.Identity.Name
                            };
                            PDFGenerator.SetAssetInfo(desktop);
                            string pdfFile = PDFGenerator.GeneratePDF(_env);
                            await service.LogPdfFile("identity", desktop.Identity.IdenId, pdfFile);
                            await service.LogPdfFile(Table, desktop.AssetTag, pdfFile);
                            await service.ReleaseIdenity(desktop, desktop.Identity, Table);
                        }
                        await service.Deactivate(desktop, values["reason"], Table);
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
            return View(desktop);
        }
        public async Task<IActionResult> Activate(string id)
        {
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Laptop";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            await BuildMenu();
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var desktops = await service.ListDekstopByID(id);
            Desktop desktop = desktops.FirstOrDefault();
            if (desktop == null)
                return NotFound();
            if (service.HasAdminAccess(service.Admin, SitePart, "Activate"))
            {
                await service.Activate(desktop, Table);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> AssignIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Assign identity in {0}", Table);
            ViewData["Title"] = "Assign identity to Desktop";
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
            ViewData["backUrl"] = "Desktop";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var desktops = await service.ListDekstopByID(id);
            Desktop desktop = desktops.FirstOrDefault();
            if (desktop == null)
                return NotFound();
            ViewBag.Identities = service.ListFreeIdentities();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    if (!service.IsDeviceFree(desktop))
                        ModelState.AddModelError("", "Desktop can not be assigned to another user");
                    Identity identity = service.GetAssignedIdentity(Int32.Parse(values["Identity"]));
                    if (ModelState.IsValid)
                    {
                        await service.AssignIdentity2Device(identity, desktop, Table);
                        return RedirectToAction("AssignForm", "Desktop", new { id });
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(desktop);
        }
        public async Task<IActionResult> AssignForm(IFormCollection values, string id)
        {
            log.Debug("Using Assign form in {0}", Table);
            ViewData["Title"] = "Assign form";
            ViewData["backUrl"] = "Desktop";
            ViewData["Action"] = "AssignForm";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var desktops = await service.ListDekstopByID(id);
            Desktop desktop = desktops.FirstOrDefault();
            if (desktop == null)
                return NotFound();
            service.GetAssignedIdentity(desktop);
            ViewData["Name"] = desktop.Identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    PDFGenerator PDFGenerator = new()
                    {
                        ITEmployee = ITPerson,
                        Singer = Employee,
                        UserID = desktop.Identity.UserID,
                        FirstName = desktop.Identity.FirstName,
                        LastName = desktop.Identity.LastName,
                        Language = desktop.Identity.Language.Code,
                        Receiver = desktop.Identity.Name
                    };
                    PDFGenerator.SetAssetInfo(desktop);
                    string pdfFile = PDFGenerator.GeneratePDF(_env);
                    await service.LogPdfFile("identity", desktop.Identity.IdenId, pdfFile);
                    await service.LogPdfFile(Table, desktop.AssetTag, pdfFile);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(desktop);
        }
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Release identity in {0}", Table);
            ViewData["Title"] = "Release identity from Desktop";
            ViewData["ReleaseIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "ReleaseIdentity");
            ViewData["backUrl"] = "Desktop";
            ViewData["Action"] = "ReleaseIdentity";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var desktops = await service.ListDekstopByID(id);
            Desktop desktop = desktops.FirstOrDefault();
            if (desktop == null)
                return NotFound();
            service.GetAssignedIdentity(desktop);
            Identity identity = desktop.Identity;
            ViewData["Name"] = identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    await service.ReleaseIdenity(desktop, desktop.Identity, Table);
                    PDFGenerator PDFGenerator = new()
                    {
                        ITEmployee = ITPerson,
                        Singer = Employee,
                        UserID = identity.UserID,
                        FirstName = identity.FirstName,
                        LastName = identity.LastName,
                        Language = identity.Language.Code,
                        Receiver = identity.Name,
                        Type = "Release"
                    };
                    PDFGenerator.SetAssetInfo(desktop);
                    string pdfFile = PDFGenerator.GeneratePDF(_env);
                    await service.LogPdfFile("identity", desktop.Identity.IdenId, pdfFile);
                    await service.LogPdfFile(Table, desktop.AssetTag, pdfFile);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(desktop);
        }
    }
}
