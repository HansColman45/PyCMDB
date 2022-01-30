using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Util;
using Microsoft.AspNetCore.Hosting;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Services;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    public class AccountController : CMDBController
    {
        private new readonly AccountService service;
        public AccountController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
            sitePart = "Account";
            table = "account";
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", sitePart);
            await BuildMenu();
            var accounts = await service.ListAll();
            ViewData["Title"] = "Account overview";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, sitePart, "AssignIdentity");
            ViewData["actionUrl"] = @"\Account\Search";
            return View(accounts);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", sitePart);
            await BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var accounts = await service.ListAll(search);
                ViewData["Title"] = "Account overview";
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
                ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, sitePart, "AssignIdentity");
                ViewData["actionUrl"] = @"\Account\Search";
                return View(accounts);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", sitePart);
            ViewData["Title"] = "Create Account";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            await BuildMenu();
            Account account = new();
            ViewBag.Types = service.ListActiveAccountTypes();
            ViewBag.Applications = service.ListActiveApplications();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string UserID = values["UserID"];
                    ViewData["UserID"] = UserID;
                    string Type = values["type"];
                    string Application = values["Application"];
                    AccountType accountType = service.GetAccountTypeByID(Convert.ToInt32(Type)).First();
                    Application application = service.GetApplicationByID(Convert.ToInt32(Application)).First();
                    account.UserID = UserID;
                    account.Application = application;
                    account.Type = accountType;
                    if (service.IsAccountExisting(account))
                        ModelState.AddModelError("", "Account alreaday exist");
                    if (ModelState.IsValid)
                    {
                        await service.CreateNew(UserID, Convert.ToInt32(Type), Convert.ToInt32(Application), table);
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
            return View();
        }
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit in {0}", sitePart);
            ViewData["Title"] = "Edit Account";
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            await BuildMenu();
            if (id == null)
                return NotFound();
            ViewBag.Types = service.ListActiveAccountTypes();
            ViewBag.Applications = service.ListActiveApplications();
            string FormSubmit = values["form-submitted"];
            var accounts = await service.GetByID((int)id);
            Account account = accounts.FirstOrDefault();
            if (account == null)
                NotFound();
            ViewData["UserID"] = account.UserID;
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string NewUserID = values["UserID"];
                    string Type = values["Type.TypeID"];
                    string Application = values["Application.AppID"];
                    if (service.IsAccountExisting(account, NewUserID, Convert.ToInt32(Type)))
                        ModelState.AddModelError("", "Account alreaday exist");
                    if (ModelState.IsValid)
                    {
                        await service.Edit(account, NewUserID, Convert.ToInt32(Type), Convert.ToInt32(Application), table);
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
            return View(account);
        }
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details in {0}", table);
            ViewData["Title"] = "Account Details";
            await BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["IdentityOverview"] = service.HasAdminAccess(service.Admin, sitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, sitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = service.HasAdminAccess(service.Admin, sitePart, "ReleaseIdentity");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
            {
                return NotFound();
            }
            var accounts = await service.GetByID((int)id);
            Account account = accounts.FirstOrDefault();
            if (account == null)
                NotFound();
            service.GetLogs(table, (int)id, account);
            service.GetAssignedIdentitiesForAccount(account);
            if (accounts == null)
            {
                return NotFound();
            }
            return View(accounts);
        }
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", table);
            ViewData["Title"] = "Deactivate Account";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var accounts = await service.GetByID((int)id);
            Account account = accounts.FirstOrDefault();
            if (accounts == null)
                return NotFound();
            string FormSubmit = values["form-submitted"];
            ViewData["backUrl"] = "Account";
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    await service.Deactivate(account, ViewData["reason"].ToString(), table);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(accounts);
        }
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Account";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var accounts = await service.GetByID((int)id);
            Account account = accounts.FirstOrDefault();
            if (account == null)
                NotFound();
            if (service.HasAdminAccess(service.Admin, sitePart, "Activate"))
            {
                await service.Activate(account, table);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> AssignIdentity(IFormCollection values, int? id)
        {
            log.Debug("Using Assign Identity in {0}", table);
            ViewData["Title"] = "Assign Identity";
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, sitePart, "AssignIdentity");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var accounts = await service.GetByID((int)id);
            Account account = accounts.FirstOrDefault();
            if (account == null)
                NotFound();
            ViewBag.Account = account;
            ViewBag.Identities = service.ListAllFreeIdentities();
            if (accounts == null)
            {
                return NotFound();
            }
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                int IdenID = Convert.ToInt32(values["Identity"]);
                DateTime from = DateTime.Parse(values["ValidFrom"]);
                DateTime until = DateTime.Parse(values["ValidUntil"]);
                service.IsPeriodOverlapping(null, id, from, until);
                if (ModelState.IsValid)
                {
                    await service.AssignIdentity2Account(account, IdenID, from, until, table);
                    return RedirectToAction("AssignFrom", "Account", new { id });
                }
            }
            return View();
        }
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, int? id)
        {
            log.Debug("Using Assign Identity in {0}", table);
            ViewData["Title"] = "Release Identity";
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, sitePart, "AssignIdentity");
            await BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var idenAccount = await service.GetIdenAccountByID((int)id);
            ViewData["backUrl"] = "Account";
            ViewData["Action"] = "ReleaseIdentity";
            ViewBag.Identity = idenAccount.First().Identity;
            ViewBag.Account = idenAccount.First().Account;
            ViewData["Name"] = idenAccount.First().Identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            ViewData["ReleaseIdentity"] = service.HasAdminAccess(service.Admin, sitePart, "ReleaseIdentity");
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    await service.ReleaseIdentity4Acount(idenAccount.First().Account, idenAccount.First().Identity, (int)id, table);
                    idenAccount = await service.GetIdenAccountByID((int)id);
                    PDFGenerator PDFGenerator = new()
                    {
                        ITEmployee = ITPerson,
                        Singer = Employee,
                        UserID = idenAccount.First().Identity.UserID,
                        Language = idenAccount.First().Identity.Language.Code,
                        Receiver = idenAccount.First().Identity.Name,
                        Type = "Release"
                    };
                    PDFGenerator.SetAccontInfo(idenAccount.First());
                    PDFGenerator.GeneratePDF(_env);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
        public async Task<IActionResult> AssignFrom(IFormCollection values, int? id)
        {
            log.Debug("Using Assign Form in {0}", table);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Assign Identity";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            var accounts = await service.GetByID((int)id);
            service.GetLogs(table, (int)id, accounts.First());
            service.GetAssignedIdentitiesForAccount(accounts.First());
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            ViewData["backUrl"] = "Account";
            ViewData["Action"] = "AssignFrom";
            ViewData["Name"] = accounts.First().Identities.First().Identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                PDFGenerator PDFGenerator = new()
                {
                    ITEmployee = ITPerson,
                    Singer = Employee,
                    UserID = accounts.First().UserID,
                    Language = accounts.First().Identities.First().Identity.Language.Code,
                    Receiver = accounts.First().Identities.First().Identity.Name
                };
                PDFGenerator.SetAccontInfo(accounts.First().Identities.First());
                PDFGenerator.GeneratePDF(_env);
            }
            return View(accounts);
        }
    }
}
