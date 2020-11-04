using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMDB.DbContekst;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using CMDB.Models;
using CMDB.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CMDB.Controllers
{
    public class DesktopController : CMDBController
    {
        private readonly CMDBContext _context;
        private readonly ILogger<DesktopController> _logger;
        private readonly static string sitePart = "Desktop";
        private readonly static string table = "devices";
        private readonly IWebHostEnvironment _env;

        public DesktopController(CMDBContext context, ILogger<DesktopController> logger, IWebHostEnvironment env) : base(context, logger, env)
        {
            _context = context;
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            _logger.LogDebug("Using List all in {0}", table);
            ViewData["Title"] = "Desktop overview";
            BuildMenu();
            var Desktops = _context.ListAllDevices(sitePart);
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            ViewData["AssignIdentityAccess"] =  _context.HasAdminAccess(_context.Admin, sitePart, "AssignIdentity");
            ViewData["actionUrl"] = @"\Desktop\Search";
            return View(Desktops);
        }
        public IActionResult Search(string search)
        {
            _logger.LogDebug("Using search for {0}", sitePart);
            BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                _logger.LogDebug("Using List all in {0}", table);
                ViewData["Title"] = "Desktop overview";
                BuildMenu();
                var Desktops = _context.ListAllDevices(sitePart, search);
                ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
                ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
                ViewData["AssignIdentityAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignIdentity");
                ViewData["actionUrl"] = @"\Desktop\Search";
                return View(Desktops);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
