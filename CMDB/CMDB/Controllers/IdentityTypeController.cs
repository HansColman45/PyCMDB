using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using CMDB.Domain.Entities;
using CMDB.Services;

namespace CMDB.Controllers
{
    public class IdentityTypeController : CMDBController
    {
        private readonly static string table = "identitytype";
        private readonly static string sitePart = "Identity Type";
        private new readonly IdentityTypeService service;
        public IdentityTypeController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
        }
        public IActionResult Index()
        {
            log.Debug("Using List all in {0}", table);
            var list = service.ListAll();
            ViewData["Title"] = "Identitytype overview";
            BuildMenu();
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewData["actionUrl"] = @"\IdentityType\Search";
            return View(list);
        }
        public IActionResult Search(string search)
        {
            log.Debug("Using List all in {0}", table);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var list = service.ListAll(search);
                ViewData["Title"] = "Identitytype overview";
                BuildMenu();
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
                ViewData["actionUrl"] = @"\IdentityType\Search";
                return View(list);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Details(int? id)
        {
            log.Debug("Using details in {0}", table);
            ViewData["Title"] = "Identitytype Details";
            BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
            {
                return NotFound();
            }
            var idenType = service.GetByID((int)id);
            service.GetLogs(table, (int)id, idenType.ElementAt<IdentityType>(0));
            if (idenType == null)
            {
                return NotFound();
            }
            return View(idenType);
        }
        public IActionResult Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", table);
            ViewData["Title"] = "Create Identitytype";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            BuildMenu();
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
                        service.Create(idenType, table);
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
        public IActionResult Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit in {0}", sitePart);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Edit Identitytype";
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            BuildMenu();
            var idenType = service.GetByID((int)id);
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newTpe = values["Type"];
                    string newDescription = values["Description"];
                    if (service.IsExisting(idenType.ElementAt<IdentityType>(0), newTpe, newDescription))
                        ModelState.AddModelError("", "Idenity type existing");
                    if (ModelState.IsValid)
                    {
                        service.Update(idenType.ElementAt<IdentityType>(0), newTpe, newDescription, table);
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
            return View(idenType.ElementAt<IdentityType>(0));
        }
        public IActionResult Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", sitePart);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Delete Identitytype";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["backUrl"] = "IdentityType";
            var idenType = service.GetByID((int)id);
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        service.Deactivate(idenType.ElementAt<IdentityType>(0), values["reason"], table);
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
        public IActionResult Activate(int? id)
        {
            log.Debug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Identitytype";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var idenType = service.GetByID((int)id);
            if (service.HasAdminAccess(service.Admin, sitePart, "Activate"))
            {
                service.Activate(idenType.ElementAt<IdentityType>(0), table);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
