using CMDB.Domain.DTOs;
using CMDB.Infrastructure;
using CMDB.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    /// <summary>
    /// Controller for Desktop
    /// </summary>
    public class DesktopController : CMDBController
    {
        private readonly DevicesService service;
        private readonly PDFService _PDFservice;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        public DesktopController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            SitePart = "Desktop";
            Table = "desktop";
            _PDFservice = new PDFService();
        }
        /// <summary>
        /// This is the index page for the desktop
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            ViewData["Title"] = "Desktop overview";
            await BuildMenu();
            var Desktops = await service.ListAll(SitePart);
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["AssignIdentityAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["AssignKeyAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignKensington");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["actionUrl"] = @"\Desktop\Search";
            ViewData["Controller"] = @"\Desktop\Create";
            return View(Desktops);
        }
        /// <summary>
        /// This is the search page for the desktop
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (!String.IsNullOrEmpty(search))
            {
                await BuildMenu();
                ViewData["Title"] = "Desktop overview";
                ViewData["search"] = search;
                var Desktops = await service.ListAll(SitePart, search);
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["AssignIdentityAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
                ViewData["AssignKeyAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignKensington");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["actionUrl"] = @"\Desktop\Search";
                ViewData["Controller"] = @"\Desktop\Create";
                return View(Desktops);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        /// <summary>
        /// This is the create page for the desktop
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            ViewData["Title"] = "Create Desktop";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["Controller"] = @"\Desktop\Create";
            await BuildMenu();
            DeviceDTO desktop = new();
            ViewBag.Types = await service.ListAssetTypes(SitePart);
            ViewBag.Rams = await service.ListRams();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    desktop.AssetTag = values["AssetTag"];
                    desktop.SerialNumber = values["SerialNumber"];
                    desktop.RAM = values["RAM"];
                    int Type = Convert.ToInt32(values["AssetType"]);
                    var AssetType = await service.GetAssetTypeById(Type);
                    var category = await service.GetAsstCategoryByCategory("Desktop");
                    desktop.AssetType = AssetType;
                    desktop.Category = category;
                    desktop.MAC = values["MAC"];
                    if (await service.IsDeviceExisting(desktop))
                        ModelState.AddModelError("", "Asset already exist");
                    if (ModelState.IsValid)
                    {
                        await service.CreateNewDevice(desktop);
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
        /// <summary>
        /// This is the edit page for the desktop
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(IFormCollection values, string id)
        {
            log.Debug("Using Edit in {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var desktop = await service.GetDeviceById(SitePart, id);
            if (desktop == null)
                return NotFound();
            ViewData["Title"] = "Edit Desktop";
            ViewData["Controller"] = @$"\Desktop\Edit\{id}";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            await BuildMenu();            
            ViewBag.AssetTypes = await service.ListAssetTypes(SitePart);
            ViewBag.Rams = await service.ListRams();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
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
                        await service.UpdateDevice(desktop, newRam, newMAC, newAssetType, newSerialNumber);
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
        /// <summary>
        /// This is the details page for the desktop
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(string id)
        {
            log.Debug("Using details in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var desktop = await service.GetDeviceById(SitePart, id);
            if (desktop == null)
                return NotFound();
            ViewData["Title"] = "Desktop details";
            ViewData["Controller"] = @"\Desktop\Create";
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
            return View(desktop);
        }
        /// <summary>
        /// This is the delete page for the desktop
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(IFormCollection values, string id)
        {
            log.Debug("Using Delete in {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var desktop = await service.GetDeviceById(SitePart, id);
            if (desktop == null)
                return NotFound();
            ViewData["Title"] = "Delete Desktop";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["backUrl"] = "Desktop";
            ViewData["Controller"] = @$"\Desktop\Delete\{id}";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        if (desktop.Identity.IdenId > 1)
                        {
                            var admin = await service.Admin();
                            await _PDFservice.SetUserinfo(
                                UserId: desktop.Identity.UserID,
                                ITEmployee: admin.Account.UserID,
                                Singer: desktop.Identity.Name,
                                FirstName: desktop.Identity.FirstName,
                                LastName: desktop.Identity.LastName,
                                Language: desktop.Identity.Language.Code,
                                Receiver: desktop.Identity.Name,
                                type: "Release");
                            await _PDFservice.SetDeviceInfo(desktop);
                            if (desktop.Kensington != null)
                            {
                                await _PDFservice.SetKeyInfo(desktop.Kensington);
                                await service.ReleaseKensington(desktop);
                            }
                            await _PDFservice.GenratePDFFile(Table, desktop.AssetTag);
                            await _PDFservice.GenratePDFFile("identity", desktop.Identity.IdenId);
                            await service.ReleaseIdenity(desktop);
                        }
                        await service.Deactivate(desktop, values["reason"]);
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
        /// <summary>
        /// This is the activate page for the desktop
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Activate(string id)
        {
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Laptop";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var desktop = await service.GetDeviceById(SitePart, id);
            if (desktop == null)
                return NotFound();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.Activate(desktop);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View();
        }
        /// <summary>
        /// This is the assign Kensington page for the desktop
        /// </summary>
        /// <param name="id"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<IActionResult> AssignKensington(string id, IFormCollection values)
        {
            log.Debug("Using Assign Kensington in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var desktop = await service.GetDeviceById(SitePart, id);
            if (desktop == null)
                return NotFound();
            ViewData["Title"] = "Assign Kensington to Desktop";
            ViewData["backUrl"] = "Desktop";
            ViewData["Controller"] = @$"\Desktop\AssignKensington\{id}";
            ViewData["AssignKeyAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignKensington");
            ViewBag.Keys = await service.ListFreeKeys();
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                int keyId = Int32.Parse(values["Kensington"]);
                var key = await service.GetKensingtonById(keyId);
                if (ModelState.IsValid)
                {
                    try
                    {
                        await service.AssignKensington2Device(key, desktop);
                        return RedirectToAction("AssignForm", "Desktop", new { id });
                    }
                    catch (Exception ex)
                    {
                        log.Error("Database exception {0}", ex.ToString());
                        ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                            "see your system administrator.");
                    }
                }
            }
            return View(desktop);
        }
        /// <summary>
        /// This is the assign identity page for the desktop
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AssignIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Assign identity in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            await BuildMenu();
            if (id == null)
                return NotFound();
            var desktop = await service.GetDeviceById(SitePart, id);
            if (desktop == null)
                return NotFound();
            ViewData["Title"] = "Assign identity to Desktop";
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["backUrl"] = "Desktop";
            ViewBag.Identities = await service.ListFreeIdentities(Table);
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    if (!service.IsDeviceFree(desktop))
                        ModelState.AddModelError("", "Desktop can not be assigned to another user");
                    var identity = await service.GetAssignedIdentity(Int32.Parse(values["Identity"]));
                    if (ModelState.IsValid)
                    {
                        await service.AssignIdentity2Device(identity, desktop);
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
        /// <summary>
        /// This is the assign form page for the desktop
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AssignForm(IFormCollection values, string id)
        {
            log.Debug("Using Assign form in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id == null)
                return NotFound();
            var desktop = await service.GetDeviceById(SitePart, id);
            if (desktop == null)
                return NotFound();
            ViewData["Title"] = "Assign form";
            ViewData["backUrl"] = "Desktop";
            ViewData["Action"] = "AssignForm";
            await BuildMenu();
            ViewData["Name"] = desktop.Identity.Name;
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
                        UserId: desktop.Identity.UserID,
                        ITEmployee: admin.Account.UserID,
                        Singer: desktop.Identity.Name,
                        FirstName: desktop.Identity.FirstName,
                        LastName: desktop.Identity.LastName,
                        Language: desktop.Identity.Language.Code,
                        Receiver: desktop.Identity.Name);
                    await _PDFservice.SetDeviceInfo(desktop);
                    await _PDFservice.GenratePDFFile(Table, desktop.AssetTag);
                    await _PDFservice.GenratePDFFile("identity", desktop.Identity.IdenId);
                    if (desktop.Kensington is not null)
                    {
                        await _PDFservice.SetKeyInfo(desktop.Kensington);
                        await _PDFservice.GenratePDFFile("kensington", desktop.Kensington.KeyID);
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(desktop);
        }
        /// <summary>
        /// This is the release identity page for the desktop
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Release identity in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id == null)
                return NotFound();
            var desktop = await service.GetDeviceById(SitePart, id);
            if (desktop == null)
                return NotFound();
            ViewData["Title"] = "Release identity from Desktop";
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["backUrl"] = "Desktop";
            ViewData["Action"] = "ReleaseIdentity";
            await BuildMenu();            
            var identity = desktop.Identity;
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
                        UserId: desktop.Identity.UserID,
                        ITEmployee: admin.Account.UserID,
                        Singer: desktop.Identity.Name,
                        FirstName: desktop.Identity.FirstName,
                        LastName: desktop.Identity.LastName,
                        Language: desktop.Identity.Language.Code,
                        Receiver: desktop.Identity.Name, 
                        type: "Release");
                    await _PDFservice.SetDeviceInfo(desktop);
                    await _PDFservice.GenratePDFFile(Table, desktop.AssetTag);
                    await _PDFservice.GenratePDFFile("identity", desktop.Identity.IdenId);
                    await service.ReleaseIdenity(desktop);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(desktop);
        }
        /// <summary>
        /// This is the release Kensington page for the desktop
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReleaseKensington(IFormCollection values, string id)
        {
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var desktop = await service.GetDeviceById(SitePart, id);
            if (desktop == null)
                return NotFound();
            ViewData["Title"] = "Release Kensington from Desktop";
            ViewData["backUrl"] = "Desktop";
            ViewData["Action"] = "ReleaseKensington";
            ViewData["Controller"] = @$"\Desktop\ReleaseKensington\{id}";
            ViewData["ReleaseKensington"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseKensington");
            await BuildMenu();
            var identity = desktop.Identity;
            ViewData["Name"] = identity.Name;
            var admin = await service.Admin();
            var key = desktop.Kensington;
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    await _PDFservice.SetUserinfo(
                        UserId: desktop.Identity.UserID,
                        ITEmployee: admin.Account.UserID,
                        Singer: desktop.Identity.Name,
                        FirstName: desktop.Identity.FirstName,
                        LastName: desktop.Identity.LastName,
                        Language: desktop.Identity.Language.Code,
                        Receiver: desktop.Identity.Name);
                    await service.ReleaseKensington(desktop);
                    await _PDFservice.SetDeviceInfo(desktop);
                    await _PDFservice.SetKeyInfo(key);
                    await _PDFservice.GenratePDFFile(Table, desktop.AssetTag);
                    await _PDFservice.GenratePDFFile("kensington", key.KeyID);
                    await _PDFservice.GenratePDFFile("identity", identity.IdenId);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(desktop);
        }
    }
}
