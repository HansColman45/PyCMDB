using CMDB.API.Models;
using CMDB.Domain.Entities;
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
    /// The web controller for managing role permissions.
    /// </summary>
    public class RolePermissionController : CMDBController
    {
        private readonly RolePermService service;
        /// <summary>
        /// Constructor for the RolePermisionController.
        /// </summary>
        /// <param name="env"></param>
        public RolePermissionController(IWebHostEnvironment env) : base(env)
        {
            SitePart = "Role permission";
            Table = "rolepermission";
            service = new RolePermService();
        }
        /// <summary>
        /// The index page with the ovierview of the permissions and its role assignments.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", SitePart);
            await BuildMenu();
            ViewData["Title"] = "Role Permission overview";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["actionUrl"] = @"\RolePermission\Search";
            ViewData["Controller"] = @"\RolePermission\Create";
            var permissions = await service.ListAll();
            return View(permissions);
        }
        /// <summary>
        /// The search page which will show all the role permission matching the search string
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            if (!string.IsNullOrEmpty(search))
            {
                ViewData["Title"] = "Role Permission overview";
                await BuildMenu();
                var Desktops = await service.ListAll(search);
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["actionUrl"] = @"\RolePermission\Search";
                ViewData["Controller"] = @"\RolePermission\Create";
                return View(Desktops);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        /// <summary>
        /// Retrieves the details of an entity specified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve. Must be a positive integer.</param>
        /// <returns>An <see cref="IActionResult"/> containing the details of the entity if found;  otherwise, an appropriate
        /// error response, such as a 404 Not Found result.</returns>
        public async Task<IActionResult> Details(int?id)
        {
            if (id is null)
                return NotFound();
            var roleper = await service.GetById((int)id);
            if (roleper is null)
                return NotFound();
            log.Debug("Using details for {0} with id {1}", SitePart, id);
            ViewData["Title"] = "Role Permission details";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            ViewData["Controller"] = @"\RolePermission\Create";
            return View(roleper);
        }
        /// <summary>
        /// Displays the "Create Role Permission" view and initializes the necessary data for the form.
        /// </summary>
        /// <param name="values">The form collection containing submitted values, if any.</param>
        /// <returns>An <see cref="IActionResult"/> that renders the "Create Role Permission" view.</returns>
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using create for {0}", SitePart);
            await BuildMenu();
            ViewData["Title"] = "Create Role Permission";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["Controller"] = @"\RolePermission\Create";
            ViewBag.Permissions = await service.GetAllPermissions();
            ViewBag.Menus = await service.GetAllMenus();
            ViewBag.Levels = service.ListAllLevels();
            RolePermissionDTO rolePermission = new();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                var level = Int32.Parse(values["Level"]);
                var menuid = Int32.Parse(values["Menu"]);
                var permission = Int32.Parse(values["Permission"]);
                Menu menu = await service.GetMenuById(menuid);
                PermissionDTO permissionDTO = await service.GetPermissionById(permission);
                rolePermission.Level = level;
                rolePermission.Menu = menu;
                rolePermission.Permission = permissionDTO;
                if (ModelState.IsValid)
                {
                    await service.Create(rolePermission);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(rolePermission);
        }
        /// <summary>
        /// Handles the editing of a role permission based on the provided form values and role ID.
        /// </summary>
        /// <remarks>This method retrieves the role permission by its ID and populates the necessary data
        /// for the edit view, including permissions, menus, and levels. If the form is submitted and the model state
        /// is valid, the role permission is updated and the user is redirected to the index page.</remarks>
        /// <param name="values">The form collection containing submitted values for the role permission.</param>
        /// <param name="id">The ID of the role permission to edit. Must not be null.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation. Returns <see
        /// cref="NotFoundResult"/> if the role ID is null or the role permission is not found. Returns a view with the
        /// role permission details if the form is not submitted or validation fails. Redirects to the <see
        /// cref="Index"/> action upon successful editing.</returns>
        public async Task<IActionResult> Edit(IFormCollection values,int? id)
        {
            log.Debug("Using edit for {0} with id {1}", SitePart, id);
            if (id is null)
                return NotFound();
            var roleper = await service.GetById((int)id);
            if (roleper is null)
                return NotFound();
            await BuildMenu();
            ViewData["Title"] = "Edit Role Permission";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["Controller"] = @$"\RolePermission\Edit\{id}";
            ViewData["backUrl"] = "RolePermission";
            ViewBag.Permissions = await service.GetAllPermissions();
            ViewBag.Menus = await service.GetAllMenus();
            ViewBag.Levels = service.ListAllLevels();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                var level = Int32.Parse(values["Level"]);
                roleper.Level = level;
                if (ModelState.IsValid)
                {
                    await service.Edit(roleper);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(roleper);
        }
        /// <summary>
        /// Handles the deletion of a role permission based on the provided ID and form submission.
        /// </summary>
        /// <param name="values">The form collection containing submitted data, including the form submission indicator.</param>
        /// <param name="id">The ID of the role permission to delete. Must not be null.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the operation.  Returns <see
        /// cref="NotFoundResult"/> if the ID is null or the role permission is not found.  Returns a redirect to the
        /// <c>Index</c> action upon successful deletion.  Otherwise, returns a view displaying the role permission to
        /// be deleted.</returns>
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            log.Debug("Using delete for {0} with id {1}", SitePart, id);
            if (id is null)
                return NotFound();
            var roleper = await service.GetById((int)id);
            if (roleper is null)
                return NotFound();
            await BuildMenu();
            ViewData["Title"] = "Delete Role Permission";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["Controller"] = @$"\RolePermission\Delete\{id}";
            ViewData["backUrl"] = "RolePermission";
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                if (ModelState.IsValid)
                {
                    await service.Delete(roleper);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(roleper);
        }
    }
}
