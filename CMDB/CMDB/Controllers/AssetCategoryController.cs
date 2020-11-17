using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using CMDB.Util;
using CMDB.DbContekst;
using Microsoft.AspNetCore.Hosting;
using Aspose.Html.Net;
using Microsoft.AspNetCore.Http;
using CMDB.Models;
using System.Diagnostics;

namespace CMDB.Controllers
{
    public class AssetCategoryController : CMDBController
    {
        private readonly CMDBContext _context;
        private readonly ILogger<AssetCategoryController> _logger;
        private readonly static string sitePart = "Asset Category";
        private readonly static string table = "assetcategory";
        private readonly IWebHostEnvironment _env;
        public AssetCategoryController(CMDBContext context, ILogger<AssetCategoryController> logger, IWebHostEnvironment env) : base(context, logger, env)
        {
            _context = context;
            _logger = logger;
            _env = env;
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Using list all for {0}", sitePart);
            BuildMenu();
            ViewData["Title"] = "Category overview";
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            ViewData["actionUrl"] = @"\AssetCategory\Search";
            var Categories = _context.ListAllAssetCategories();
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
                ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
                ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
                ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
                ViewData["actionUrl"] = @"\AssetCategory\Search";
                var Categories = _context.ListAllAssetCategories(search);
                return View(Categories);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        public IActionResult Create(IFormCollection values)
        {
            _logger.LogDebug("Using Edit in {0}", sitePart);
            ViewData["Title"] = "Create category";
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            BuildMenu();
            AssetCategory category = new AssetCategory();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    category.Category = values["Category"];
                    category.Prefix = values["Prefix"];
                    if (_context.IsAssetCategoryExisting(category))
                        ModelState.AddModelError("", "Assetcategory alreaday exist");
                    if (ModelState.IsValid)
                    {
                        _context.CreateAssetCategory(category, table);
                        _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (MySqlException ex)
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
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            AssetCategory category = _context.ListAssetCategoryByID((int)id).ElementAt<AssetCategory>(0);
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string Category = values["Category"];
                    string Prefix = values["Prefix"];
                    if (_context.IsAssetCategoryExisting(category, Category))
                        ModelState.AddModelError("", "Assetcategory alreaday exist");
                    if (ModelState.IsValid)
                    {
                        _context.UpdateAssetCategory(category, Category, Prefix, table);
                        _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (MySqlException ex)
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
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["LogDateFormat"] = _context.LogDateFormat;
            ViewData["DateFormat"] = _context.DateFormat;
            if (id == null)
            {
                return NotFound();
            }
            AssetCategory category = _context.ListAssetCategoryByID((int)id).ElementAt<AssetCategory>(0);
            _context.GetLogs(table, category.ID, category);
            return View(category);
        }
        public IActionResult Delete(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Delete in {0}", table);
            ViewData["Title"] = "Deactivate Account";
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            AssetCategory category = _context.ListAssetCategoryByID((int)id).ElementAt<AssetCategory>(0);
            string FormSubmit = values["form-submitted"];
            ViewData["backUrl"] = "Account";
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    if (ModelState.IsValid)
                    {
                        _context.DeactivateAssetCategory(category, values["reason"], table);
                        _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (MySqlException ex)
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
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            AssetCategory category = _context.ListAssetCategoryByID((int)id).ElementAt<AssetCategory>(0);
            if (_context.HasAdminAccess(_context.Admin, sitePart, "Activate"))
            {
                _context.ActivateAssetCategory(category, table);
                _context.SaveChangesAsync();
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
