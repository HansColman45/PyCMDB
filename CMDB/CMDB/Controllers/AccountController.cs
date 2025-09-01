﻿using CMDB.Domain.DTOs;
using CMDB.Infrastructure;
using CMDB.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    /// <summary>
    /// Controller for Account
    /// </summary>
    public class AccountController : CMDBController
    {
        private readonly AccountService service;
        private readonly PDFService PDFservice;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        public AccountController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            PDFservice = new();
            SitePart = "Account";
            Table = "account";
        }
        /// <summary>
        /// This will return the view with all the accounts
        /// </summary>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", SitePart);
            if(string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
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
        /// <summary>
        /// This will return the view with all the account based on the search string
        /// </summary>
        /// <param name="search">The searchstring</param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
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
        /// <summary>
        /// This will return the view to create a new account
        /// </summary>
        /// <param name="values">The values from the form</param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            ViewData["Title"] = "Create Account";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["Controller"] = @"\Account\Create";
            await BuildMenu();
            ViewBag.Types = await service.ListActiveAccountTypes();
            ViewBag.Applications = await service.ListActiveApplications();
            string FormSubmit = values["form-submitted"];
            AccountDTO accountDTO = new();
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string UserID = values["UserID"];
                    ViewData["UserID"] = UserID;
                    string Type = values["type"];
                    string Application = values["Application"];
                    var app = await service.GetApplicationByID(Convert.ToInt32(Application));
                    var type = await service.GetAccountTypeByID(Convert.ToInt32(Type));
                    accountDTO = new()
                    {
                        Active = 1,
                        UserID = UserID,
                        ApplicationId = Convert.ToInt32(Application),
                        TypeId = Convert.ToInt32(Type),
                        Type = type,
                        Application = app
                    };
                    if (await service.IsAccountExisting(accountDTO))
                        ModelState.AddModelError("", "Account alreaday exist");
                    if (ModelState.IsValid) { 
                        await service.CreateNew(accountDTO);
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
            return View(accountDTO);
        }
        /// <summary>
        /// This will return the view to edit an account
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit in {0}", SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id == null)
                return NotFound();
            var account = await service.GetByID((int)id);
            if (account == null)
                NotFound();
            ViewData["Title"] = "Edit Account";
            ViewData["Controller"] = @$"\Account\Edit\{id}";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            await BuildMenu();
            ViewBag.Types = await service.ListActiveAccountTypes();
            ViewBag.Applications = await service.ListActiveApplications();
            string FormSubmit = values["form-submitted"];
            ViewData["UserID"] = account.UserID;
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string NewUserID = values["UserID"];
                    string Type = values["Type.TypeId"];
                    string Application = values["Application.AppID"];
                    if (await service.IsAccountExisting(account, NewUserID,Convert.ToInt32(Application), Convert.ToInt32(Type)))
                        ModelState.AddModelError("", "Account alreaday exist");
                    if (ModelState.IsValid)
                    {
                        await service.Edit(account, NewUserID, Convert.ToInt32(Type), Convert.ToInt32(Application));
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
        /// <summary>
        /// This will return the view with the details of an account
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id == null)
            {
                return NotFound();
            }
            var account = await service.GetByID((int)id);
            if (account == null)
                return NotFound();
            ViewData["Title"] = "Account Details";
            ViewData["Controller"] = @"\Account\Create";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["IdentityOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            return View(account);
        }
        /// <summary>
        /// This will return the view to delete an account
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id == null)
                return NotFound();
            var account = await service.GetByID((int)id);
            if (account == null)
                return NotFound();
            ViewData["Title"] = "Deactivate Account";
            ViewData["Controller"] = @$"\Account\Delete\{id}"; 
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            ViewData["backUrl"] = "Account";
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    await service.Deactivate(account, ViewData["reason"].ToString());
                    return RedirectToAction(nameof(Index));
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
        /// <summary>
        /// This will activate an account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using Activate in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id == null)
                return NotFound();
            var account = await service.GetByID((int)id);
            if (account == null)
                NotFound();
            ViewData["Title"] = "Activate Account";
            ViewData["Controller"] = @$"\Account\Activate\{id}";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.Activate(account);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View();
        }
        /// <summary>
        /// This will assign an identity to an account
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AssignIdentity(IFormCollection values, int? id)
        {
            log.Debug("Using Assign Identity in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id == null)
                return NotFound();
            var account = await service.GetByID((int)id);
            if (account == null)
                NotFound();
            ViewData["Title"] = "Assign Identity";
            ViewData["Controller"] = @$"\Account\AssignIdentity\{id}";
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            await BuildMenu();
            ViewBag.Account = account;
            ViewBag.Identities = await service.ListAllFreeIdentities();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                int IdenID = Convert.ToInt32(values["Identity"]);
                DateTime from = DateTime.Parse(values["ValidFrom"]);
                DateTime until = DateTime.Parse(values["ValidUntil"]);
                service.IsPeriodOverlapping((int)id, from, until);
                if (ModelState.IsValid)
                {
                    await service.AssignIdentity2Account(account, IdenID, from, until);
                    return RedirectToAction("AssignForm", "Account", new { id });
                }
            }
            return View();
        }
        /// <summary>
        /// This will release an identity from an account
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, int? id)
        {
            log.Debug("Using Assign Identity in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id == null)
                return NotFound();
            IdenAccountDTO idenAccount = await service.GetIdenAccountByID((int)id);
            if (idenAccount == null)
                return NotFound();
            ViewData["Title"] = "Release Identity";
            ViewData["Controller"] = @$"\Account\ReleaseIdentity\{id}";
            await BuildMenu();
            ViewData["backUrl"] = "Account";
            ViewData["Action"] = "ReleaseIdentity";
            ViewBag.Identity = idenAccount.Identity;
            ViewBag.Account = idenAccount.Account;
            ViewData["Name"] = idenAccount.Identity.Name;
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    await PDFservice.SetUserinfo(idenAccount.Identity.UserID, 
                        ITPerson,
                        Employee,
                        idenAccount.Identity.FirstName,
                        idenAccount.Identity.LastName,
                        idenAccount.Identity.Name, 
                        idenAccount.Identity.Language.Code,
                        "Release");
                    await PDFservice.SetAccontInfo(idenAccount);
                    await PDFservice.GenratePDFFile(Table,idenAccount.Account.AccID);
                    await service.ReleaseIdentity4Acount(idenAccount);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
        /// <summary>
        /// This will open the Assign Form
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> AssignForm(IFormCollection values, int? id)
        {
            log.Debug("Using Assign Form in {0}", Table);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id == null)
                return NotFound();
            var account = await service.GetByID((int)id);
            if (account == null)
                return NotFound();
            ViewData["Title"] = "Assign Identity";
            ViewData["Controller"] = @$"\Account\AssignForm\{id}";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            ViewData["backUrl"] = "Account";
            ViewData["Action"] = "AssignForm";
            ViewData["Name"] = account.Identities.Last().Identity.Name;
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                await PDFservice.SetUserinfo(
                    account.Identities.Last().Identity.UserID, 
                    ITPerson, 
                    Employee, 
                    account.Identities.Last().Identity.FirstName,
                    account.Identities.Last().Identity.LastName,
                    account.Identities.Last().Identity.Name, 
                    account.Identities.Last().Identity.Language.Code);
                await PDFservice.SetAccontInfo(account.Identities.Last());
                await PDFservice.GenratePDFFile(Table, account.AccID);
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }
    }
}
