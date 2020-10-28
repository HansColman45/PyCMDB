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

namespace CMDB.Controllers
{
    public class AdminController : Controller
    {
        private readonly CMDBContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly static string table = "admin";
        private readonly static string sitePart = "Admin";
        public AdminController(CMDBContext context, ILogger<AdminController> logger)
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
        private void BuildMenu()
        {
            List<Menu> menul1 = (List<Menu>)_context.ListFirstMenuLevel();
            foreach (Menu m in menul1)
            {
                if (m.Children is null)
                    m.Children = new List<Menu>();
                List<Menu> mL2 = (List<Menu>)_context.ListSecondMenuLevel(m.MenuId);
                foreach (Menu m1 in mL2)
                {
                    if (m1.Children is null)
                        m1.Children = new List<Menu>();
                    var mL3 = _context.ListPersonalMenu(_context.Admin.Level, m1.MenuId);
                    foreach (Menu menu in mL3)
                    {
                        m1.Children.Add(new Menu()
                        {
                            MenuId = menu.MenuId,
                            Label = menu.Label,
                            URL = menu.URL
                        });
                    }
                    m.Children.Add(m1);
                }
            }
            ViewBag.Menu = menul1;
        }
    }
}
