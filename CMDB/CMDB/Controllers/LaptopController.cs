using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using CMDB.Util;
using CMDB.DbContekst;
using CMDB.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CMDB.Controllers
{
    public class LaptopController : CMDBController
    {
        private readonly CMDBContext _context;
        private readonly ILogger<LaptopController> _logger;
        private readonly static string sitePart = "Laptop";
        private readonly static string table = "devices";
        private readonly IWebHostEnvironment _env;

        public LaptopController(CMDBContext context, ILogger<LaptopController> logger, IWebHostEnvironment env):base(context,logger,env)
        {
            _context = context;
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            _logger.LogDebug("Using List all in {0}", table);
            ViewData["Title"] = "Laptop overview";
            BuildMenu();
            var Desktops = _context.ListAllDevices(sitePart);
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            ViewData["AssignIdentityAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignIdentity");
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            ViewData["actionUrl"] = @"\Laptop\Search";
            return View(Desktops);
        }
        public IActionResult Search(string search)
        {
            _logger.LogDebug("Using search for {0}", sitePart);
            BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                _logger.LogDebug("Using List all in {0}", table);
                ViewData["Title"] = "Laptop overview";
                BuildMenu();
                var Desktops = _context.ListAllDevices(sitePart, search);
                ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
                ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
                ViewData["AssignIdentityAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignIdentity");
                ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
                ViewData["actionUrl"] = @"\Laptop\Search";
                return View(Desktops);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Create(IFormCollection values)
        {
            _logger.LogDebug("Using Create in {0}", sitePart);
            ViewData["Title"] = "Create Laptop";
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            BuildMenu();
            Laptop laptop = new Laptop();
            ViewBag.Types = _context.ListAssetTypes(sitePart);
            ViewBag.Rams = _context.ListRams();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    laptop.AssetTag = values["AssetTag"];
                    laptop.SerialNumber = values["SerialNumber"];
                    laptop.RAM = values["RAM"];
                    int Type = Convert.ToInt32(values["Type"]);
                    var AssetType = _context.ListAssetTypeById(Type);
                    laptop.Type = AssetType.ElementAt<AssetType>(0);
                    laptop.Category = AssetType.ElementAt<AssetType>(0).Cateory;
                    laptop.MAC = values["MAC"];
                    if (_context.IsDeviceExisting(laptop))
                    {
                        ModelState.AddModelError("", "Asset already exist");
                    }
                    if (ModelState.IsValid)
                    {
                        _context.CreateNewLaptop(laptop, table);
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
            return View(laptop);
        }
        public IActionResult Edit(IFormCollection values, string id)
        {
            _logger.LogDebug("Using Edit in {0}", sitePart);
            ViewData["Title"] = "Edit Laptop";
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            BuildMenu();
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            Laptop laptop = _context.ListLaptopByID(id).ElementAt<Laptop>(0);
            ViewBag.AssetTypes = _context.ListAssetTypes(sitePart);
            ViewBag.Rams = _context.ListRams();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newSerialNumber = values["SerialNumber"];
                    string newRam = values["RAM"];
                    int Type = Convert.ToInt32(values["Type.TypeID"]);
                    var newAssetType = _context.ListAssetTypeById(Type).ElementAt<AssetType>(0);
                    string newMAC = values["MAC"];
                    if (ModelState.IsValid)
                    {
                        _context.UpdateLaptop(laptop, newRam, newMAC, newAssetType, newSerialNumber, table);
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
            return View(laptop);
        }
        public IActionResult Details(string id)
        {
            _logger.LogDebug("Using details in {0}", table);
            ViewData["Title"] = "Desktop details";
            BuildMenu();
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["IdentityOverview"] = _context.HasAdminAccess(_context.Admin, sitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = _context.HasAdminAccess(_context.Admin, sitePart, "ReleaseIdentity");
            ViewData["LogDateFormat"] = _context.LogDateFormat;
            ViewData["DateFormat"] = _context.DateFormat;
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            Laptop laptop = _context.ListLaptopByID(id).ElementAt<Laptop>(0);
            _context.GetLogs(table, laptop.AssetTag, laptop);
            ViewBag.Identity = _context.GetIdentityByID(laptop.Identity.IdenID).ElementAt<Identity>(0);
            return View(laptop);
        }
        public IActionResult Delete(IFormCollection values, string id)
        {
            _logger.LogDebug("Using Delete in {0}", sitePart);
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            ViewData["Title"] = "Delete Laptop";
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["backUrl"] = "Admin";
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            Laptop laptop = _context.ListLaptopByID(id).ElementAt<Laptop>(0);
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        _context.DeactivateDevice(laptop, values["reason"], table);
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
            return View(laptop);
        }
        public IActionResult Activate(string id)
        {
            _logger.LogDebug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Laptop";
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            BuildMenu();
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            Laptop laptop = _context.ListLaptopByID(id).ElementAt<Laptop>(0);
            if (_context.HasAdminAccess(_context.Admin, sitePart, "Activate"))
            {
                _context.ActivateDevice(laptop, table);
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
