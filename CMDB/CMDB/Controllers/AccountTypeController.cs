using CMDB.API.Models;
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
    /// Controller for AccountType
    /// </summary>
    public class AccountTypeController : CMDBController
    {
        private readonly AccountTypeService service;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        public AccountTypeController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            SitePart = "Account Type";
            Table = "accounttype";
        }
        /// <summary>
        /// This wii return the view with all AccountTypes
        /// </summary>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", SitePart);
            await BuildMenu();
            var types = await service.ListAll();
            ViewData["Title"] = "Accounttype overview";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["actionUrl"] = @"\AccountType\Search";
            ViewData["Controller"] = @"\AccountType\Create";
            return View(types);
        }
        /// <summary>
        /// This will search for specific accounttypes
        /// </summary>
        /// <param name="search">>The search string</param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            await BuildMenu();
            if (!string.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var types = await service.ListAll(search);
                ViewData["Title"] = "Accounttype overview";
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["actionUrl"] = @"\AccountType\Search";
                ViewData["Controller"] = @"\AccountType\Create";
                return View(types);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        /// <summary>
        /// THis will open the form to create a new accounttype
        /// </summary>
        /// <param name="values"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", SitePart);
            ViewData["Title"] = "Create Accounttype";
            ViewData["Controller"] = @"\AccountType\Create";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            TypeDTO type = new();
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    type = new()
                    {
                        Type = values["Type"],
                        Description = values["Description"]
                    };
                    if (await service.IsExisting(type))
                        ModelState.AddModelError("", "Account type existing");
                    if (ModelState.IsValid)
                    {
                        await service.Create(type);
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
            return View(type);
        }
        /// <summary>
        /// This will open the form to edit an accounttype
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit in {0}", SitePart);
            if (id == null)
                return NotFound();
            var accountType = await service.GetAccountTypeByID((int)id);
            if (accountType == null)
                return NotFound();
            ViewData["Title"] = "Edit Accounttype";
            ViewData["Controller"] = @$"\AccountType\Edit\{id}";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newType = values["Type"];
                    string newDescription = values["Description"];
                    if (await service.IsExisting(accountType, newType, newDescription))
                        ModelState.AddModelError("", "Account type already exist");
                    if (ModelState.IsValid)
                    {
                        await service.Update(accountType, newType, newDescription);
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
            return View(accountType);
        }
        /// <summary>
        /// This will open the form to delete an accounttype
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", SitePart);
            if (id == null)
                return NotFound();
            var accountType = await service.GetAccountTypeByID((int)id);
            if (accountType == null)
                return NotFound();
            ViewData["Title"] = "Delete Accounttype";
            ViewData["Controller"] = @$"\AccountType\Delete\{id}";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["backUrl"] = "AccountType";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        await service.Deactivate(accountType, ViewData["reason"].ToString());
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
            return View(accountType);
        }
        /// <summary>
        /// This will activate an accounttype
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using Activate in {0}", Table);
            if (id == null)
                return NotFound();
            var accountType = await service.GetAccountTypeByID((int)id);
            if (accountType == null)
                return NotFound();
            ViewData["Title"] = "Activate Accounttype";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.Activate(accountType);
                return RedirectToAction(nameof(Index));
            }
            else
                RedirectToAction(nameof(Index));
            return View();
        }
        /// <summary>
        /// This will open the form with the details of an accounttype
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details in {0}", Table);
            if (id == null)
                return NotFound();
            var accountType = await service.GetAccountTypeByID((int)id);
            if (accountType == null)
                return NotFound();
            ViewData["Title"] = "Accounttype details";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            ViewData["Controller"] = @"\AccountType\Create";
            return View(accountType);
        }
    }
}
