using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Graph.Models.Security;
using Microsoft.Graph.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using CMDB.Util;
using System.Drawing.Drawing2D;

namespace CMDB.Controllers
{
    public class KensingtonController : CMDBController
    {
        private KensingtonService service = new();
        public KensingtonController(IWebHostEnvironment env) : base(env)
        {
            SitePart = "Account";
            Table = "account";
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", SitePart);
            await BuildMenu();
            var keys = await service.ListAll();
            ViewData["Title"] = "Kensington overview";
            ViewData["Controller"] = @"\Kensington\Create";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["AssignDeviceAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignDevice");
            ViewData["actionUrl"] = @"\Kensington\Search";
            return View(keys);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search in {0}", Table);
            if (!string.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                ViewData["Title"] = "Identity overview";
                ViewData["Controller"] = @"\Identity\Create";
                await BuildMenu();
                var list = await service.Search(search);
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["AssignDeviceAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignDevice");
                ViewData["actionUrl"] = @"\Identity\Search";
                return View(list);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using create for {0}", SitePart);
            await BuildMenu();
            ViewData["Title"] = "Create new Kensington";
            ViewData["Controller"] = @"\Kensington\Create";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewBag.Type = await service.ListAssetTypes("Kensington");
            KensingtonDTO kensington =new();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    var type = values["type"];
                    var serial = values["SerialNumber"];
                    kensington.SerialNumber = serial;
                    var hasLock = values["HasLock"];
                    bool haslock = StringExtensions.GetBool(hasLock);
                    kensington.HasLock = haslock;
                    var amount = values["AmountOfKeys"];
                    kensington.AmountOfKeys = int.Parse(amount);
                    AssetTypeDTO assettype = await service.GetAssetTypeById(int.Parse(type));
                    if (ModelState.IsValid)
                    {
                        await service.Create(serial, haslock, int.Parse(amount), assettype);
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
            return View(kensington);
        }
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit for {0}", SitePart);
            if (id is null)
                return NotFound();
            var key = await service.GetByID((int)id);
            if (key is null)
                return NotFound();
            await BuildMenu();
            ViewData["Title"] = "Edit Kensington";
            ViewData["Controller"] = @$"\Kensington\Edit\{id}";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewBag.Type = await service.ListAssetTypes("Kensington");
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    AssetTypeDTO assettype = await service.GetAssetTypeById(key.Type.TypeID);
                    key.Type = assettype;
                    key.SerialNumber = values["SerialNumber"];
                    var hasLock = values["HasLock"];
                    bool haslock = StringExtensions.GetBool(hasLock);
                    key.HasLock = haslock;
                    var amount = values["AmountOfKeys"];
                    key.AmountOfKeys = int.Parse(amount);
                    if (ModelState.IsValid)
                    {
                        await service.Update(key);
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
            return View(key);
        }
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details for {0}", SitePart);
            if (id is null)
                return NotFound();
            var key = await service.GetByID((int)id);
            if (key is null)
                return NotFound();
            await BuildMenu();
            ViewData["Title"] = "Kensington details";
            ViewData["Controller"] = @"\Kensington\Create";
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["DeviceOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "DeviceOverview");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            return View(key);
        }
        public async Task<IActionResult> Delete(IFormCollection values, int? Id)
        {
            log.Debug("Using delete for {0}", SitePart);
            if (Id is null)
                return NotFound();
            var key = await service.GetByID((int)Id);
            if (key is null)
                return NotFound();
            await BuildMenu();
            ViewData["Title"] = "Delete Kensington";
            ViewData["Controller"] = @$"\Kensington\Delete\{Id}";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.Deactivate(key, values["reason"]);
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
            return View(key);
        }
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using activate for {0}", SitePart);
            if (id is null)
                return NotFound();
            var key = await service.GetByID((int)id);
            if (key is null)
                return NotFound();
            await BuildMenu();
            ViewData["Title"] = "Activate Kensington";
            ViewData["Controller"] = @$"\Kensington\Activate\{id}";
            bool hasAccess = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["ActiveAccess"] = hasAccess;
            if (hasAccess)
            {
                await service.Activate(key);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
