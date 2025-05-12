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
    /// The controller for the asset category
    /// </summary>
    public class AssetCategoryController : CMDBController
    {
        private readonly AssetCategoryService service;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="env"></param>
        public AssetCategoryController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            SitePart = "Asset Category";
            Table = "assetcategory";
        }
        /// <summary>
        /// Retruns the index page with all categories
        /// </summary>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", SitePart);
            await BuildMenu();
            ViewData["Title"] = "Category overview";
            ViewData["Controller"] = @"\AssetCategory\Create";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["actionUrl"] = @"\AssetCategory\Search";
            var Categories = await service.ListAll();
            return View(Categories);
        }
        /// <summary>
        /// This will return the search page with the search results
        /// </summary>
        /// <param name="search"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            await BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                ViewData["Title"] = "Category overview";
                ViewData["Controller"] = @"\AssetCategory\Create";
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["actionUrl"] = @"\AssetCategory\Search";
                var Categories = await service.ListAll(search);
                return View(Categories);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// This will open the view to create a new category
        /// </summary>
        /// <param name="values"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Edit in {0}", SitePart);
            ViewData["Title"] = "Create category";
            ViewData["Controller"] = @"\AssetCategory\Create";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            await BuildMenu();
            AssetCategoryDTO category = new();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    category.Category = values["Category"];
                    category.Prefix = values["Prefix"];
                    if (await service.IsExisting(category))
                        ModelState.AddModelError("", "Assetcategory alreaday exist");
                    if (ModelState.IsValid)
                    {
                        await service.Create(category);
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
            return View(category);
        }
        /// <summary>
        /// This will open the view to edit a category
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit in {0}", SitePart);
            if (id == null)
                return NotFound();
            var category = await service.GetByID((int)id); ;
            if (category == null)
                return NotFound();
            ViewData["Title"] = "Edit Account";
            ViewData["Controller"] = @$"\AssetCategory\Edit\{id}";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string Category = values["Category"];
                    string Prefix = values["Prefix"];
                    if (await service.IsExisting(category, Category))
                        ModelState.AddModelError("", "Assetcategory alreaday exist");
                    if (ModelState.IsValid)
                    {
                        await service.Update(category, Category, Prefix);
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
            return View(category);
        }
        /// <summary>
        /// This will open the view to show the details of a category
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details in {0}", Table);
            if (id == null)
                return NotFound();
            var category = await service.GetByID((int)id);
            if (category == null)
                return NotFound();
            ViewData["Title"] = "Category Details";
            ViewData["Controller"] = @"\AssetCategory\Create";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            return View(category);
        }
        /// <summary>
        /// This will open the view to delete a category
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", Table);
            if (id == null)
                return NotFound();
            var category = await service.GetByID((int)id);
            if (category == null)
                return NotFound();
            ViewData["Title"] = "Deactivate Account";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["Controller"] = @$"\AssetCategory\Delete\{id}";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            ViewData["backUrl"] = "Account";
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.Deactivate(category, values["reason"]);
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
            return View(category);
        }
        /// <summary>
        /// This will open the view to activate a category
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ViewResult"/></returns>
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using Activate in {0}", Table);
            if (id == null)
                return NotFound();
            var category = await service.GetByID((int)id);
            if (category == null)
                return NotFound();
            ViewData["Title"] = "Activate Category";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.Activate(category);
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
