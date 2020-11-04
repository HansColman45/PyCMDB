using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CMDB.DbContekst;
using CMDB.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting;

namespace CMDB.Controllers
{
    public class AdminController : CMDBController
    {
        private readonly CMDBContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly static string table = "admin";
        private readonly static string sitePart = "Admin";

        public AdminController(CMDBContext context, ILogger<AdminController> logger, IWebHostEnvironment env) : base(context, logger, env)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogDebug("Using List all in {0}", table);
            var list = _context.ListAllAdmins();
            ViewData["Title"] = "Admin overview";
            BuildMenu();
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            ViewData["actionUrl"] = @"\Admin\Search";
            return View(list);
        }
        public IActionResult Search(string search)
        {
            _logger.LogDebug("Using List all in {0}", table);
            if (!String.IsNullOrEmpty(search)) {
                ViewData["search"] = search;
                var list = _context.ListAllAdmins(search);
                ViewData["Title"] = "Admin overview";
                BuildMenu();
                ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
                ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
                ViewData["actionUrl"] = @"\Admin\Search";
                return View(list);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Create(IFormCollection values)
        {
            _logger.LogDebug("Using Create in {0}", sitePart);
            Admin admin = new Admin();
            ViewData["Title"] = "Create Admin";
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewBag.Accounts = _context.ListActiveCMDBAccounts();
            ViewBag.Levels = _context.ListAllLevels();
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    admin.Account = _context.GetAccountByID(Convert.ToInt32(values["Account"])).ElementAt<Account>(0);
                    admin.Level = Convert.ToInt32(values["Level"]);
                    if (ModelState.IsValid)
                    {
                        _context.CreateNewAdmin(admin, table);
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
            return View(admin);
        }
        public IActionResult Details(int? id)
        {
            _logger.LogDebug("Using details in {0}", table);
            ViewData["Title"] = "Admin details";
            BuildMenu();
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["LogDateFormat"] = _context.LogDateFormat;
            ViewData["DateFormat"] = _context.DateFormat;
            if (id == null)
            {
                return NotFound();
            }
            var Admins = _context.GetAdminByID((int)id);
            _context.GetLogs(table, (int)id, Admins.ElementAt<Admin>(0));
            return View(Admins);
        }
    }
}
