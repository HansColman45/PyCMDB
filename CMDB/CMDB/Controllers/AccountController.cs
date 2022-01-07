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

namespace CMDB.Controllers
{
    public class AccountController : CMDBController
    {
        private readonly static string sitePart = "Account";
        private readonly static string table = "account";
        private new readonly AccountService service;
        public AccountController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
        }
        public IActionResult Index()
        {
            log.Debug("Using list all for {0}", sitePart);
            BuildMenu();
            var accounts = service.ListAll();
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
        public IActionResult Search(string search)
        {
            log.Debug("Using search for {0}", sitePart);
            BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var accounts = service.ListAll(search);
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
        public IActionResult Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", sitePart);
            ViewData["Title"] = "Create Account";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            BuildMenu();
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
                    AccountType accountType = service.GetAccountTypeByID(Convert.ToInt32(Type)).ElementAt<AccountType>(0);
                    Application application = service.GetApplicationByID(Convert.ToInt32(Application)).ElementAt<Application>(0);
                    account.UserID = UserID;
                    account.Application = application;
                    account.Type = accountType;
                    if (service.IsAccountExisting(account))
                        ModelState.AddModelError("", "Account alreaday exist");
                    if (ModelState.IsValid)
                    {
                        service.CreateNew(UserID, Convert.ToInt32(Type), Convert.ToInt32(Application), table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View();
        }
        public IActionResult Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit in {0}", sitePart);
            ViewData["Title"] = "Edit Account";
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Types = service.ListActiveAccountTypes();
            ViewBag.Applications = service.ListActiveApplications();
            string FormSubmit = values["form-submitted"];
            var accounts = service.GetByID((int)id);
            Account account = accounts.ElementAt<Account>(0);
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
                        service.Edit(account, NewUserID, Convert.ToInt32(Type), Convert.ToInt32(Application), table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(account);
        }
        public IActionResult Details(int? id)
        {
            log.Debug("Using details in {0}", table);
            ViewData["Title"] = "Account Details";
            BuildMenu();
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
            var accounts = service.GetByID((int)id);
            service.GetLogs(table, (int)id, accounts.ElementAt<Account>(0));
            service.GetAssignedIdentitiesForAccount(accounts.ElementAt<Account>(0));
            if (accounts == null)
            {
                return NotFound();
            }
            return View(accounts);
        }
        public IActionResult Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", table);
            ViewData["Title"] = "Deactivate Account";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var accounts = service.GetByID((int)id);
            Account account = accounts.ElementAt<Account>(0);
            if (accounts == null)
            {
                return NotFound();
            }
            string FormSubmit = values["form-submitted"];
            ViewData["backUrl"] = "Account";
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    service.Deactivate(account, ViewData["reason"].ToString(), table);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(accounts);
        }
        public IActionResult Activate(int? id)
        {
            log.Debug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Account";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var accounts = service.GetByID((int)id);
            Account account = accounts.ElementAt<Account>(0);
            if (accounts == null)
            {
                return NotFound();
            }
            if (service.HasAdminAccess(service.Admin, sitePart, "Activate"))
            {
                service.Activate(account, table);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult AssignIdentity(IFormCollection values, int? id)
        {
            log.Debug("Using Assign Identity in {0}", table);
            ViewData["Title"] = "Assign Identity";
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, sitePart, "AssignIdentity");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var accounts = service.GetByID((int)id);
            Account account = accounts.ElementAt<Account>(0);
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
                    service.AssignIdentity2Account(account, IdenID, from, until, table);
                    return RedirectToAction("AssignFrom", "Account", new { id });
                }
            }
            return View();
        }
        public IActionResult ReleaseIdentity(IFormCollection values, int? id)
        {
            log.Debug("Using Assign Identity in {0}", table);
            ViewData["Title"] = "Release Identity";
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, sitePart, "AssignIdentity");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var idenAccount = service.GetIdenAccountByID((int)id);
            ViewData["backUrl"] = "Account";
            ViewData["Action"] = "ReleaseIdentity";
            ViewBag.Identity = idenAccount.ElementAt<IdenAccount>(0).Identity;
            ViewBag.Account = idenAccount.ElementAt<IdenAccount>(0).Account;
            ViewData["Name"] = idenAccount.ElementAt<IdenAccount>(0).Identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            ViewData["ReleaseIdentity"] = service.HasAdminAccess(service.Admin, sitePart, "ReleaseIdentity");
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    service.ReleaseIdentity4Acount(idenAccount.ElementAt<IdenAccount>(0).Account, idenAccount.ElementAt<IdenAccount>(0).Identity, (int)id, table);
                    idenAccount = service.GetIdenAccountByID((int)id);
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
                    PDFGenerator.GeneratePDF(_env);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
        public IActionResult AssignFrom(IFormCollection values, int? id)
        {
            log.Debug("Using Assign Form in {0}", table);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Assign Identity";
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            var accounts = service.GetByID((int)id);
            service.GetLogs(table, (int)id, accounts.ElementAt<Account>(0));
            service.GetAssignedIdentitiesForAccount(accounts.ElementAt<Account>(0));
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            ViewData["backUrl"] = "Account";
            ViewData["Action"] = "AssignFrom";
            ViewData["Name"] = accounts.ElementAt<Account>(0).Identities.ElementAt<IdenAccount>(0).Identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                PDFGenerator PDFGenerator = new()
                {
                    ITEmployee = ITPerson,
                    Singer = Employee,
                    UserID = accounts.ElementAt<Account>(0).UserID,
                    Language = accounts.ElementAt<Account>(0).Identities.ElementAt<IdenAccount>(0).Identity.Language.Code,
                    Receiver = accounts.ElementAt<Account>(0).Identities.ElementAt<IdenAccount>(0).Identity.Name
                };
                PDFGenerator.SetAccontInfo(accounts.ElementAt<Account>(0).Identities.ElementAt<IdenAccount>(0));
                PDFGenerator.GeneratePDF(_env);
            }
            return View(accounts);
        }
    }
}
