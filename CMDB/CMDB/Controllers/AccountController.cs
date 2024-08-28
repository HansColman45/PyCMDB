using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Services;
using CMDB.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    public class AccountController : CMDBController
    {
        private new readonly AccountService service;
        public AccountController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
            SitePart = "Account";
            Table = "account";
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", SitePart);
            await BuildMenu();
            var accounts = await service.ListAll();
            ViewData["Title"] = "Account overview";
            ViewData["Controller"] = @"\Account\Create";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["actionUrl"] = @"\Account\Search";
            return View(accounts);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            await BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var accounts = await service.ListAll(search);
                ViewData["Title"] = "Account overview";
                ViewData["Controller"] = @"\Account\Create";
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
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
            log.Debug("Using Create in {0}", SitePart);
            ViewData["Title"] = "Create Account";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["Controller"] = @"\Account\Create";
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
                    /*AccountType accountType = service.GetAccountTypeByID(Convert.ToInt32(Type)).First();
                    Application application = service.GetApplicationByID(Convert.ToInt32(Application)).First();
                    account.UserID = UserID;
                    account.Application = application;
                    account.Type = accountType;
                    if (service.IsAccountExisting(account))
                        ModelState.AddModelError("", "Account alreaday exist");*/
                    try
                    {
                        await service.CreateNew(UserID, Convert.ToInt32(Type), Convert.ToInt32(Application), Table);
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("API Error", e.Message);
                        throw;
                    }
                    if (ModelState.IsValid)
                        return RedirectToAction(nameof(Index));
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
            log.Debug("Using Edit in {0}", SitePart);
            ViewData["Title"] = "Edit Account";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
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
                    string Type = values["Type.TypeId"];
                    string Application = values["Application.AppID"];
                    if (service.IsAccountExisting(account, NewUserID, Convert.ToInt32(Type)))
                        ModelState.AddModelError("", "Account alreaday exist");
                    if (ModelState.IsValid)
                    {
                        await service.Edit(account, NewUserID, Convert.ToInt32(Type), Convert.ToInt32(Application), Table);
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
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = "Account Details";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["IdentityOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
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
            service.GetLogs(Table, (int)id, account);
            service.GetAssignedIdentitiesForAccount(account);
            if (accounts == null)
            {
                return NotFound();
            }
            return View(accounts);
        }
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", Table);
            ViewData["Title"] = "Deactivate Account";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
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
                    await service.Deactivate(account, ViewData["reason"].ToString(), Table);
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
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Account";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var accounts = await service.GetByID((int)id);
            Account account = accounts.FirstOrDefault();
            if (account == null)
                NotFound();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.Activate(account, Table);
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
            log.Debug("Using Assign Identity in {0}", Table);
            ViewData["Title"] = "Assign Identity";
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var accounts = await service.GetByID((int)id);
            Account account = accounts.FirstOrDefault();
            if (account == null)
                NotFound();
            ViewBag.Account = account;
            ViewBag.Identities = await service.ListAllFreeIdentities();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                int IdenID = Convert.ToInt32(values["Identity"]);
                DateTime from = DateTime.Parse(values["ValidFrom"]);
                DateTime until = DateTime.Parse(values["ValidUntil"]);
                service.IsPeriodOverlapping((int)id, from, until);
                if (ModelState.IsValid)
                {
                    await service.AssignIdentity2Account(account, IdenID, from, until, Table);
                    return RedirectToAction("AssignForm", "Account", new { id });
                }
            }
            return View();
        }
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, int? id)
        {
            log.Debug("Using Assign Identity in {0}", Table);
            ViewData["Title"] = "Release Identity";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var idenAccounts = await service.GetIdenAccountByID((int)id);
            IdenAccount idenAccount = idenAccounts.FirstOrDefault();
            if (idenAccount == null)
                return NotFound();
            ViewData["backUrl"] = "Account";
            ViewData["Action"] = "ReleaseIdentity";
            ViewBag.Identity = idenAccount.Identity;
            ViewBag.Account = idenAccount.Account;
            ViewData["Name"] = idenAccount.Identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
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
                        UserID = idenAccount.Identity.UserID,
                        FirstName = idenAccount.Identity.FirstName,
                        LastName = idenAccount.Identity.LastName,
                        Language = idenAccount.Identity.Language.Code,
                        Receiver = idenAccount.Identity.Name,
                        Type = "Release"
                    };
                    PDFGenerator.SetAccontInfo(idenAccount);
                    string pdfFile = PDFGenerator.GeneratePath(_env);
                    PDFGenerator.GeneratePdf(pdfFile);
                    await service.ReleaseIdentity4Acount(idenAccount.Account, idenAccount.Identity, (int)id, Table, pdfFile);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
        public async Task<IActionResult> AssignForm(IFormCollection values, int? id)
        {
            log.Debug("Using Assign Form in {0}", Table);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Assign Identity";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            var accounts = await service.GetByID((int)id);
            Account account = accounts.First();
            service.GetAssignedIdentitiesForAccount(account);
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            ViewData["backUrl"] = "Account";
            ViewData["Action"] = "AssignForm";
            ViewData["Name"] = account.Identities.Last().Identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                PDFGenerator PDFGenerator = new()
                {
                    ITEmployee = ITPerson,
                    Singer = Employee,
                    UserID = account.Identities.Last().Identity.UserID,
                    FirstName = account.Identities.Last().Identity.FirstName,
                    LastName = account.Identities.Last().Identity.LastName,
                    Language = account.Identities.Last().Identity.Language.Code,
                    Receiver = account.Identities.Last().Identity.Name
                };
                PDFGenerator.SetAccontInfo(account.Identities.First());
                string pdfFile = PDFGenerator.GeneratePath(_env);
                PDFGenerator.GeneratePdf(pdfFile);
                await service.LogPdfFile(Table, account, pdfFile);
                return RedirectToAction(nameof(Index));
            }
            return View(accounts);
        }
    }
}
