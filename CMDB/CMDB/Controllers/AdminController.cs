using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CMDB.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using CMDB.Domain.Entities;
using CMDB.Services;

namespace CMDB.Controllers
{
    public class AdminController : CMDBController
    {
        private readonly ILogger<AdminController> _logger;
        private readonly static string table = "admin";
        private readonly static string sitePart = "Admin";
        private new readonly AdminService service;
        public AdminController(CMDBContext context, ILogger<AdminController> logger, IWebHostEnvironment env) : base(context, logger, env)
        {
            _logger = logger;
            service = new(context);
        }

        public IActionResult Index()
        {
            _logger.LogDebug("Using List all in {0}", table);
            var list = service.ListAll();
            ViewData["Title"] = "Admin overview";
            BuildMenu();
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            ViewData["actionUrl"] = @"\Admin\Search";
            return View(list);
        }
        public IActionResult Search(string search)
        {
            _logger.LogDebug("Using List all in {0}", table);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var list = service.ListAll(search);
                ViewData["Title"] = "Admin overview";
                BuildMenu();
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
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
            Admin admin = new();
            ViewData["Title"] = "Create Admin";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewBag.Accounts = service.ListActiveCMDBAccounts();
            ViewBag.Levels = service.ListAllLevels();
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    admin.Account = service.GetAccountByID(Convert.ToInt32(values["Account"])).First();
                    admin.Level = Convert.ToInt32(values["Level"]);
                    if (service.IsExisting(admin))
                        ModelState.AddModelError("", "Admin is already existing");
                    if (ModelState.IsValid)
                    {
                        service.Create(admin, table);
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
            return View(admin);
        }
        public IActionResult Edit(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Edit in {0}", sitePart);
            ViewData["Title"] = "Edit Admin";
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewBag.Accounts = service.ListActiveCMDBAccounts();
            ViewBag.Levels = service.ListAllLevels();
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            string FormSubmit = values["form-submitted"];
            Admin admin = service.GetByID((int)id).ElementAt<Admin>(0);
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    int Level = Convert.ToInt32(values["Level"]);
                    if (ModelState.IsValid)
                    {
                        service.Update(admin, Level, table);
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
            return View(admin);
        }
        public IActionResult Details(int? id)
        {
            _logger.LogDebug("Using details in {0}", table);
            ViewData["Title"] = "Admin details";
            BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
            {
                return NotFound();
            }
            var Admins = service.GetByID((int)id);
            service.GetLogs(table, (int)id, Admins.ElementAt<Admin>(0));
            return View(Admins);
        }
        public IActionResult Delete(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Delete in {0}", sitePart);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Delete admin";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["backUrl"] = "Admin";
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            Admin admin = service.GetByID((int)id).ElementAt<Admin>(0);
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        service.Deactivate(admin, values["reason"].ToString(), table);
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
            return View(admin);
        }
        public IActionResult Activate(int? id)
        {
            _logger.LogDebug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Admin";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            Admin admin = service.GetByID((int)id).ElementAt<Admin>(0);
            if (service.HasAdminAccess(service.Admin, sitePart, "Activate"))
            {
                service.Activate(admin, table);
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
