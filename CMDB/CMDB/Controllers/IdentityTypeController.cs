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
using CMDB.API.Models;

namespace CMDB.Controllers
{
    public class IdentityTypeController : CMDBController
    {
        private readonly IdentityTypeService service;
        public IdentityTypeController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            Table = "identitytype";
            SitePart = "Identity Type";
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            var list = await service.ListAll();
            ViewData["Title"] = "Identitytype overview";
            ViewData["Controller"] = @"\Identitytype\Create";
            await BuildMenu();
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["actionUrl"] = @"\IdentityType\Search";
            return View(list);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using List all in {0}", Table);
            if (!string.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var list = await service.ListAll(search);
                ViewData["Title"] = "Identitytype overview";
                ViewData["Controller"] = @"\Identitytype\Create";
                await BuildMenu();
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["actionUrl"] = @"\IdentityType\Search";
                return View(list);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details in {0}", Table);
            if (id == null)
                return NotFound();
            var idenType = await service.GetByID((int)id);
            if (idenType == null)
                return NotFound();
            ViewData["Title"] = "Identitytype Details";
            await BuildMenu();
            ViewData["Controller"] = @"\Identitytype\Create";
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            return View(idenType);
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", Table);
            ViewData["Title"] = "Create Identitytype";
            ViewData["Controller"] = @"\Identitytype\Create";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            await BuildMenu();
            TypeDTO idenType = new();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    idenType.Type = values["Type"];
                    idenType.Description = values["Description"];
                    if (await service.IsExisting(idenType))
                        ModelState.AddModelError("", "Idenity type existing");
                    if (ModelState.IsValid)
                    {
                        await service.Create(idenType);
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
            var idenType = await service.GetByID((int)id);
            if (idenType == null)
                return NotFound();
            ViewData["Title"] = "Edit Identitytype";
            ViewData["Controller"] = @$"\Identitytype\Edit\{id}";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newTpe = values["Type"];
                    string newDescription = values["Description"];
                    if (await service.IsExisting(idenType, newTpe, newDescription))
                        ModelState.AddModelError("", "Idenity type existing");
                    if (ModelState.IsValid)
                    {
                        await service.Update(idenType, newTpe, newDescription);
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
            var idenType = await service.GetByID((int)id);
            if (idenType == null)
                return NotFound();
            ViewData["Title"] = "Delete Identitytype";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["backUrl"] = "IdentityType";
            ViewData["Controller"] = @$"\Identitytype\Delete\{id}";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        await service.Deactivate(idenType, values["reason"]);
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
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using Activate in {0}", Table);
            if (id == null)
                return NotFound();
            var idenType = await service.GetByID((int)id);
            if (idenType == null)
                return NotFound();
            ViewData["Title"] = "Activate Identitytype";
            ViewData["Controller"] = @$"\Identitytype\Activate\{id}";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.Activate(idenType);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
