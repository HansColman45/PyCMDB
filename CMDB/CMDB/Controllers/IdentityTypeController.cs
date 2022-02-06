using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using CMDB.Domain.Entities;
using CMDB.Services;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    public class IdentityTypeController : CMDBController
    {
        private new readonly IdentityTypeService service;
        public IdentityTypeController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
            Table = "identitytype";
            SitePart = "Identity Type";
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            var list = await service.ListAll();
            ViewData["Title"] = "Identitytype overview";
            await BuildMenu();
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            ViewData["actionUrl"] = @"\IdentityType\Search";
            return View(list);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using List all in {0}", Table);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var list = await service.ListAll(search);
                ViewData["Title"] = "Identitytype overview";
                await BuildMenu();
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
                ViewData["actionUrl"] = @"\IdentityType\Search";
                return View(list);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details in {0}", Table);
            if (id == null)
                return NotFound();
            ViewData["Title"] = "Identitytype Details";
            await BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            var idenTypes = await service.GetByID((int)id);
            var idenType = idenTypes.FirstOrDefault();
            if (idenType == null)
                return NotFound();
            service.GetLogs(Table, (int)id, idenType);
            if (idenType == null)
            {
                return NotFound();
            }
            return View(idenTypes);
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", Table);
            ViewData["Title"] = "Create Identitytype";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            await BuildMenu();
            IdentityType idenType = new();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    idenType.Type = values["Type"];
                    idenType.Description = values["Description"];
                    if (service.IsExisting(idenType))
                        ModelState.AddModelError("", "Idenity type existing");
                    if (ModelState.IsValid)
                    {
                        await service.Create(idenType, Table);
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
            return View(idenType);
        }
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit in {0}", SitePart);
            if (id == null)
                return NotFound();
            ViewData["Title"] = "Edit Identitytype";
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            await BuildMenu();
            var idenTypes = await service.GetByID((int)id);
            var idenType = idenTypes.FirstOrDefault();
            if (idenType == null)
                return NotFound();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newTpe = values["Type"];
                    string newDescription = values["Description"];
                    if (service.IsExisting(idenType, newTpe, newDescription))
                        ModelState.AddModelError("", "Idenity type existing");
                    if (ModelState.IsValid)
                    {
                        _ = service.Update(idenType, newTpe, newDescription, Table);
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
            return View(idenType);
        }
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", SitePart);
            if (id == null)
                return NotFound();
            ViewData["Title"] = "Delete Identitytype";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["backUrl"] = "IdentityType";
            var idenTypes = await service.GetByID((int)id);
            var idenType = idenTypes.FirstOrDefault();
            if (idenType == null)
                return NotFound();
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        _ = service.Deactivate(idenType, values["reason"], Table);
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
            return View(idenTypes);
        }
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Identitytype";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var idenTypes = await service.GetByID((int)id);
            var idenType = idenTypes.FirstOrDefault();
            if (idenType == null)
                return NotFound();
            if (service.HasAdminAccess(service.Admin, SitePart, "Activate"))
            {
                await service.Activate(idenType, Table);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
