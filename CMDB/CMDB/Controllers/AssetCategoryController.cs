using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using CMDB.Domain.Entities;
using CMDB.Services;

namespace CMDB.Controllers
{
    public class AssetCategoryController : CMDBController
    {
        private readonly ILogger<AssetCategoryController> _logger;
        private readonly static string sitePart = "Asset Category";
        private readonly static string table = "assetcategory";
        private readonly IWebHostEnvironment _env;
        private new readonly AssetCategoryService service;
        public AssetCategoryController(CMDBContext context, ILogger<AssetCategoryController> logger, IWebHostEnvironment env) : base(context, logger, env)
        {
            _logger = logger;
            _env = env;
            service = new(context);
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Using list all for {0}", sitePart);
            BuildMenu();
            ViewData["Title"] = "Category overview";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewData["actionUrl"] = @"\AssetCategory\Search";
            var Categories = service.ListAll();
            return View(Categories);
        }
        public IActionResult Search(string search)
        {
            _logger.LogDebug("Using search for {0}", sitePart);
            BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                ViewData["Title"] = "Category overview";
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
                ViewData["actionUrl"] = @"\AssetCategory\Search";
                var Categories = service.ListAll(search);
                return View(Categories);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        public IActionResult Create(IFormCollection values)
        {
            _logger.LogDebug("Using Edit in {0}", sitePart);
            ViewData["Title"] = "Create category";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            BuildMenu();
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
                        service.Create(category, table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    _logger.LogError("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(category);
        }
        public IActionResult Edit(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Edit in {0}", sitePart);
            ViewData["Title"] = "Edit Account";
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            AssetCategory category = service.ListByID((int)id).ElementAt<AssetCategory>(0);
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
                        service.Update(category, Category, Prefix, table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    _logger.LogError("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }

            }
            return View(category);
        }
        public IActionResult Details(int? id)
        {
            _logger.LogDebug("Using details in {0}", table);
            ViewData["Title"] = "Category Details";
            BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
            {
                return NotFound();
            }
            AssetCategory category = service.ListByID((int)id).ElementAt<AssetCategory>(0);
            service.GetLogs(table, category.Id, category);
            return View(category);
        }
        public IActionResult Delete(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Delete in {0}", table);
            ViewData["Title"] = "Deactivate Account";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            AssetCategory category = service.ListByID((int)id).ElementAt<AssetCategory>(0);
            string FormSubmit = values["form-submitted"];
            ViewData["backUrl"] = "Account";
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    if (ModelState.IsValid)
                    {
                        service.Deactivate(category, values["reason"], table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    _logger.LogError("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(category);
        }
        public IActionResult Activate(int? id)
        {
            _logger.LogDebug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Category";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            AssetCategory category = service.ListByID((int)id).ElementAt<AssetCategory>(0);
            if (service.HasAdminAccess(service.Admin, sitePart, "Activate"))
            {
                service.Activate(category, table);
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
