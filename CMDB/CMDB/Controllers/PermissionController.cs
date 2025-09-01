using CMDB.Domain.DTOs;
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
    /// Controller for the permission
    /// </summary>
    public class PermissionController : CMDBController
    {
        private readonly PermissionService service;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        public PermissionController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            SitePart = "Permissions";
            Table = "permission";
        }
        /// <summary>
        /// The index page with the ovierview of the permissions
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
            ViewData["Title"] = "Permission overview";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["actionUrl"] = @"\Permission\Search";
            ViewData["Controller"] = @"\Permission\Create";
            var permissions = await service.ListAll();
            return View(permissions);
        }
        /// <summary>
        /// The searrch page
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Searching for {0} in {1}", search, SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (!string.IsNullOrEmpty(search)) { 
                await BuildMenu();
                ViewData["search"] = search;
                ViewData["Title"] = "Permission overview";
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["actionUrl"] = @"\Permission\Search";
                ViewData["Controller"] = @"\Permission\Create";
                var permissions = await service.ListAll(search);
                return View("Index", permissions);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// This will open teh details view for a specific permission
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Details for {0} in {1}", id, SitePart);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id is null)
                return NotFound();
            var roleper = await service.GetById((int)id);
            if (roleper is null)
                return NotFound();
            ViewData["Title"] = "Permission details";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["RorePermOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "RorePermOverview");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            ViewData["Controller"] = @"\Permission\Create";
            ViewBag.rolePerms = await service.GetRolePermissionInfo((int)id);
            return View(roleper);
        }
        /// <summary>
        /// The create page which will create a new Permission
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug($"Using Create in {SitePart}");
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            await BuildMenu();
            ViewData["Title"] = "Create Permssion";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewBag.Types = await service.ListAssetTypes(SitePart);
            ViewData["backUrl"] = "¨Permission";
            ViewData["Controller"] = @"\Permission\Create";
            PermissionDTO permission = new();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    permission.Right = values["Right"];
                    permission.Description = values["Description"];
                    if (ModelState.IsValid)
                    {
                        await service.CreatePermission(permission);
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
            return View(permission);
        }
        /// <summary>
        /// This will open the edit page for a specific permission
        /// </summary>
        /// <param name="id">The Id of the Permision you want to edit</param>
        /// <param name="values">The values from the form</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id, IFormCollection values)
        {
            log.Debug("Using Edit in {0} with id {1}", SitePart, id);
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                log.Error("Unauthourized acces");
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            if (id is null)
                return NotFound();
            var permission = await service.GetById((int)id);
            if (permission is null)
                return NotFound();
            await BuildMenu();
            ViewData["Title"] = "Edit Permission";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["backUrl"] = "¨Permission";
            ViewData["Controller"] = @$"\Permission\Edit\{id}";
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    permission.Right = values["Right"];
                    permission.Description = values["Description"];
                    if (ModelState.IsValid)
                    {
                        await service.UpdatePermission(permission);
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
            return View(permission);
        }
    }
}
