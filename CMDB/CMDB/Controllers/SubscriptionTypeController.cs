using CMDB.API.Models;
using CMDB.Infrastructure;
using CMDB.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    /// <summary>
    /// Controller for the subscription type
    /// </summary>
    public class SubscriptionTypeController : CMDBController
    {
        private SubscriptionTypeService service;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        public SubscriptionTypeController(IWebHostEnvironment env) : base(env)
        {
            SitePart = "Subscription Type";
            Table = "subscriptiontype";
            service = new();
        }
        /// <summary>
        /// The index page with the overview of the subscription types
        /// </summary>
        /// <returns></returns>
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
            ViewData["Controller"] = @"\SubscriptionType\Create";
            var types = await service.ListAll();
            return View(types);
        }
        /// <summary>
        /// The search page for the subscription types
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
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
                ViewData["Controller"] = @"\SubscriptionType\Create";
                return View(types);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// The create page for the subscription types
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", Table);
            ViewData["Title"] = "Create Subscription";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            await BuildMenu();
            SubscriptionTypeDTO subscriptionType = new();
            ViewBag.Types = await service.GetCategories();
            string FormSubmit = values["form-submitted"];
            ViewData["Controller"] = @"\SubscriptionType\Create";
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                var cat = values["AssetCategory"];
                subscriptionType.AssetCategory = await service.GetAssetCategory(Int32.Parse(cat));
                subscriptionType.Provider = values["Provider"];
                subscriptionType.Type = values["Type"];
                subscriptionType.Description = values["Description"];
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.Create(subscriptionType);
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
        /// <summary>
        /// The edit page for the subscription types
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            SubscriptionTypeDTO subscriptionType = await service.GetById((int)id);
            if (subscriptionType == null)
                return NotFound();
            log.Debug("Using Edit in {0}", Table);
            ViewData["Title"] = $"Edit {subscriptionType.AssetCategory.Category}";
            ViewData["Controller"] = @$"\SubscriptionType\Edit\{id}";
            await BuildMenu();
            ViewBag.Types = await service.GetCategories();
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
                        await service.Edit(subscriptionType, provider, Type, Description);
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
        /// <summary>
        /// The details page for the subscription types
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            SubscriptionTypeDTO subscriptionType = await service.GetById((int)id);
            if (subscriptionType == null)
                return NotFound();
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = $"{subscriptionType.AssetCategory.Category} Details";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] =  service.DateFormat;
            ViewData["Controller"] = @"\SubscriptionType\Create";
            return View(subscriptionType);
        }
        /// <summary>
        /// The delete page for the subscription types
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            SubscriptionTypeDTO subscriptionType = await service.GetById((int)id);
            if (subscriptionType == null)
                return NotFound();
            log.Debug("Using Delete in {0}", Table);
            ViewData["Controller"] = @$"\SubscriptionType\Delete\{id}";
            await BuildMenu();
            ViewData["Title"] = $"Deactivate {subscriptionType.AssetCategory.Category}";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.Delete(subscriptionType, ViewData["reason"].ToString());
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
        /// <summary>
        /// The activate page for the subscription types
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Activate(int? id)
        {
            if (id == null)
                return NotFound();
            SubscriptionTypeDTO subscriptionType = await service.GetById((int)id);
            if (subscriptionType == null)
                return NotFound();
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = $"Activate {subscriptionType.AssetCategory.Category}";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                try
                {
                    await service.Activate(subscriptionType);
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
