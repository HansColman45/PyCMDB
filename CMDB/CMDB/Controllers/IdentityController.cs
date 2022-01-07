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
        private readonly IWebHostEnvironment env;
        private readonly static string table = "identity";
        private readonly static string sitePart = "Identity";
        private new readonly IdentityServices service;
        public IdentityController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            this.env = env;
            service = new(context);
        }
        public IActionResult Index()
        {
            log.Debug("Using List all in {0}", table);
            var list = service.ListAll();
            ViewData["Title"] = "Identity overview";
            BuildMenu();
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewData["AssignAccountAccess"] = service.HasAdminAccess(service.Admin, sitePart, "AssignAccount");
            ViewData["AssignDeviceAccess"] = service.HasAdminAccess(service.Admin, sitePart, "AssignDevice");
            ViewData["actionUrl"] = @"\Identity\Search";
            return View(list);
        }
        public IActionResult Search(string search)
        {
            log.Debug("Using search in {0}", table);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var list = service.ListAll(search);
                ViewData["Title"] = "Identity overview";
                BuildMenu();
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
                ViewData["AssignAccountAccess"] = service.HasAdminAccess(service.Admin, sitePart, "AssignAccount");
                ViewData["AssignDeviceAccess"] = service.HasAdminAccess(service.Admin, sitePart, "AssignDevice");
                ViewData["actionUrl"] = @"\Identity\Search";
                return View(list);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Details(int? id)
        {
            log.Debug("Using details in {0}", table);
            ViewData["Title"] = "Identity Details";
            BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["AccountOverview"] = service.HasAdminAccess(service.Admin, sitePart, "AccountOverview");
            ViewData["DeviceOverview"] = service.HasAdminAccess(service.Admin, sitePart, "DeviceOverview");
            ViewData["AssignDevice"] = service.HasAdminAccess(service.Admin, sitePart, "AssignDevice");
            ViewData["AssignAccount"] = service.HasAdminAccess(service.Admin, sitePart, "AssignAccount");
            ViewData["ReleaseAccount"] = service.HasAdminAccess(service.Admin, sitePart, "ReleaseAccount");
            ViewData["ReleaseDevice"] = service.HasAdminAccess(service.Admin, sitePart, "ReleaseDevice");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
            {
                return NotFound();
            }
            var list = service.GetByID((int)id);
            service.GetAssingedDevices(list.ElementAt<Identity>(0));
            service.GetAssignedAccounts(list.ElementAt<Identity>(0));
            service.GetLogs(table, (int)id, list.ElementAt<Identity>(0));
            if (list == null)
            {
                return NotFound();
            }
            return View(list);
        }
        public IActionResult Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", table);
            ViewData["Title"] = "Create Identity";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            BuildMenu();
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
                        service.Create(FirstName, LastName, Convert.ToInt32(Type), UserID, Company, EMail, Language, table);
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
            log.Debug("Using Edit in {0}", table);
            ViewData["Title"] = "Edit Identity";
            BuildMenu();
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Types = service.ListActiveIdentityTypes();
            ViewBag.Languages = service.ListAllActiveLanguages();
            string FormSubmit = values["form-submitted"];
            var list = service.GetByID((int)id);
            Identity identity = list.ElementAt<Identity>(0);
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string NewFirstName = values["FirstName"];
                string NewLastName = values["LastName"];
                string NewUserID = values["UserID"];
                string NewCompany = values["Company"];
                string NewType = values["Type.TypeID"];
                string NewLanguage = values["Language.Code"];
                string NewEMail = values["EMail"];
                if (service.IsExisting(identity, NewUserID))
                    ModelState.AddModelError("", "Idenity alreday existing");
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.EditAsync(identity, NewFirstName, NewLastName, Convert.ToInt32(NewType), NewUserID, NewCompany, NewEMail, NewLanguage, table);
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
        public IActionResult Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", table);
            ViewData["Title"] = "Deactivate Identity";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            string FormSubmit = values["form-submitted"];
            var list = service.GetByID((int)id);
            Identity identity = list.ElementAt<Identity>(0);
            ViewData["backUrl"] = "Identity";
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    service.Deactivate(identity, ViewData["reason"].ToString(), table);
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
        public IActionResult Activate(int? id)
        {
            log.Debug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Identity";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var list = service.GetByID((int)id);
            Identity identity = list.ElementAt<Identity>(0);
            if (service.HasAdminAccess(service.Admin, sitePart, "Activate"))
            {
                service.Activate(identity, table);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult AssignDevice(IFormCollection values, int? id)
        {
            log.Debug("Using Activate in {0}", table);
            ViewData["Title"] = "Assign device to identity";
            ViewData["AssignDevice"] = service.HasAdminAccess(service.Admin, sitePart, "AssignDevice");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            string FormSubmit = values["form-submitted"];
            var list = service.GetByID((int)id);
            if (list == null)
            {
                return NotFound();
            }
            Identity identity = list.ElementAt<Identity>(0);
            ViewBag.Devices = service.ListAllFreeDevices();
            ViewData["backUrl"] = "Identity";
            if (!String.IsNullOrEmpty(FormSubmit))
            {
            }
            return View(list);
        }
        public IActionResult AssignAccount(IFormCollection values, int? id)
        {
            log.Debug("Using Assign Account in {0}", table);
            ViewData["Title"] = "Assign Account";
            ViewData["AssignAccount"] = service.HasAdminAccess(service.Admin, sitePart, "AssignAccount");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            string FormSubmit = values["form-submitted"];
            var list = service.GetByID((int)id);
            if (list == null)
            {
                return NotFound();
            }
            Identity identity = list.ElementAt<Identity>(0);
            ViewBag.Identity = identity;
            ViewBag.Accounts = service.ListAllFreeAccounts();
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                int AccId = Convert.ToInt32(values["Account"]);
                DateTime from = DateTime.Parse(values["ValidFrom"]);
                DateTime until = DateTime.Parse(values["ValidUntil"]);
                try
                {
                    if (service.IsPeriodOverlapping(id, null, from, until))
                        ModelState.AddModelError("", "Periods are overlapping please choose other dates");
                    if (ModelState.IsValid)
                    {
                        service.AssignAccount2Idenity(identity, AccId, from, until, table);
                        return RedirectToAction("AssignFrom", "Identity", new { id });
                    }
                }
                catch (Exception ex)
                {
                    log.Error("DB error: {0}", ex.ToString());
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View();
        }
        public IActionResult ReleaseAccount(IFormCollection values, int id)
        {
            log.Debug("Using Release Account in {0}", table);
            if (id == 0)
            {
                return NotFound();
            }
            ViewData["Title"] = "Release Account";
            var idenAccount = service.GetIdenAccountByID(id);
            ViewBag.Identity = idenAccount.ElementAt<IdenAccount>(0).Identity;
            ViewBag.Account = idenAccount.ElementAt<IdenAccount>(0).Account;
            ViewData["ReleaseAccount"] = service.HasAdminAccess(service.Admin, sitePart, "ReleaseAccount");
            BuildMenu();
            ViewData["backUrl"] = "Identity";
            ViewData["Action"] = "ReleaseAccount";
            ViewData["Name"] = idenAccount.ElementAt<IdenAccount>(0).Identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    service.ReleaseAccount4Identity(idenAccount.ElementAt<IdenAccount>(0).Identity, idenAccount.ElementAt<IdenAccount>(0).Account, id, table);
                    idenAccount = service.GetIdenAccountByID(id);
                    PDFGenerator PDFGenerator = new()
                    {
                        ITEmployee = ITPerson,
                        Singer = Employee,
                        UserID = idenAccount.ElementAt<IdenAccount>(0).Identity.UserID,
                        Language = idenAccount.ElementAt<IdenAccount>(0).Identity.Language.Code,
                        Receiver = idenAccount.ElementAt<IdenAccount>(0).Identity.Name,
                        Type = "Release"
                    };
                    PDFGenerator.SetAccontInfo(idenAccount.ElementAt<IdenAccount>(0));
                    PDFGenerator.GeneratePDF(env);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
        public IActionResult AssignFrom(IFormCollection values, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            log.Debug("Using Assign Form in {0}", table);
            ViewData["Title"] = "Assign form";
            ViewData["AssignDevice"] = service.HasAdminAccess(service.Admin, sitePart, "AssignDevice");
            ViewData["AssignAccount"] = service.HasAdminAccess(service.Admin, sitePart, "AssignAccount");
            var list = service.GetByID((int)id);
            service.GetAssingedDevices(list.ElementAt<Identity>(0));
            service.GetAssignedAccounts(list.ElementAt<Identity>(0));
            BuildMenu();
            ViewData["backUrl"] = "Identity";
            ViewData["Action"] = "AssignFrom";
            ViewData["Name"] = list.ElementAt<Identity>(0).Name;
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
                    UserID = list.ElementAt<Identity>(0).UserID,
                    Language = list.ElementAt<Identity>(0).Language.Code,
                    Receiver = list.ElementAt<Identity>(0).Name
                };
                if (list.ElementAt<Identity>(0).Accounts.Count > 0)
                    PDFGenerator.SetAccontInfo(list.ElementAt<Identity>(0).Accounts.ElementAt<IdenAccount>(0));
                else if (list.ElementAt<Identity>(0).Laptops.Count > 0)
                {
                    foreach (Device d in list.ElementAt<Identity>(0).Laptops)
                        PDFGenerator.SetAssetInfo(d);
                }
                else if (list.ElementAt<Identity>(0).Desktops.Count > 0)
                {
                    foreach (Device d in list.ElementAt<Identity>(0).Desktops)
                        PDFGenerator.SetAssetInfo(d);
                }
                else if (list.ElementAt<Identity>(0).Dockings.Count > 0)
                {
                    foreach (Device d in list.ElementAt<Identity>(0).Dockings)
                        PDFGenerator.SetAssetInfo(d);
                }
                else if (list.ElementAt<Identity>(0).Screens.Count > 0)
                {
                    foreach (Device d in list.ElementAt<Identity>(0).Screens)
                        PDFGenerator.SetAssetInfo(d);
                }
                else if (list.ElementAt<Identity>(0).Tokens.Count > 0)
                {
                    foreach (Device d in list.ElementAt<Identity>(0).Tokens)
                        PDFGenerator.SetAssetInfo(d);
                }
                PDFGenerator.GeneratePDF(env);
                return RedirectToAction(nameof(Index));
            }
            return View(list);
        }
    }
}
