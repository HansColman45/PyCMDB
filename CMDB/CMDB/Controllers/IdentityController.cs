using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Util;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using CMDB.Domain.Entities;
using CMDB.Services;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    public class IdentityController : CMDBController
    {
        private new readonly IdentityServices service;
        public IdentityController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
            Table = "identity";
            SitePart = "Identity";
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            ViewData["Title"] = "Identity overview";
            await BuildMenu();
            var list = await service.ListAll();
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            ViewData["AssignAccountAccess"] = service.HasAdminAccess(service.Admin, SitePart, "AssignAccount");
            ViewData["AssignDeviceAccess"] = service.HasAdminAccess(service.Admin, SitePart, "AssignDevice");
            ViewData["actionUrl"] = @"\Identity\Search";
            return View(list);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search in {0}", Table);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var list = await service.ListAll(search);
                ViewData["Title"] = "Identity overview";
                await BuildMenu();
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
                ViewData["AssignAccountAccess"] = service.HasAdminAccess(service.Admin, SitePart, "AssignAccount");
                ViewData["AssignDeviceAccess"] = service.HasAdminAccess(service.Admin, SitePart, "AssignDevice");
                ViewData["actionUrl"] = @"\Identity\Search";
                return View(list);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = "Identity Details";
            await BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["AccountOverview"] = service.HasAdminAccess(service.Admin, SitePart, "AccountOverview");
            ViewData["DeviceOverview"] = service.HasAdminAccess(service.Admin, SitePart, "DeviceOverview");
            ViewData["AssignDevice"] = service.HasAdminAccess(service.Admin, SitePart, "AssignDevice");
            ViewData["AssignAccount"] = service.HasAdminAccess(service.Admin, SitePart, "AssignAccount");
            ViewData["ReleaseAccount"] = service.HasAdminAccess(service.Admin, SitePart, "ReleaseAccount");
            ViewData["ReleaseDevice"] = service.HasAdminAccess(service.Admin, SitePart, "ReleaseDevice");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
                return NotFound();
            var list = await service.GetByID((int)id);
            service.GetAssingedDevices(list.First());
            service.GetAssignedAccounts(list.First());
            service.GetLogs(Table, (int)id, list.First());
            if (list == null)
                return NotFound();

            return View(list);
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", Table);
            ViewData["Title"] = "Create Identity";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            await BuildMenu();
            ViewBag.Types = service.ListActiveIdentityTypes();
            ViewBag.Languages = service.ListAllActiveLanguages();
            Identity identity = new();
            string FormSubmit = values["form-submitted"];
            try
            {
                if (!String.IsNullOrEmpty(FormSubmit))
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
                    if (service.IsExisting(identity))
                        ModelState.AddModelError("", "Idenity alreday existing");
                    if (ModelState.IsValid)
                    {
                        await service.Create(FirstName, LastName, Convert.ToInt32(Type), UserID, Company, EMail, Language, Table);
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
            log.Debug("Using Edit in {0}", Table);
            ViewData["Title"] = "Edit Identity";
            await BuildMenu();
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            if (id == null)
                return NotFound();
            ViewBag.Types = service.ListActiveIdentityTypes();
            ViewBag.Languages = service.ListAllActiveLanguages();
            string FormSubmit = values["form-submitted"];
            var list = await service.GetByID((int)id);
            Identity identity = list.FirstOrDefault();
            if (identity == null)
                return NotFound();

            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string NewFirstName = values["FirstName"];
                string NewLastName = values["LastName"];
                string NewUserID = values["UserID"];
                string NewCompany = values["Company"];
                string NewType = values["Type.TypeId"];
                string NewLanguage = values["Language.Code"];
                string NewEMail = values["EMail"];
                if (service.IsExisting(identity, NewUserID))
                    ModelState.AddModelError("", "Idenity alreday existing");
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.EditAsync(identity, NewFirstName, NewLastName, Convert.ToInt32(NewType), NewUserID, NewCompany, NewEMail, NewLanguage, Table);
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
            log.Debug("Using Delete in {0}", Table);
            ViewData["Title"] = "Deactivate Identity";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            await BuildMenu();
            if (id == null)
                return NotFound();
            string FormSubmit = values["form-submitted"];
            var list = await service.GetByID((int)id);
            Identity identity = list.FirstOrDefault();
            if (identity == null)
                return NotFound();
            ViewData["backUrl"] = "Identity";
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    await service.Deactivate(identity, ViewData["reason"].ToString(), Table);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    log.Error("DB error: {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(list);
        }
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Identity";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var list = await service.GetByID((int)id);
            Identity identity = list.FirstOrDefault();
            if (identity == null)
                return NotFound();
            if (service.HasAdminAccess(service.Admin, SitePart, "Activate"))
            {
                await service.Activate(identity, Table);
                return RedirectToAction(nameof(Index));
            }
            else
                RedirectToAction(nameof(Index));
            return View();
        }
        public async Task<IActionResult> AssignDevice(IFormCollection values, int? id)
        {
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Assign device to identity";
            ViewData["AssignDevice"] = service.HasAdminAccess(service.Admin, SitePart, "AssignDevice");
            await BuildMenu();
            if (id == null)
                return NotFound();
            string FormSubmit = values["form-submitted"];
            var list = await service.GetByID((int)id);
            Identity identity = list.FirstOrDefault();
            if (identity == null)
                return NotFound();
            ViewBag.Devices = await service.ListAllFreeDevices();
            ViewData["backUrl"] = "Identity";
            if (!String.IsNullOrEmpty(FormSubmit))
            {

            }
            return View(list);
        }
        public async Task<IActionResult> AssignAccount(IFormCollection values, int? id)
        {
            log.Debug("Using Assign Account in {0}", Table);
            ViewData["Title"] = "Assign Account";
            ViewData["AssignAccount"] = service.HasAdminAccess(service.Admin, SitePart, "AssignAccount");
            await BuildMenu();
            if (id == null)
                return NotFound();
            string FormSubmit = values["form-submitted"];
            var list = await service.GetByID((int)id);
            Identity identity = list.FirstOrDefault();
            if (identity == null)
                return NotFound();
            ViewBag.Identity = identity;
            ViewBag.Accounts = await service.ListAllFreeAccounts();
            if (!String.IsNullOrEmpty(FormSubmit))
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
                        await service.AssignAccount2Idenity(identity, AccId, from, until, Table);
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
            log.Debug("Using Release Account in {0}", Table);
            if (id == 0)
                return NotFound();
            ViewData["Title"] = "Release Account";
            var idenAccounts = await service.GetIdenAccountByID(id);
            IdenAccount idenAccount = idenAccounts.FirstOrDefault();
            if (idenAccount == null)
                return NotFound();
            ViewBag.Identity = idenAccount.Identity;
            ViewBag.Account = idenAccount.Account;
            ViewData["ReleaseAccount"] = service.HasAdminAccess(service.Admin, SitePart, "ReleaseAccount");
            await BuildMenu();
            ViewData["backUrl"] = "Identity";
            ViewData["Action"] = "ReleaseAccount";
            ViewData["Name"] = idenAccount.Identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    await service.ReleaseAccount4Identity(idenAccount.Identity, idenAccount.Account, id, Table);
                    PDFGenerator PDFGenerator = new()
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
                    PDFGenerator.GeneratePDF(_env);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
        public async Task<IActionResult> AssignForm(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            log.Debug("Using Assign Form in {0}", Table);
            ViewData["Title"] = "Assign form";
            ViewData["AssignDevice"] = service.HasAdminAccess(service.Admin, SitePart, "AssignDevice");
            ViewData["AssignAccount"] = service.HasAdminAccess(service.Admin, SitePart, "AssignAccount");
            var list = await service.GetByID((int)id);
            Identity identity = list.FirstOrDefault();
            if (identity == null)
                return NotFound();
            service.GetAssingedDevices(list.First());
            service.GetAssignedAccounts(list.First());
            await BuildMenu();
            ViewData["backUrl"] = "Identity";
            ViewData["Action"] = "AssignForm";
            ViewData["Name"] = list.First().Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
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
                if (identity.Accounts.Count > 0)
                {
                    foreach (var account in identity.Accounts)
                        PDFGenerator.SetAccontInfo(account);
                }
                else if (identity.Devices.Count > 0)
                {
                    foreach (Device d in list.First().Devices)
                        PDFGenerator.SetAssetInfo(d);
                }
                PDFGenerator.GeneratePDF(_env);
                return RedirectToAction(nameof(Index));
            }
            return View(list);
        }
    }
}
