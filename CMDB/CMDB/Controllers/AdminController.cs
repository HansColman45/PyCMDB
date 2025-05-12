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
    /// The Admin controller
    /// </summary>
    public class AdminController : CMDBController
    {
        private readonly AdminService service;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        public AdminController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            Table = "admin";
            SitePart = "Admin";
        }
        /// <summary>
        /// This will return the view with the list of all admins
        /// </summary>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            var list = await service.ListAll();
            ViewData["Title"] = "Admin overview";
            await BuildMenu();
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["actionUrl"] = @"\Admin\Search";
            ViewData["Controller"] = @"\Admin\Create";
            return View(list);
        }
        /// <summary>
        /// This will open the view with a list of admins based on the search string
        /// </summary>
        /// <param name="search">The search string</param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using List all in {0}", Table);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var list = await service.ListAll(search);
                ViewData["Title"] = "Admin overview";
                await BuildMenu();
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["actionUrl"] = @"\Admin\Search";
                ViewData["Controller"] = @"\Admin\Create";
                return View(list);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        /// <summary>
        /// This will open the view to create a new admin
        /// </summary>
        /// <param name="values"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", SitePart);
            AdminDTO admin = new();
            ViewData["Title"] = "Create Admin";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["Controller"] = @"\Admin\Create";
            ViewBag.Accounts = await service.ListActiveCMDBAccounts();
            ViewBag.Levels = service.ListAllLevels();
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    var account = await service.GetAccountByID(Convert.ToInt32(values["Account"]));
                    admin.Account = account;
                    admin.Level = Convert.ToInt32(values["Level"]);
                    if (service.IsExisting(admin))
                        ModelState.AddModelError("", "Admin is already existing");
                    if (ModelState.IsValid)
                    {
                        await service.Create(admin);
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
            return View(admin);
        }
        /// <summary>
        /// This will open the view to edit an admin
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit in {0}", SitePart);
            ViewData["Title"] = "Edit Admin";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewBag.Accounts = await service.ListActiveCMDBAccounts();
            ViewBag.Levels = service.ListAllLevels();
            await BuildMenu();
            if (id == null)
                return NotFound();
            string FormSubmit = values["form-submitted"];
            var admin = await service.GetByID((int)id);
            if (admin == null)
                return NotFound();
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    int Level = Convert.ToInt32(values["Level"]);
                    if (ModelState.IsValid)
                    {
                        await service.Update(admin, Level);
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
            return View(admin);
        }
        /// <summary>
        /// This will open the view to show the details of an admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = "Admin details";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
                return NotFound();
            var admin = await service.GetByID((int)id);
            if (admin == null)
                return NotFound();
            return View(admin);
        }
        /// <summary>
        /// This will open the view to delete an admin
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns><see cref="Delegate"/></returns>
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", SitePart);
            if (id == null)
                return NotFound();
            ViewData["Title"] = "Delete admin";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["backUrl"] = "Admin";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            var admin = await service.GetByID((int)id);
            if (admin == null)
                return NotFound();
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        await service.Deactivate(admin, values["reason"].ToString());
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
            return View(admin);
        }
        /// <summary>
        /// This will activate an admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Admin";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var admin = await service.GetByID((int)id);
            if (admin == null)
                return NotFound();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.Activate(admin);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
