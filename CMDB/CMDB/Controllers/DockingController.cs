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
using Microsoft.AspNetCore.Http;
using CMDB.Models;

namespace CMDB.Controllers
{
    public class DockingController : CMDBController
    {
        private readonly CMDBContext _context;
        private readonly ILogger<DockingController> _logger;
        private readonly static string sitePart = "Docking station";
        private readonly static string table = "devices";
        private readonly IWebHostEnvironment _env;

        public DockingController(CMDBContext context, ILogger<DockingController> logger, IWebHostEnvironment env): base(context, logger, env)
        {
            _logger = logger;
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Using List all in {0}", table);
            ViewData["Title"] = "Docking overview";
            BuildMenu();
            var Desktops = _context.ListAllDevices(sitePart);
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            ViewData["AssignIdentityAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignIdentity");
            ViewData["actionUrl"] = @"\Docking\Search";
            return View(Desktops);
        }
        public IActionResult Search(string search)
        {
            _logger.LogDebug("Using search for {0}", sitePart);
            BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                _logger.LogDebug("Using List all in {0}", table);
                ViewData["Title"] = "Docking overview";
                BuildMenu();
                var Desktops = _context.ListAllDevices(sitePart, search);
                ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
                ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
                ViewData["AssignIdentityAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignIdentity");
                ViewData["actionUrl"] = @"\Docking\Search";
                return View(Desktops);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Delete(IFormCollection values, string id)
        {
            _logger.LogDebug("Using Delete in {0}", sitePart);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Delete Docking station";
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["backUrl"] = "Admin";
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            Docking docking = _context.ListDockingByID(id).ElementAt<Docking>(0);
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        _context.DeactivateDevice(docking, values["reason"], table);
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
            return View(docking);
        }
        public IActionResult Activate(string id)
        {
            _logger.LogDebug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Docking station";
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            BuildMenu();
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            Docking docking = _context.ListDockingByID(id).ElementAt<Docking>(0);
            if (_context.HasAdminAccess(_context.Admin, sitePart, "Activate"))
            {
                _context.ActivateDevice(docking, table);
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
