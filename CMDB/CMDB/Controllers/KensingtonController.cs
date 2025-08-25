using CMDB.Domain.DTOs;
using CMDB.Infrastructure;
using CMDB.Services;
using CMDB.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    /// <summary>
    /// The controller that handles the Kensington keys
    /// </summary>
    public class KensingtonController : CMDBController
    {
        private readonly KensingtonService service = new();
        private readonly PDFService _PDFservice;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        public KensingtonController(IWebHostEnvironment env) : base(env)
        {
            SitePart = "Kensington";
            Table = "kensington";
            _PDFservice = new();
        }
        /// <summary>
        /// The index page which shows all the keys
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            await BuildMenu();
            var keys = await service.ListAll();
            ViewData["Title"] = "Kensington overview";
            ViewData["Controller"] = @"\Kensington\Create";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["AssignDeviceAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignDevice");
            ViewData["actionUrl"] = @"\Kensington\Search";
            return View(keys);
        }
        /// <summary>
        /// The search page which shows all the keys matching the search string
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (!string.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                ViewData["Title"] = "Identity overview";
                ViewData["Controller"] = @"\Identity\Create";
                await BuildMenu();
                var list = await service.Search(search);
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["AssignDeviceAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignDevice");
                ViewData["actionUrl"] = @"\Identity\Search";
                return View(list);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// The page to create a new Kensington key
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using create for {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            await BuildMenu();
            ViewData["Title"] = "Create new Kensington";
            ViewData["Controller"] = @"\Kensington\Create";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewBag.Type = await service.ListAssetTypes("Kensington");
            KensingtonDTO kensington = new();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    var type = values["type"];
                    var serial = values["SerialNumber"];
                    kensington.SerialNumber = serial;
                    var hasLock = values["HasLock"];
                    bool haslock = StringExtensions.GetBool(hasLock);
                    kensington.HasLock = haslock;
                    var amount = values["AmountOfKeys"];
                    kensington.AmountOfKeys = int.Parse(amount);
                    AssetTypeDTO assettype = await service.GetAssetTypeById(int.Parse(type));
                    if (ModelState.IsValid)
                    {
                        await service.Create(serial, haslock, int.Parse(amount), assettype);
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
            return View(kensington);
        }
        /// <summary>
        /// The page to edit a Kensington key
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit for {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id is null)
                return NotFound();
            var key = await service.GetByID((int)id);
            if (key is null)
                return NotFound();
            await BuildMenu();
            ViewData["Title"] = "Edit Kensington";
            ViewData["Controller"] = @$"\Kensington\Edit\{id}";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewBag.Type = await service.ListAssetTypes("Kensington");
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    AssetTypeDTO assettype = await service.GetAssetTypeById(key.Type.TypeID);
                    key.Type = assettype;
                    key.SerialNumber = values["SerialNumber"];
                    var hasLock = values["HasLock"];
                    bool haslock = StringExtensions.GetBool(hasLock);
                    key.HasLock = haslock;
                    var amount = values["AmountOfKeys"];
                    key.AmountOfKeys = int.Parse(amount);
                    if (ModelState.IsValid)
                    {
                        await service.Update(key);
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
            return View(key);
        }
        /// <summary>
        /// The details page of a Kensington key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details for {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id is null)
                return NotFound();
            var key = await service.GetByID((int)id);
            if (key is null)
                return NotFound();
            await BuildMenu();
            ViewData["Title"] = "Kensington details";
            ViewData["Controller"] = @"\Kensington\Create";
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["DeviceOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "DeviceOverview");
            ViewData["ReleseDevice"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleseDevice");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            return View(key);
        }
        /// <summary>
        /// The page to delete a Kensington key
        /// </summary>
        /// <param name="values"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(IFormCollection values, int? Id)
        {
            log.Debug("Using delete for {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (Id is null)
                return NotFound();
            var key = await service.GetByID((int)Id);
            if (key is null)
                return NotFound();
            await BuildMenu();
            ViewData["Title"] = "Delete Kensington";
            ViewData["Controller"] = @$"\Kensington\Delete\{Id}";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.Deactivate(key, values["reason"]);
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
            return View(key);
        }
        /// <summary>
        /// The page to activate a Kensington key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using activate for {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id is null)
                return NotFound();
            var key = await service.GetByID((int)id);
            if (key is null)
                return NotFound();
            await BuildMenu();
            ViewData["Title"] = "Activate Kensington";
            ViewData["Controller"] = @$"\Kensington\Activate\{id}";
            bool hasAccess = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["ActiveAccess"] = hasAccess;
            if (hasAccess)
            {
                await service.Activate(key);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        /// <summary>
        /// The page to assign a Kensington key to a device
        /// </summary>
        /// <param name="id"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<IActionResult> AssignDevice(int? id, IFormCollection values)
        {
            log.Debug("Using assign for {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id is null)
                return NotFound();
            var key = await service.GetByID((int)id);
            if (key is null)
                return NotFound();
            await BuildMenu();
            ViewData["Title"] = "Assign Kensington";
            ViewData["Controller"] = @$"\Kensington\AssignDevice\{id}";
            ViewData["AssignDeviceAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignDevice");
            ViewData["Devices"] = await service.ListFreeDevices();
            ViewData["backUrl"] = "Kensington";
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                string assetTag = values["Device"];
                if (string.IsNullOrEmpty(assetTag))
                {
                    ModelState.AddModelError("", "No device selected");
                }
                if (ModelState.IsValid) {
                    DeviceDTO device = await service.GetDeviceByAssetTag(assetTag);
                    await service.AssignKey2Device(key, device);
                    return RedirectToAction("AssignForm", "Kensington", new { id });
                }

            }
            return View(key);
        }
        /// <summary>
        /// The page to release a Kensington key from a device
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <param name="MobileId"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReleaseDevice(IFormCollection values, int? id, string MobileId)
        {
            log.Debug("Using release for {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id is null || string.IsNullOrEmpty(MobileId))
                return NotFound();
            var key = await service.GetByID((int)id);
            if (key is null)
                return NotFound();
            var device = await service.GetDeviceByAssetTag(MobileId);
            if (device == null)
                return NotFound();
            await BuildMenu();
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            ViewData["Name"] = key.Device.Identity.Name;
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            ViewData["Title"] = "Release Kensington";
            ViewData["Controller"] = @$"\Kensington\ReleaseDevice\{id}\{MobileId}";
            ViewData["AssignDeviceAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignDevice");
            ViewData["backUrl"] = "Kensington";
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                await service.ReleaseDevice(key);
                await _PDFservice.SetUserinfo(device.Identity.Name, ITPerson, Employee, device.Identity.Name, device.Identity.LastName, device.Identity.Name, device.Identity.Language.Code);
                await _PDFservice.SetDeviceInfo(device);
                await _PDFservice.SetKeyInfo(key);
                await _PDFservice.GenratePDFFile(Table, key.KeyID);
                await _PDFservice.GenratePDFFile("identity", key.Device.Identity.IdenId);
                return RedirectToAction(nameof(Index));
            }
            return View(key);
        }
        /// <summary>
        /// The page that shows the assign form
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AssignForm(IFormCollection values, int? id)
        {
            log.Debug("Using AssignForm in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id == null)
                return NotFound();
            var key = await service.GetByID((int)id);
            if (key is null)
                return NotFound();
            await BuildMenu();
            log.Debug("Using Assign Form in {0}", Table);
            ViewData["Title"] = "Assign form";
            ViewData["backUrl"] = "Kensington";
            ViewData["Action"] = "AssignForm";
            ViewData["Name"] = key.Device.Identity.Name;
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit)) 
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                await _PDFservice.SetUserinfo(key.Device.Identity.Name,ITPerson,Employee, key.Device.Identity.Name, key.Device.Identity.LastName,key.Device.Identity.Name,key.Device.Identity.Language.Code);
                await _PDFservice.SetDeviceInfo(key.Device);
                await _PDFservice.SetKeyInfo(key);
                await _PDFservice.GenratePDFFile(Table, key.KeyID);
                await _PDFservice.GenratePDFFile("identity", key.Device.Identity.IdenId);
                return RedirectToAction(nameof(Index));
            }
            return View(key);
        }
    }
}
