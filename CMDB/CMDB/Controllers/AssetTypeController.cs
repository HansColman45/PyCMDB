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
using CMDB.Models;
using Microsoft.AspNetCore.Http;

namespace CMDB.Controllers
{
    public class AssetTypeController : CMDBController
    {
        private readonly CMDBContext _context;
        private readonly ILogger<AssetTypeController> _logger;
        private readonly static string sitePart = "Asset Type";
        private readonly static string table = "assettype";
        private readonly IWebHostEnvironment _env;

        public AssetTypeController(CMDBContext context, ILogger<AssetTypeController> logger, IWebHostEnvironment env):base(context, logger,env)
        {
            _context = context;
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            _logger.LogDebug("Using list all for {0}", sitePart);
            BuildMenu();
            var accounts = _context.ListAllAssetTypes();
            ViewData["Title"] = "Assettype overview";
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            ViewData["actionUrl"] = @"\AssetType\Search";
            return View(accounts);
        }
        public IActionResult Search(string search)
        {
            _logger.LogDebug("Using search for {0}", sitePart);
            BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var accounts = _context.ListAllAssetTypes(search);
                ViewData["Title"] = "Assettype overview";
                ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
                ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
                ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
                ViewData["actionUrl"] = @"\AssetType\Search";
                return View(accounts);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        public IActionResult Create(IFormCollection values)
        {
            _logger.LogDebug("Using Create in {0}", sitePart);
            ViewData["Title"] = "Create assettype";
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            AssetType assetType = new AssetType();
            BuildMenu();
            ViewBag.Catgories = _context.ListActiveCategories();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    assetType.Vendor = values["Vendor"];
                    assetType.Type = values["Type"];
                    int id = Convert.ToInt32(values["Cateory"]);
                    var category = _context.ListAssetCategoryByID(id);
                    assetType.Cateory = category.ElementAt<AssetCategory>(0);
                    if (_context.IsAssetTypeExisting(assetType))
                    {
                        ModelState.AddModelError("", "Asset type already exist");
                    }
                    if (ModelState.IsValid)
                    {
                        _context.CreateNewAssetType(assetType, table);
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
            return View(assetType);
        }
        public IActionResult Edit(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Edit in {0}", sitePart);
            ViewData["Title"] = "Edit assettype";
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            BuildMenu();
            if(id == null)
            {
                return NotFound();
            }
            AssetType assetType = _context.ListAssetTypeById((int)id).ElementAt<AssetType>(0);
            ViewBag.Catgories = _context.ListActiveCategories();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newVendor = values["Vendor"];
                    string newType = values["Type"];
                    if (_context.IsAssetTypeExisting(assetType, newVendor,newType))
                    {
                        ModelState.AddModelError("", "Asset type already exist");
                    }
                    if (ModelState.IsValid)
                    {
                        _context.UpdateAssetType(assetType, newVendor, newType,table);
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
            return View(assetType);
        }
        public IActionResult Details(int? id)
        {
            _logger.LogDebug("Using details in {0}", table);
            ViewData["Title"] = "Assettype details";
            BuildMenu();
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["LogDateFormat"] = _context.LogDateFormat;
            ViewData["DateFormat"] = _context.DateFormat;
            if (id == null)
            {
                return NotFound();
            }
            AssetType assetType = _context.ListAssetTypeById((int)id).ElementAt<AssetType>(0);
            _context.GetLogs(table, assetType.TypeID, assetType);
            return View(assetType);
        }
        public IActionResult Delete(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Delete in {0}", table);
            ViewData["Title"] = "Deactivate Assettype";
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            AssetType assetType = _context.ListAssetTypeById((int)id).ElementAt<AssetType>(0);
            string FormSubmit = values["form-submitted"];
            ViewData["backUrl"] = "AssetType";
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        _context.DeactivateAssetType(assetType, values["reason"].ToString(), table);
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
            return View(assetType);
        }
        public IActionResult Activate(int? id)
        {
            _logger.LogDebug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Account";
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            AssetType assetType = _context.ListAssetTypeById((int)id).ElementAt<AssetType>(0);
            if (_context.HasAdminAccess(_context.Admin, sitePart, "Activate"))
            {
                _context.ActivateAssetType(assetType, table);
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
