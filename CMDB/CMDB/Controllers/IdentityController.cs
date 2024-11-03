using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Device = CMDB.Domain.Entities.Device;

namespace CMDB.Controllers
{
    public class IdentityController : CMDBController
    {
        private new readonly IdentityService service;
        private readonly PDFService _PDFservice;
        public IdentityController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            Table = "identity";
            SitePart = "Identity";
            _PDFservice = new();
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            ViewData["Title"] = "Identity overview";
            ViewData["Controller"] = @"\Identity\Create";
            await BuildMenu();
            var list = await service.ListAll();
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["AssignAccountAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignAccount");
            ViewData["AssignDeviceAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignDevice");
            ViewData["ReleaseDevice"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseDevice");
            ViewData["actionUrl"] = @"\Identity\Search";
            return View(list);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search in {0}", Table);
            if (!string.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var list = await service.ListAll(search);
                ViewData["Title"] = "Identity overview";
                ViewData["Controller"] = @"\Identity\Create";
                await BuildMenu();
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["AssignAccountAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignAccount");
                ViewData["AssignDeviceAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignDevice");
                ViewData["actionUrl"] = @"\Identity\Search";
                return View(list);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var identity = await service.GetByID((int)id);
            if (identity == null)
                return NotFound();
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = "Identity Details";
            ViewData["Controller"] = @"\Identity\Create";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["AccountOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AccountOverview");
            ViewData["DeviceOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "DeviceOverview");
            ViewData["AssignDevice"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignDevice");
            ViewData["AssignAccount"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignAccount");
            ViewData["ReleaseAccount"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseAccount");
            ViewData["ReleaseDevice"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseDevice");
            ViewData["MobileOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "MobileOverview");
            ViewData["ReleaseMobile"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseMobile");
            ViewData["SubscriptionOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "SubscriptionOverview");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            return View(identity);
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", Table);
            ViewData["Title"] = "Create Identity";
            ViewData["Controller"] = @"\Identity\Create";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            await BuildMenu();
            ViewBag.Types = await service.ListActiveIdentityTypes();
            ViewBag.Languages = await service.ListAllActiveLanguages();
            IdentityDTO identity = new();
            string FormSubmit = values["form-submitted"];
            try
            {
                if (!string.IsNullOrEmpty(FormSubmit))
                {
                    string FirstName = values["FirstName"];
                    identity.FirstName = FirstName;
                    string LastName = values["LastName"];
                    identity.LastName = LastName;
                    string UserID = values["UserID"];
                    identity.UserID = UserID;
                    string Company = values["Company"];
                    identity.Company = Company;
                    string Type = values["Type"];
                    string EMail = values["EMail"];
                    identity.EMail = EMail;
                    string Language = values["Language"];
                    if (await service.IsExisting(identity))
                        ModelState.AddModelError("", "Idenity alreday existing");
                    if (ModelState.IsValid)
                    {
                        await service.Create(FirstName, LastName, Convert.ToInt32(Type), UserID, Company, EMail, Language);
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                    "see your system administrator.");
                log.Error(ex.ToString());
            }
            return View(identity);
        }
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            IdentityDTO identity = await service.GetByID((int)id);
            if (identity == null)
                return NotFound();
            log.Debug("Using Edit in {0}", Table);
            ViewData["Title"] = "Edit Identity";
            ViewData["Controller"] = @$"\Identity\Edit\{id}";
            await BuildMenu();
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewBag.Types = await service.ListActiveIdentityTypes();
            ViewBag.Languages = await service.ListAllActiveLanguages();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                string NewFirstName = values["FirstName"];
                string NewLastName = values["LastName"];
                string NewUserID = values["UserID"];
                string NewCompany = values["Company"];
                string NewType = values["Type.TypeId"];
                string NewLanguage = values["Language.Code"];
                string NewEMail = values["EMail"];
                if (await service.IsExisting(identity, NewUserID))
                    ModelState.AddModelError("", "Idenity alreday existing");
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.Edit(identity, NewFirstName, NewLastName, Convert.ToInt32(NewType), NewUserID, NewCompany, NewEMail, NewLanguage);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("DB error: {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(identity);
        }
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            IdentityDTO identity = await service.GetByID((int)id);
            if (identity == null)
                return NotFound();
            log.Debug("Using Delete in {0}", Table);
            ViewData["Title"] = "Deactivate Identity";
            ViewData["Controller"] = @$"\Identity\Delete\{id}";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            ViewData["backUrl"] = "Identity";
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    await service.Deactivate(identity, ViewData["reason"].ToString());
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    log.Error("DB error: {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(identity);
        }
        public async Task<IActionResult> Activate(int? id)
        {
            if (id == null)
                return NotFound();
            IdentityDTO identity = await service.GetByID((int)id);
            if (identity == null)
                return NotFound();
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Identity";
            ViewData["Controller"] = @$"\Identity\Activate\{id}";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.Activate(identity);
                return RedirectToAction(nameof(Index));
            }
            else
                RedirectToAction(nameof(Index));
            return View();
        }
        public async Task<IActionResult> AssignDevice(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            IdentityDTO identity = await service.GetByID((int)id);
            if (identity == null)
                return NotFound();
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Assign device to identity";
            ViewData["AssignDevice"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignDevice");
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            ViewBag.Devices = await service.ListAllFreeDevices();
            ViewBag.Mobiles = await service.ListAllFreeMobiles();
            ViewData["backUrl"] = "Identity";
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                List<DeviceDTO> devicesToAdd = new();
                List<MobileDTO> mobilesToAdd = new();
                var devices = await service.ListAllFreeDevices();
                foreach (var device in devices)
                {
                    if (!string.IsNullOrEmpty(values[device.AssetTag]))
                        devicesToAdd.Add(device);
                }
                var mobiles = await service.ListAllFreeMobiles();
                foreach (var mobile in mobiles)
                {
                    if (!string.IsNullOrEmpty(values[mobile.IMEI.ToString()]))
                        mobilesToAdd.Add(mobile);
                }
                if (devicesToAdd.Count == 0 && mobilesToAdd.Count == 0)
                {
                    ModelState.AddModelError("", "Please select at lease 1 Device");
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        if(devicesToAdd.Count > 0)
                            await service.AssignDevice(identity, devicesToAdd);
                        if (mobilesToAdd.Count > 0)
                            service.AssignMobiles(identity, mobilesToAdd);
                        return RedirectToAction("AssignForm", "Identity", new { id });
                    }
                    catch (Exception ex)
                    {
                        log.Error("DB error: {0}", ex.ToString());
                        ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                            "see your system administrator.");
                    }
                }
            }
            return View(identity);
        }
        public async Task<IActionResult> AssignAccount(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            IdentityDTO identity = await service.GetByID((int)id);
            if (identity == null)
                return NotFound();
            log.Debug("Using Assign Account in {0}", Table);
            ViewData["Title"] = "Assign Account";
            ViewData["AssignAccount"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignAccount");
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            ViewBag.Identity = identity;
            ViewBag.Accounts = await service.ListAllFreeAccounts();
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                int AccId = Convert.ToInt32(values["Account"]);
                DateTime from = DateTime.Parse(values["ValidFrom"]);
                DateTime until = DateTime.Parse(values["ValidUntil"]);
                try
                {
                    if (service.IsPeriodOverlapping(id, from, until))
                        ModelState.AddModelError("", "Periods are overlapping please choose other dates");
                    if (ModelState.IsValid)
                    {
                        await service.AssignAccount2Idenity(identity, AccId, from, until);
                        return RedirectToAction("AssignForm", "Identity", new { id });
                    }
                }
                catch (Exception ex)
                {
                    log.Error("DB error: {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View();
        }
        public async Task<IActionResult> ReleaseAccount(IFormCollection values, int id)
        {
            if (id == 0)
                return NotFound();
            var idenAccount = await service.GetIdenAccountByID(id);
            if (idenAccount == null)
                return NotFound();
            log.Debug("Using Release Account in {0}", Table);
            ViewData["Title"] = "Release Account";
            ViewBag.Identity = idenAccount.Identity;
            ViewBag.Account = idenAccount.Account;
            ViewData["ReleaseAccount"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseAccount");
            await BuildMenu();
            ViewData["backUrl"] = "Identity";
            ViewData["Action"] = "ReleaseAccount";
            ViewData["Name"] = idenAccount.Identity.Name;
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    /*PDFGenerator PDFGenerator = new()
                    {
                        ITEmployee = ITPerson,
                        Singer = Employee,
                        UserID = idenAccount.Identity.UserID,
                        FirstName = idenAccount.Identity.FirstName,
                        LastName = idenAccount.Identity.LastName,
                        Language = idenAccount.Identity.Language.Code,
                        Receiver = idenAccount.Identity.Name,
                        Type = "Release"
                    };
                    PDFGenerator.SetAccontInfo(idenAccount);
                    string pdfFile = PDFGenerator.GeneratePath(_env);
                    PDFGenerator.GeneratePdf(pdfFile);*/
                    await service.ReleaseAccount4Identity(idenAccount);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
        public async Task<IActionResult> AssignForm(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            IdentityDTO identity = await service.GetByID((int)id);
            if (identity == null)
                return NotFound();
            log.Debug("Using Assign Form in {0}", Table);
            ViewData["Title"] = "Assign form";
            ViewData["AssignDevice"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignDevice");
            ViewData["AssignAccount"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignAccount");
            await BuildMenu();
            ViewData["backUrl"] = "Identity";
            ViewData["Action"] = "AssignForm";
            ViewData["Name"] = identity.Name;
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                await _PDFservice.SetUserinfo(identity.UserID,
                        ITPerson,
                        Employee,
                        identity.FirstName,
                        identity.LastName,
                        identity.Name,
                        identity.Language.Code);
                if (identity.Accounts.Count > 0)
                {
                    foreach (var account in identity.Accounts)
                        await _PDFservice.SetAccontInfo(account);
                }
                if (identity.Devices.Count > 0)
                {
                    foreach (var d in identity.Devices)
                        await _PDFservice.SetDeviceInfo(d);
                }
                /* if(identity.Mobiles.Count > 0) 
                 {
                     foreach (Mobile mobile in identity.Mobiles)
                         PDFGenerator.SetMobileInfo(mobile);
                 }*/
                 await _PDFservice.GenratPDFFile(Table, identity.IdenId);
                 return RedirectToAction(nameof(Index));
             }
             return View(identity);
         }
         [Route("ReleaseDevice/{id:int}/{AssetTag}")]
         public async Task<IActionResult> ReleaseDevice(IFormCollection values, int? id, string AssetTag)
         {
             if (id == null)
                 return NotFound();
             IdentityDTO identity = await service.GetByID((int)id);
             if (identity == null)
                 return NotFound();
             log.Debug("Using Release from in {0}", Table);
             ViewData["Title"] = "Release device from identity";
             ViewData["ReleaseDevice"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseDevice");
             await BuildMenu();
             var device = await service.GetDevice(AssetTag);
             if (device == null) 
                 return NotFound();
             ViewBag.Device = device;
             ViewData["backUrl"] = "Identity";
             ViewData["Action"] = "ReleaseDevice";
             ViewData["Name"] = identity.Name;
             var admin = await service.Admin();
             ViewData["AdminName"] = admin.Account.UserID;
             string FormSubmit = values["form-submitted"];
             if (!string.IsNullOrEmpty(FormSubmit))
             {
                 List<DeviceDTO> devices2Remove = new()
                 {
                     device
                 };
                 string Employee = values["Employee"];
                 string ITPerson = values["ITEmp"];
                await _PDFservice.SetUserinfo(identity.UserID,
                        ITPerson,
                        Employee,
                        identity.FirstName,
                        identity.LastName,
                        identity.Name,
                        identity.Language.Code,
                        "Release");
                await _PDFservice.SetDeviceInfo(device);
                await service.ReleaseDevices(identity, devices2Remove);
                await _PDFservice.GenratPDFFile(Table, identity.IdenId);
                return RedirectToAction(nameof(Index));
            }
            return View(identity);
        }
        [Route("ReleaseMobile/{id:int}/{MobileId}")]
        public async Task<IActionResult> ReleaseMobile(IFormCollection values, int? id, int MobileId)
        {
            if (id == null && MobileId == 0)
                return NotFound();
            IdentityDTO identity = await service.GetByID((int)id);
            if (identity == null)
                return NotFound();
            MobileDTO mobile = await service.GetMobile(MobileId);
            if (mobile == null)
                return NotFound();
            log.Debug("Using Release from in {0}", Table);
            ViewData["Title"] = "Release mobile from identity";
            ViewData["ReleaseMobile"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseMobile");
            await BuildMenu();            
            ViewBag.Mobile = mobile;
            ViewData["backUrl"] = "Identity";
            ViewData["Action"] = "ReleaseMobile";
            ViewData["Name"] = identity.Name;
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                await _PDFservice.SetUserinfo(identity.UserID,
                    ITPerson,
                    Employee,
                    identity.FirstName,
                    identity.LastName,
                    identity.Name,
                    identity.Language.Code,
                    "Release");
                /*
                PDFGenerator.SetMobileInfo(mobile);
                await service.ReleaseMobile(identity, mobile, Table);
                await service.LogPdfFile("mobile", mobile.MobileId, PdfFile);*/
                await _PDFservice.GenratPDFFile(Table, identity.IdenId);
                return RedirectToAction(nameof(Index));
            }
            return View(identity);
        }
        public async Task<IActionResult> ReleaseDevices(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            IdentityDTO identity = await service.GetByID((int)id);
            if (identity == null)
                return NotFound();
            log.Debug("Using Release from in {0}", Table);
            ViewData["Title"] = "Release devices from identity";
            ViewData["ReleaseDevice"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseDevice");
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                List<DeviceDTO> devicesToRelease = new();
                foreach (var device in identity.Devices)
                {
                    if (!string.IsNullOrEmpty(values[device.AssetTag])) { 
                        devicesToRelease.Add(device);
                    }
                }
                if (ModelState.IsValid)
                {
                    var admin = await service.Admin();
                    await service.ReleaseDevices(identity, devicesToRelease);
                    await _PDFservice.SetUserinfo(identity.UserID,
                        admin.Account.UserID,
                        identity.Name,
                        identity.FirstName,
                        identity.LastName,
                        identity.Name,
                        identity.Language.Code
                        ,"Release");
                     foreach (var device in devicesToRelease)
                     {
                         await _PDFservice.SetDeviceInfo(device);
                     }
                    await _PDFservice.GenratPDFFile(Table, identity.IdenId);
                    return RedirectToAction(nameof(Index));
                    //return RedirectToAction("ReleaseForm", "Identity", new {id, devicesToRelease});
                }
            }   
            return View(identity);
        }
        public async Task<IActionResult> ReleaseForm(IFormCollection values, int? id, List<Device> releasedDevices)
        {
            if (id == null)
                return NotFound();
            IdentityDTO identity = await service.GetByID((int)id);
            if (identity == null)
                return NotFound();
            log.Debug("Using Releasefrom in {0}", Table);
            ViewData["Title"] = "Releasefrom";
            ViewData["ReleaseDevice"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseDevice");
            await BuildMenu();
            ViewData["backUrl"] = "Identity";
            ViewData["Action"] = "ReleaseDevice";
            ViewData["Name"] = identity.Name;
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                await _PDFservice.SetUserinfo(identity.UserID,
                ITPerson,
                Employee,
                identity.FirstName,
                identity.LastName,
                identity.Name,
                identity.Language.Code);
                /* foreach (var device in devices)
                {
                    PDFGenerator.SetAssetInfo(device);
                }*//*
                string PdfFile = PDFGenerator.GeneratePath(_env);
                PDFGenerator.GeneratePdf(PdfFile);
                await service.LogPdfFile(Table, identity.IdenId, PdfFile);*/
                return RedirectToAction(nameof(Index));
            }
            return View(identity);
        }
    }
}
