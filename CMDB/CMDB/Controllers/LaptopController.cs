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
    public class LaptopController : CMDBController
    {
        private readonly DevicesService service;
        private readonly PDFService _PDFservice;
        public LaptopController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            SitePart = "Laptop";
            Table = "laptop";
            _PDFservice = new();
        }

        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            ViewData["Title"] = "Laptop overview";
            ViewData["Controller"] = @"\Laptop\Create";
            await BuildMenu();
            var Desktops = await service.ListAll(SitePart);
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["AssignIdentityAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["actionUrl"] = @"\Laptop\Search";
            return View(Desktops);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["Title"] = "Laptop overview";
                ViewData["Controller"] = @"\Laptop\Create";
                await BuildMenu();
                var Desktops = await service.ListAll(SitePart, search);
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["AssignIdentityAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["actionUrl"] = @"\Laptop\Search";
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
            ViewData["Title"] = "Create Laptop";
            ViewData["Controller"] = @"\Laptop\Create";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            await BuildMenu();
            DeviceDTO laptop = new();
            ViewBag.Types = await service.ListAssetTypes(SitePart);
            ViewBag.Rams = await service.ListRams();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    laptop.AssetTag = values["AssetTag"];
                    laptop.SerialNumber = values["SerialNumber"];
                    laptop.RAM = values["RAM"];
                    int Type = Convert.ToInt32(values["AssetType"]);
                    var AssetType = await service.GetAssetTypeById(Type);
                    var category = await service.GetAsstCategoryByCategory("Laptop");
                    laptop.AssetType = AssetType;
                    laptop.Category = category;
                    laptop.MAC = values["MAC"];
                    if (await service.IsDeviceExisting(laptop))
                        ModelState.AddModelError("", "Asset already exist");
                    if (ModelState.IsValid)
                    {
                        await service.CreateNewDevice(laptop);
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
            return View(laptop);
        }
        public async Task<IActionResult> Edit(IFormCollection values, string id)
        {
            log.Debug("Using Edit in {0}", SitePart);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var laptop = await service.GetDeviceById(SitePart, id);
            if (laptop == null)
                return NotFound();
            ViewData["Title"] = "Edit Laptop";
            ViewData["Controller"] = @$"\Laptop\Edit\{id}";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            await BuildMenu();
            ViewBag.AssetTypes = await service.ListAssetTypes(SitePart);
            ViewBag.Rams = await service.ListRams();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newSerialNumber = values["SerialNumber"];
                    string newRam = values["RAM"];
                    int Type = Convert.ToInt32(values["AssetType.TypeID"]);
                    var newAssetType = await service.GetAssetTypeById(Type);
                    string newMAC = values["MAC"];
                    if (ModelState.IsValid)
                    {
                        await service.UpdateDevice(laptop, newRam, newMAC, newAssetType, newSerialNumber);
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
            return View(laptop);
        }
        public async Task<IActionResult> Details(string id)
        {
            log.Debug("Using details in {0}", Table);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var laptop = await service.GetDeviceById(SitePart, id);
            if (laptop == null)
                return NotFound();
            ViewData["Title"] = "Laptop details";
            ViewData["Controller"] = @"\Laptop\Create";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["IdentityOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            return View(laptop);
        }
        public async Task<IActionResult> Delete(IFormCollection values, string id)
        {
            log.Debug("Using Delete in {0}", SitePart);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var laptop = await service.GetDeviceById(SitePart, id);
            if (laptop == null)
                return NotFound();
            ViewData["Title"] = "Delete Laptop";
            ViewData["Controller"] = @$"\Laptop\Delete\{id}";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["backUrl"] = "Laptop";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        if (laptop.Identity.IdenId > 1)
                        {
                            var admin = await service.Admin();
                            await _PDFservice.SetUserinfo(
                                UserId: laptop.Identity.UserID,
                                ITEmployee: admin.Account.UserID,
                                Singer: laptop.Identity.Name,
                                FirstName: laptop.Identity.FirstName,
                                LastName: laptop.Identity.LastName,
                                Language: laptop.Identity.Language.Code,
                                Receiver: laptop.Identity.Name,
                                type:"Release");
                            await _PDFservice.SetDeviceInfo(laptop);
                            await _PDFservice.GenratPDFFile(Table, laptop.AssetTag);
                            await _PDFservice.GenratPDFFile("identity", laptop.Identity.IdenId);
                            await service.ReleaseIdenity(laptop);
                        }
                        await service.Deactivate(laptop, values["reason"]);
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
            return View(laptop);
        }
        public async Task<IActionResult> Activate(string id)
        {
            log.Debug("Using Activate in {0}", Table);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var laptop = await service.GetDeviceById(SitePart, id);
            if (laptop == null)
                return NotFound();
            ViewData["Title"] = "Activate Laptop";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.Activate(laptop);
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
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var laptop = await service.GetDeviceById(SitePart, id);
            if (laptop == null)
                return NotFound();
            ViewData["Title"] = "Assign identity to Laptop";
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["backUrl"] = "Laptop";
            ViewData["Controller"] = @$"\Laptop\AssignIdentity\{id}";
            await BuildMenu();
            ViewBag.Identities = await service.ListFreeIdentities();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    if (!service.IsDeviceFree(laptop))
                        ModelState.AddModelError("", "Laptop can not be assigned to another user");
                    var identity = await service.GetAssignedIdentity(Int32.Parse(values["Identity"]));
                    if (ModelState.IsValid)
                    {
                        await service.AssignIdentity2Device(identity,laptop);
                        return RedirectToAction("AssignForm", "Laptop", new { id });
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(laptop);
        }
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Release identity in {0}", Table);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var laptop = await service.GetDeviceById(SitePart, id);
            if (laptop == null)
                return NotFound();
            ViewData["Title"] = "Release identity from Laptop";
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["backUrl"] = "Laptop";
            ViewData["Action"] = "ReleaseIdentity";
            ViewData["Controller"] = @$"\Laptop\ReleaseIdentity\{id}";
            await BuildMenu();
            var identity = laptop.Identity;
            ViewData["Name"] = identity.Name;
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    await _PDFservice.SetUserinfo(
                        UserId: laptop.Identity.UserID,
                        ITEmployee: admin.Account.UserID,
                        Singer: laptop.Identity.Name,
                        FirstName: laptop.Identity.FirstName,
                        LastName: laptop.Identity.LastName,
                        Language: laptop.Identity.Language.Code,
                        Receiver: laptop.Identity.Name);
                    await _PDFservice.SetDeviceInfo(laptop);
                    await _PDFservice.GenratPDFFile(Table, laptop.AssetTag);
                    await _PDFservice.GenratPDFFile("identity", laptop.Identity.IdenId);
                    await service.ReleaseIdenity(laptop);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(laptop);
        }
        public async Task<IActionResult> AssignForm(IFormCollection values, string id)
        {
            log.Debug("Using Assign form in {0}", Table);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var laptop = await service.GetDeviceById(SitePart, id);
            if (laptop == null)
                return NotFound();
            ViewData["Title"] = "Assign form";
            ViewData["backUrl"] = "Laptop";
            ViewData["Action"] = "AssignForm";
            await BuildMenu();
            ViewData["Name"] = laptop.Identity.Name;
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
                        UserId: laptop.Identity.UserID,
                        ITEmployee: admin.Account.UserID,
                        Singer: laptop.Identity.Name,
                        FirstName: laptop.Identity.FirstName,
                        LastName: laptop.Identity.LastName,
                        Language: laptop.Identity.Language.Code,
                        Receiver: laptop.Identity.Name,
                        type: "Release");
                    await _PDFservice.SetDeviceInfo(laptop);
                    await _PDFservice.GenratPDFFile(Table, laptop.AssetTag);
                    await _PDFservice.GenratPDFFile("identity", laptop.Identity.IdenId);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(laptop);
        }
    }
}
