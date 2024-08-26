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
using CMDB.Util;
using Microsoft.Graph;
using Subscription = CMDB.Domain.Entities.Subscription;
using QuestPDF.Fluent;

namespace CMDB.Controllers
{

    public class SubscriptionController : CMDBController
    {
        private new readonly SubscriptionService service;
        public SubscriptionController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
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
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
                return View(list);
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
            ViewBag.Types = service.GetSubscriptionTypes();
            Subscription subscription = new();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string type = values["SubscriptionType"];
                string phoneNumber = values["PhoneNumber"];
                try
                {
                    SubscriptionType subscriptionType = await service.GetSubscriptionTypeById(Int32.Parse(type));
                    if (await service.IsSubscritionExisting(subscriptionType, phoneNumber))
                        ModelState.AddModelError("", "Subscription already exist please change values");
                    if (ModelState.IsValid) { 
                        await service.Create(subscriptionType,phoneNumber,Table);
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
            var list = await service.GetByID((int)id);
            Subscription subscription = list.FirstOrDefault();
            if (subscription is null)
                return NotFound();
            log.Debug("Using Edit in {0}", Table);
            ViewData["Title"] = $"Edit {subscription.Category.Category}";
            await BuildMenu();
            ViewBag.Types = service.GetSubscriptionTypes();
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
                        await service.Edit(subscription,phoneNumber,Table);
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
            var list = await service.GetByID((int)id);
            Subscription subscription = list.FirstOrDefault();
            if (subscription is null)
                return NotFound();
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = $"{subscription.Category.Category} Details";
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
            service.GetLogs(Table, (int)id, subscription);
            return View(subscription);
        }
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            if (id is null)
                return NotFound();
            var list = await service.GetByID((int)id);
            Subscription subscription = list.FirstOrDefault();
            if (subscription is null)
                return NotFound();
            log.Debug("Using Delete in {0}", Table);
            await BuildMenu();
            ViewData["Title"] = $"Deactivate {subscription.Category.Category}";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    if (ModelState.IsValid) { 
                        if(subscription.IdentityId > 1 || subscription.Mobile.IdentityId >1)
                        {
                            PDFGenerator PDFGenerator = new()
                            {
                                ITEmployee = service.Admin.Account.UserID,
                                Singer = subscription.Identity is not null ? subscription.Identity.Name : subscription.Mobile.Identity.Name,
                                FirstName = subscription.Identity is not null ? subscription.Identity.FirstName : subscription.Mobile.Identity.FirstName,
                                LastName = subscription.Identity is not null ? subscription.Identity.LastName : subscription.Mobile.Identity.LastName,
                                UserID = subscription.Identity is not null ? subscription.Identity.UserID : subscription.Mobile.Identity.UserID,
                                Language = subscription.Identity is not null ? subscription.Identity.Language.Code : subscription.Mobile.Identity.Language.Code,
                                Receiver = subscription.Identity is not null ? subscription.Identity.Name : subscription.Mobile.Identity.Name,
                            };
                            PDFGenerator.SetSubscriptionInfo(subscription);
                            string pdfFile = PDFGenerator.GeneratePath(_env);
                            PDFGenerator.GeneratePdf(pdfFile);
                            int intID = subscription.Identity is not null ? (int)subscription.Identity.IdenId : (int)subscription.Mobile.IdentityId;
                            await service.LogPdfFile("identity", intID, pdfFile);
                            await service.LogPdfFile(Table, subscription.SubscriptionId, pdfFile);
                            await service.ReleaseIdenity(subscription, subscription.Identity is not null ? subscription.Identity : subscription.Mobile.Identity);
                        }
                        await service.Deactivate(subscription, ViewData["reason"].ToString(), Table);
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
            var list = await service.GetByID((int)id);
            Subscription subscription = list.FirstOrDefault();
            if (subscription is null)
                return NotFound();
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = $"Activate {subscription.Category.Category}";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                try
                {
                    await service.Activate(subscription,Table);
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
            var list = await service.GetByID((int)id);
            Subscription subscription = list.FirstOrDefault();
            if (subscription == null)
                return NotFound();
            log.Debug($"Using Release in {Table}");
            
            return View(subscription);
        }
    }
}
