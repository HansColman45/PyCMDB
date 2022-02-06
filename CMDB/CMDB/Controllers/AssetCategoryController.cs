using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using CMDB.Domain.Entities;
using CMDB.Services;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    public class AssetCategoryController : CMDBController
    {
        private new readonly AssetCategoryService service;
        public AssetCategoryController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
            SitePart = "Asset Category";
            Table = "assetcategory";
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", SitePart);
            await BuildMenu();
            ViewData["Title"] = "Category overview";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            ViewData["actionUrl"] = @"\AssetCategory\Search";
            var Categories = service.ListAll();
            return View(Categories);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            await BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                ViewData["Title"] = "Category overview";
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
                ViewData["actionUrl"] = @"\AssetCategory\Search";
                var Categories = await service.ListAll(search);
                return View(Categories);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Edit in {0}", SitePart);
            ViewData["Title"] = "Create category";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            await BuildMenu();
            AssetCategory category = new();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    category.Category = values["Category"];
                    category.Prefix = values["Prefix"];
                    if (service.IsExisting(category))
                        ModelState.AddModelError("", "Assetcategory alreaday exist");
                    if (ModelState.IsValid)
                    {
                        await service.Create(category, Table);
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
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit in {0}", SitePart);
            ViewData["Title"] = "Edit Account";
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var categories = await service.ListByID((int)id);
            AssetCategory category = categories.FirstOrDefault();
            if (category == null)
                return NotFound();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string Category = values["Category"];
                    string Prefix = values["Prefix"];
                    if (service.IsExisting(category, Category))
                        ModelState.AddModelError("", "Assetcategory alreaday exist");
                    if (ModelState.IsValid)
                    {
                        await service.Update(category, Category, Prefix, Table);
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
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = "Category Details";
            await BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
                return NotFound();
            var categories = await service.ListByID((int)id);
            AssetCategory category = categories.FirstOrDefault();
            if (category == null)
                return NotFound();
            service.GetLogs(Table, category.Id, category);
            return View(category);
        }
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", Table);
            ViewData["Title"] = "Deactivate Account";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var categories = await service.ListByID((int)id);
            AssetCategory category = categories.FirstOrDefault();
            if (category == null)
                return NotFound();
            string FormSubmit = values["form-submitted"];
            ViewData["backUrl"] = "Account";
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.Deactivate(category, values["reason"], Table);
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
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Category";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var categories = await service.ListByID((int)id);
            AssetCategory category = categories.FirstOrDefault();
            if (category == null)
                return NotFound();
            if (service.HasAdminAccess(service.Admin, SitePart, "Activate"))
            {
                await service.Activate(category, Table);
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
