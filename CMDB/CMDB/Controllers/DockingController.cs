using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using CMDB.Domain.Entities;
using CMDB.Services;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    public class DockingController : CMDBController
    {
        private new readonly DevicesService service;

        public DockingController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
            SitePart = "Docking station";
            Table = "docking";
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            ViewData["Title"] = "Docking overview";
            await BuildMenu();
            var Desktops = await service.ListAll(SitePart);
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            ViewData["AssignIdentityAccess"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
            ViewData["actionUrl"] = @"\Docking\Search";
            return View(Desktops);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["Title"] = "Docking overview";
                await BuildMenu();
                var Desktops = await service.ListAll(SitePart, search);
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
                ViewData["AssignIdentityAccess"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
                ViewData["actionUrl"] = @"\Docking\Search";
                return View(Desktops);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> Delete(IFormCollection values, string id)
        {
            log.Debug("Using Delete in {0}", SitePart);
            if (id == null)
                return NotFound();
            ViewData["Title"] = "Delete Docking station";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["backUrl"] = "Admin";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            var dockings = await service.ListDockingByID(id);
            Docking docking = dockings.FirstOrDefault();
            if (docking == null)
                return NotFound();
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        await service.Deactivate(docking, values["reason"], Table);
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
            return View(docking);
        }
        public async Task<IActionResult> Activate(string id)
        {
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Docking station";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            await BuildMenu();
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var dockings = await service.ListDockingByID(id);
            Docking docking = dockings.FirstOrDefault();
            if (docking == null)
                return NotFound();
            if (service.HasAdminAccess(service.Admin, SitePart, "Activate"))
            {
                await service.Activate(docking, Table);
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
