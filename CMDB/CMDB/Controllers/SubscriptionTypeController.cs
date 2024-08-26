using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using CMDB.Services;
using System;
using Microsoft.AspNetCore.Http;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.Graph;

namespace CMDB.Controllers
{
    public class SubscriptionTypeController : CMDBController
    {
        private new SubscriptionTypeService service;
        public SubscriptionTypeController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            SitePart = "Subscription Type";
            Table = "subscriptiontype";
            service = new(context);
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            ViewData["Title"] = "Subscription overview";
            await BuildMenu();
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["AssignMobile"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignMobile");
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["actionUrl"] = @"\SubscriptionType\Search";
            var types = await service.ListAll();
            return View(types);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search in {0}", Table);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                ViewData["Title"] = "Subscription overview";
                await BuildMenu();
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["AssignMobile"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignMobile");
                ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
                var types = await service.ListAll(search);
                return View(types);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", Table);
            ViewData["Title"] = "Create Subscription";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            await BuildMenu();
            SubscriptionType subscriptionType = new();
            ViewBag.Types = service.GetCategories();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                var cat = values["Category"];
                subscriptionType.Category = service.GetAssetCategory(Int32.Parse(cat));
                subscriptionType.Provider = values["Provider"];
                subscriptionType.Type = values["Type"];
                subscriptionType.Description = values["Description"];
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.Create(subscriptionType, Table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                    log.Error(ex.ToString());
                }
            }
            return View(subscriptionType);
        }
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            var list = await service.GetById((int)id);
            SubscriptionType subscriptionType = list.FirstOrDefault();
            if (subscriptionType == null)
                return NotFound();
            log.Debug("Using Edit in {0}", Table);
            ViewData["Title"] = $"Edit {subscriptionType.Category.Category}";
            await BuildMenu();
            ViewBag.Types = service.GetCategories();
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string provider = values["Provider"];
                string Type = values["Type"];
                string Description = values["Description"];
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.Edit(subscriptionType, provider, Type, Description, Table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                    log.Error(ex.ToString());
                }
            }
            return View(subscriptionType);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var list = await service.GetById((int)id);
            SubscriptionType subscriptionType = list.FirstOrDefault();
            if (subscriptionType == null)
                return NotFound();
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = $"{subscriptionType.Category.Category} Details";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] =  service.DateFormat;
            service.GetLogs(Table, (int)id, subscriptionType);
            return View(subscriptionType);
        }
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            var list = await service.GetById((int)id);
            SubscriptionType subscriptionType = list.FirstOrDefault();
            if (subscriptionType == null)
                return NotFound();
            log.Debug("Using Delete in {0}", Table);
            await BuildMenu();
            ViewData["Title"] = $"Deactivate {subscriptionType.Category.Category}";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.Delete(subscriptionType, ViewData["reason"].ToString(), Table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                    log.Error(ex.ToString());
                }
            }
            return View(subscriptionType);
        }
        public async Task<IActionResult> Activate(int? id)
        {
            if (id == null)
                return NotFound();
            var list = await service.GetById((int)id);
            SubscriptionType subscriptionType = list.FirstOrDefault();
            if (subscriptionType == null)
                return NotFound();
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = $"Activate {subscriptionType.Category.Category}";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                try
                {
                    await service.Activate(subscriptionType,Table);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                    log.Error(ex.ToString());
                }
            }
            else
                return RedirectToAction(nameof(Index));
            return View();
        }
    }
}
