using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
    public class DockingController : CMDBController
    {
        private new readonly DevicesService service;

        public DockingController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
            SitePart = "Docking station";
            Table = "docking";
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
            ViewData["actionUrl"] = @"\Docking\Search";
            return View(Desktops);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            if (!String.IsNullOrEmpty(search))
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
                ViewData["actionUrl"] = @"\Docking\Search";
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
            ViewData["Title"] = "Delete docking station";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["backUrl"] = "Docking";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            var dockings = await service.ListDockingByID(id);
            Docking docking = dockings.FirstOrDefault();
            if (docking == null)
                return NotFound();
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        if (docking.IdentityId > 1)
                        {
                            PDFGenerator PDFGenerator = new()
                            {
                                ITEmployee = service.Admin.Account.UserID,
                                Singer = docking.Identity.Name,
                                UserID = docking.Identity.UserID,
                                FirstName = docking.Identity.FirstName,
                                LastName = docking.Identity.LastName,
                                Language = docking.Identity.Language.Code,
                                Receiver = docking.Identity.Name
                            };
                            PDFGenerator.SetAssetInfo(docking);
                            string pdfFile = PDFGenerator.GeneratePath(_env);
                            PDFGenerator.GeneratePdf(pdfFile);
                            await service.LogPdfFile("identity", docking.Identity.IdenId, pdfFile);
                            await service.LogPdfFile(Table, docking.AssetTag, pdfFile);
                            await service.ReleaseIdenity(docking, docking.Identity, Table);
                        }
                        await service.Deactivate(docking, values["reason"], Table);
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
            ViewData["Title"] = "Activate docking station";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var dockings = await service.ListDockingByID(id);
            Docking docking = dockings.FirstOrDefault();
            if (docking == null)
                return NotFound();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.Activate(docking, Table);
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
            var dockings = await service.ListDockingByID(id);
            if (dockings == null)
                return NotFound();
            Docking docking = dockings.FirstOrDefault();
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = "Docking station details";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["IdentityOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            service.GetLogs(Table, docking.AssetTag, docking);
            service.GetAssignedIdentity(docking);
            return View(docking);
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug($"Using Create in {SitePart}");
            ViewData["Title"] = "Create docking station";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            await BuildMenu();
            Docking docking = new();
            ViewBag.Types = service.ListAssetTypes(SitePart);
            ViewData["backUrl"] = "Docking";
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    docking.AssetTag = values["AssetTag"];
                    docking.SerialNumber = values["SerialNumber"];
                    int Type = Convert.ToInt32(values["Type"]);
                    var AssetType = service.ListAssetTypeById(Type);
                    docking.Type = AssetType;
                    docking.Category = AssetType.Category;
                    if (service.IsDeviceExisting(docking))
                        ModelState.AddModelError("", "Asset already exist");
                    if (ModelState.IsValid)
                    {
                        await service.CreateNewDevice(docking, Table);
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
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var dockings = await service.ListDockingByID(id);
            if (dockings == null)
                return NotFound();
            await BuildMenu();
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["Title"] = "Edit docking station";
            Docking docking = dockings.FirstOrDefault();
            ViewBag.Types = service.ListAssetTypes(SitePart);
            ViewData["backUrl"] = "Docking";
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string newSerial = values["SerialNumber"];
                int Type = Convert.ToInt32(values["Type.TypeID"]);
                var AssetType = service.ListAssetTypeById(Type);
                if (ModelState.IsValid)
                {
                    await service.UpdateDocking(docking, newSerial, AssetType, Table);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(docking);
        }
        public async Task<IActionResult> AssignIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Assign identity in {0}", Table);
            ViewData["Title"] = "Assign identity to docking";
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["backUrl"] = "Laptop";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var dockings = await service.ListDockingByID(id);
            Docking docking = dockings.FirstOrDefault();
            if (docking == null)
                return NotFound();
            ViewBag.Identities = service.ListFreeIdentities();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    if (!service.IsDeviceFree(docking))
                        ModelState.AddModelError("", "Docking station can not be assigned to another user");
                    Identity identity = service.GetAssignedIdentity(Int32.Parse(values["Identity"]));
                    if (ModelState.IsValid)
                    {
                        await service.AssignIdentity2Device(identity, docking, Table);
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
            ViewData["Title"] = "Assign form";
            ViewData["backUrl"] = "Docking";
            ViewData["Action"] = "AssignForm";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var dockings = await service.ListDockingByID(id);
            Docking docking = dockings.FirstOrDefault();
            if (docking == null)
                return NotFound();
            service.GetAssignedIdentity(docking);
            ViewData["Name"] = docking.Identity.Name;
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
                        UserID = docking.Identity.UserID,
                        FirstName = docking.Identity.FirstName,
                        LastName = docking.Identity.LastName,
                        Language = docking.Identity.Language.Code,
                        Receiver = docking.Identity.Name
                    };
                    PDFGenerator.SetAssetInfo(docking);
                    string pdfFile = PDFGenerator.GeneratePath(_env);
                    PDFGenerator.GeneratePdf(pdfFile);
                    await service.LogPdfFile("identity", docking.Identity.IdenId, pdfFile);
                    await service.LogPdfFile(Table, docking.AssetTag, pdfFile);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(docking);
        }
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Release identity in {0}", Table);
            ViewData["Title"] = "Release identity from Docking";
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["backUrl"] = "Docking";
            ViewData["Action"] = "ReleaseIdentity";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var dockings = await service.ListDockingByID(id);
            Docking docking = dockings.FirstOrDefault();
            if (docking == null)
                return NotFound();
            service.GetAssignedIdentity(docking);
            ViewData["Name"] = docking.Identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                Identity identity = docking.Identity;
                if (ModelState.IsValid)
                {
                    await service.ReleaseIdenity(docking, identity, Table);
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
                    PDFGenerator.SetAssetInfo(docking);
                    string pdfFile = PDFGenerator.GeneratePath(_env);
                    PDFGenerator.GeneratePdf(pdfFile);
                    await service.LogPdfFile("identity", docking.Identity.IdenId, pdfFile);
                    await service.LogPdfFile(Table, docking.AssetTag, pdfFile);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(docking);
        }
    }
}
