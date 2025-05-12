using CMDB.API.Models;
using CMDB.Domain.Entities;
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
    /// This controller is used to manage the monitor devices
    /// </summary>
    public class MonitorController : CMDBController
    {
        private readonly DevicesService service;
        private readonly PDFService _PDFservice;
        /// <summary>
        /// The constructor is used to inject the IWebHostEnvironment
        /// </summary>
        /// <param name="env"></param>
        public MonitorController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            SitePart = "Monitor";
            Table = "screen";
            _PDFservice = new();
        }
        /// <summary>
        /// The index page which will show all the monitors
        /// </summary>
        /// <returns></returns>
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
            ViewData["AssignKeyAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignKensington");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["actionUrl"] = @"\Monitor\Search";
            ViewData["Controller"] = @"\Monitor\Create";
            return View(Desktops);
        }
        /// <summary>
        /// The search page which will show all the monitors matching the search string
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            if (!string.IsNullOrEmpty(search))
            {
                ViewData["Title"] = "Monitor overview";
                await BuildMenu();
                var Desktops = await service.ListAll(SitePart, search);
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["AssignIdentityAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
                ViewData["AssignKeyAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignKensington");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["actionUrl"] = @"\Monitor\Search";
                ViewData["Controller"] = @"\Monitor\Create";
                return View(Desktops);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        /// <summary>
        /// The delete page which will delete the monitor
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(IFormCollection values, string id)
        {
            log.Debug("Using Delete in {0}", SitePart);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var monitor = await service.GetDeviceById(SitePart, id);
            if (monitor == null)
                return NotFound();
            ViewData["Title"] = "Delete Monitor";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["backUrl"] = "Admin";
            ViewData["Controller"] = @$"\Monitor\Delete\{id}";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
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
                            if (monitor.Kensington != null)
                            {
                                await _PDFservice.SetKeyInfo(monitor.Kensington);
                                await service.ReleaseKensington(monitor);
                            }
                            await _PDFservice.GenratePDFFile(Table, monitor.AssetTag);
                            await _PDFservice.GenratePDFFile("identity", monitor.Identity.IdenId);
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
        /// <summary>
        /// The activate page which will activate the monitor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Activate(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var moniror = await service.GetDeviceById(SitePart, id);
            if (moniror == null)
                return NotFound();
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Laptop";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
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
        /// <summary>
        /// The create page which will create a new monitor
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug($"Using Create in {SitePart}");
            await BuildMenu();
            ViewData["Title"] = "Create monitor";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewBag.Types = await service.ListAssetTypes(SitePart);
            ViewData["backUrl"] = "Desktop";
            ViewData["Controller"] = @"\Monitor\Create";
            DeviceDTO screen = new();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
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
        /// <summary>
        /// The details page which will show the details of the monitor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var screen = await service.GetDeviceById(SitePart, id);
            if (screen == null)
                return NotFound();
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = "Monitor details";
            ViewData["Controller"] = @"\Monitor\Create";
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
            return View(screen);
        }
        /// <summary>
        /// The edit page which will edit the monitor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(string id, IFormCollection values)
        {
            log.Debug("Using Edit in {0}", SitePart);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var screen = await service.GetDeviceById(SitePart, id);
            if (screen == null)
                return NotFound();
            await BuildMenu();
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["Title"] = "Edit monitor";
            ViewBag.Types = await service.ListAssetTypes(SitePart);
            ViewData["backUrl"] = "Monitor";
            ViewData["Controller"] = @$"\Monitor\Edit\{id}";
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
        /// <summary>
        /// The assign identity page which will assign the identity to the monitor
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AssignIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Assign identity in {0}", Table);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var moniror = await service.GetDeviceById(SitePart, id);
            if (moniror == null)
                return NotFound();
            ViewData["Title"] = "Assign identity to Monitor";
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["backUrl"] = "Monitor";
            await BuildMenu();
            ViewData["Controller"] = @$"\Monitor\AssignIdentity\{id}";
            ViewBag.Identities = await service.ListFreeIdentities(Table);
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
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
        /// <summary>
        /// The assign form page which will show the information of the monitor and what is assigned to it
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AssignForm(IFormCollection values, string id)
        {
            log.Debug("Using Assign form in {0}", Table);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var monitor = await service.GetDeviceById(SitePart, id);
            if (monitor == null)
                return NotFound();
            ViewData["Title"] = "Assign form";
            ViewData["backUrl"] = "Monitor";
            ViewData["Action"] = "AssignForm";
            await BuildMenu();
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
                    await _PDFservice.GenratePDFFile(Table, monitor.AssetTag);
                    await _PDFservice.GenratePDFFile("identity", monitor.Identity.IdenId);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(monitor);
        }
        /// <summary>
        /// The release identity page which will release the identity from the monitor
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, string id)
        {
            log.Debug("Using Release identity in {0}", Table);
            await BuildMenu();
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var monitor = await service.GetDeviceById(SitePart, id);
            if (monitor == null)
                return NotFound();
            ViewData["Title"] = "Release identity from Monitor";
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["backUrl"] = "Monitor";
            ViewData["Action"] = "ReleaseIdentity";
            ViewData["Controller"] = @$"\Monitor\ReleaseIdentity\{id}";
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
                    await _PDFservice.GenratePDFFile(Table, monitor.AssetTag);
                    await _PDFservice.GenratePDFFile("identity", monitor.Identity.IdenId);
                    await service.ReleaseIdenity(monitor);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(monitor);
        }
        /// <summary>
        /// Page to assign a Kensington key to a laptop
        /// </summary>
        /// <param name="id"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<IActionResult> AssignKensington(string id, IFormCollection values)
        {
            log.Debug("Using Assign Kensington in {0}", Table);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var monitor = await service.GetDeviceById(SitePart, id);
            if (monitor == null)
                return NotFound();
            ViewData["Title"] = "Assign Kensington to Monitor";
            ViewData["AssignKeyAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignKensington");
            ViewData["backUrl"] = "Monitor";
            ViewData["Controller"] = @$"\Monitor\AssignKensington\{id}";
            ViewBag.Keys = await service.ListFreeKeys();
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                int keyId = Int32.Parse(values["Kensington"]);
                var key = await service.GetKensingtonById(keyId);
                if (ModelState.IsValid)
                {
                    try
                    {
                        await service.AssignKensington2Device(key, monitor);
                        return RedirectToAction("AssignForm", "Laptop", new { id });
                    }
                    catch (Exception ex)
                    {
                        log.Error("Database exception {0}", ex.ToString());
                        ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                            "see your system administrator.");
                    }
                }
            }
            return View(monitor);
        }
        /// <summary>
        /// Page to release a Kensington key from a laptop
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReleaseKensington(IFormCollection values, string id)
        {
            log.Debug("Using Release Kensington in {0}", Table);
            if (string.IsNullOrEmpty(id))
                return NotFound();
            var monitor = await service.GetDeviceById(SitePart, id);
            if (monitor == null)
                return NotFound();
            ViewData["Title"] = "Release Kensington from Monitor";
            ViewData["ReleaseKensington"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseKensington");
            ViewData["backUrl"] = "Monitor";
            ViewData["Controller"] = @$"\Monitor\ReleaseKensington\{id}";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.ReleaseKensington(monitor);
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
    }
}
