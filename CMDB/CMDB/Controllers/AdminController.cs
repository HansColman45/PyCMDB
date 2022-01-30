using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CMDB.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using CMDB.Domain.Entities;
using CMDB.Services;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    public class AdminController : CMDBController
    {
        private readonly static string table = "admin";
        private readonly static string sitePart = "Admin";
        private new readonly AdminService service;
        public AdminController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
        }

        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", table);
            var list = await service.ListAll();
            ViewData["Title"] = "Admin overview";
            await BuildMenu();
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            ViewData["actionUrl"] = @"\Admin\Search";
            return View(list);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using List all in {0}", table);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var list = await service.ListAll(search);
                ViewData["Title"] = "Admin overview";
                await BuildMenu();
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
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", sitePart);
            Admin admin = new();
            ViewData["Title"] = "Create Admin";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewBag.Accounts = await service.ListActiveCMDBAccounts();
            ViewBag.Levels = service.ListAllLevels();
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    var accounts = await service.GetAccountByID(Convert.ToInt32(values["Account"]));
                    admin.Account = accounts.First();
                    admin.Level = Convert.ToInt32(values["Level"]);
                    if (service.IsExisting(admin))
                        ModelState.AddModelError("", "Admin is already existing");
                    if (ModelState.IsValid)
                    {
                        await service.Create(admin, table);
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
            return View(admin);
        }
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit in {0}", sitePart);
            ViewData["Title"] = "Edit Admin";
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewBag.Accounts = await service.ListActiveCMDBAccounts();
            ViewBag.Levels = service.ListAllLevels();
            await BuildMenu();
            if (id == null)
                return NotFound();
            string FormSubmit = values["form-submitted"];
            var admins = await service.GetByID((int)id);
            Admin admin = admins.FirstOrDefault();
            if (admin == null)
                return NotFound();
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    int Level = Convert.ToInt32(values["Level"]);
                    if (ModelState.IsValid)
                    {
                        await service.Update(admin, Level, table);
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
            return View(admin);
        }
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details in {0}", table);
            ViewData["Title"] = "Admin details";
            await BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
                return NotFound();
            var admins = await service.GetByID((int)id);
            Admin admin = admins.FirstOrDefault();
            if (admin == null)
                return NotFound();
            service.GetLogs(table, (int)id, admin);
            return View(admins);
        }
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", sitePart);
            if (id == null)
                return NotFound();
            ViewData["Title"] = "Delete admin";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["backUrl"] = "Admin";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            var admins = await service.GetByID((int)id);
            Admin admin = admins.FirstOrDefault();
            if (admin == null)
                return NotFound();
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        await service.Deactivate(admin, values["reason"].ToString(), table);
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
            return View(admin);
        }
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Admin";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var admins = await service.GetByID((int)id);
            Admin admin = admins.FirstOrDefault();
            if (admin == null)
                return NotFound();
            if (service.HasAdminAccess(service.Admin, sitePart, "Activate"))
            {
                await service.Activate(admin, table);
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
