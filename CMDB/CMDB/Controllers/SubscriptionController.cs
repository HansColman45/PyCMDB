using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;
using Subscription = CMDB.Domain.Entities.Subscription;

namespace CMDB.Controllers
{

    public class SubscriptionController : CMDBController
    {
        private readonly SubscriptionService service;
        private readonly PDFService _PDFservice;
        public SubscriptionController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            _PDFservice = new();
            SitePart = "Subscription";
            Table = "subscription";
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", Table);
            ViewData["Title"] = "Subscription overview";
            await BuildMenu();
            var list = await service.ListAll();
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["AssignMobile"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignMobile");
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["actionUrl"] = @"\Subscription\Search";
            ViewData["Controller"] = @"\Subscription\Create";
            return View(list);
        }
        public async Task<IActionResult> Search(string search)
        {

            log.Debug("Using search in {0}", Table);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                ViewData["Title"] = "Subscription overview";
                await BuildMenu();
                var list = await service.ListAll(search);
                ViewData["Controller"] = @"\Subscription\Create";
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                ViewData["AssignMobile"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignMobile");
                ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
                ViewData["actionUrl"] = @"\Subscription\Search";
                ViewData["Controller"] = @"\Subscription\Create";
                return View(list);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", Table);
            ViewData["Title"] = "Create Subscription";
            ViewData["Controller"] = @"\Subscription\Create";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            await BuildMenu();
            ViewBag.Types = await service.GetSubscriptionTypes();
            SubscriptionDTO subscription = new();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string type = values["SubscriptionType"];
                string phoneNumber = values["PhoneNumber"];
                try
                {
                    SubscriptionTypeDTO subscriptionType = await service.GetSubscriptionTypeById(Int32.Parse(type));
                    if (await service.IsSubscritionExisting(subscriptionType, phoneNumber))
                        ModelState.AddModelError("", "Subscription already exist please change values");
                    if (ModelState.IsValid) { 
                        await service.Create(subscriptionType,phoneNumber);
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
            
            return View(subscription);
        }
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            if (id is null)
                return NotFound();
            var subscription = await service.GetByID((int)id);
            if (subscription is null)
                return NotFound();
            ViewData["Controller"] = @$"\Subscription\Edit\{id}";
            log.Debug("Using Edit in {0}", Table);
            ViewData["Title"] = $"Edit {subscription.SubscriptionType.AssetCategory.Category}";
            await BuildMenu();
            ViewBag.Types = await service.GetSubscriptionTypes();
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string phoneNumber = values["PhoneNumber"];
                try
                {
                    if (await service.IsSubscritionExisting(subscription.SubscriptionType, phoneNumber, (int)id))
                        ModelState.AddModelError("", "Subscription already exist please change values");
                    if (ModelState.IsValid)
                    {
                        await service.Edit(subscription,phoneNumber);
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
            return View(subscription);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return NotFound();
            var subscription = await service.GetByID((int)id);
            ViewData["Controller"] = @"\Subscription\Create";
            if (subscription is null)
                return NotFound();
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = $"{subscription.SubscriptionType.AssetCategory.Category} Details";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["MobileOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "MobileOverview");
            ViewData["IdentityOverview"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["AssignMobile"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignMobile");
            ViewData["ReleaseMobile"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseMobile");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            return View(subscription);
        }
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            if (id is null)
                return NotFound();
            ViewData["Controller"] = @$"\Subscription\Delete\{id}";
            var subscription = await service.GetByID((int)id);
            if (subscription is null)
                return NotFound();
            log.Debug("Using Delete in {0}", Table);
            await BuildMenu();
            ViewData["Title"] = $"Deactivate {subscription.SubscriptionType.AssetCategory.Category}";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    if (ModelState.IsValid) { 
                        if(subscription.IdentityId > 1 || subscription.Mobile?.IdentityId >1)
                        {
                            var admin = await service.Admin();
                            await _PDFservice.SetUserinfo(UserId: subscription.Identity is not null ? subscription.Identity.UserID : subscription.Mobile.Identity.UserID,
                                ITEmployee: admin.Account.UserID,
                                Singer: subscription.Identity is not null ? subscription.Identity.Name : subscription.Mobile.Identity.Name,
                                FirstName: subscription.Identity is not null ? subscription.Identity.FirstName : subscription.Mobile.Identity.FirstName,
                                LastName: subscription.Identity is not null ? subscription.Identity.LastName : subscription.Mobile.Identity.LastName,
                                Receiver: subscription.Identity is not null ? subscription.Identity.Name : subscription.Mobile.Identity.Name,
                                Language: subscription.Identity is not null ? subscription.Identity.Language.Code : subscription.Mobile.Identity.Language.Code,
                                "Release");
                            await _PDFservice.SetSubscriptionInfo(subscription);
                            await _PDFservice.GenratePDFFile(Table, subscription.SubscriptionId);
                            int intID = subscription.Identity is not null ? (int)subscription.Identity.IdenId : (int)subscription.Mobile.IdentityId;
                            await _PDFservice.GenratePDFFile("identity", intID);
                            await service.ReleaseIdenity(subscription, subscription.Identity is not null ? subscription.Identity : subscription.Mobile.Identity);
                        }
                        await service.Deactivate(subscription, ViewData["reason"].ToString());
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
            return View(subscription);
        }
        public async Task<IActionResult> Activate(int? id)
        {
            if (id is null)
                return NotFound();
            var subscription = await service.GetByID((int)id);
            if (subscription is null)
                return NotFound();
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = $"Activate {subscription.SubscriptionType.AssetCategory.Category}";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                try
                {
                    await service.Activate(subscription);
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
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, int? id, int? idenid)
        {
            if (id is null)
                return NotFound();
            if (idenid is null)
                return NotFound();
            var subscription = await service.GetByID((int)id);
            if (subscription == null)
                return NotFound();
            log.Debug($"Using Release in {Table}");
            
            return View(subscription);
        }
    }
}
