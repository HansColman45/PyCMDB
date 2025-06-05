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
    /// Controller for the subscription
    /// </summary>
    public class SubscriptionController : CMDBController
    {
        private readonly SubscriptionService service;
        private readonly PDFService _PDFservice;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        public SubscriptionController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            _PDFservice = new();
            SitePart = "Subscription";
            Table = "subscription";
        }
        /// <summary>
        /// The index page with the overview of the subscriptions
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// The search page for the subscriptions matching the search string
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
        /// <summary>
        /// The create page for creating a new subscription
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
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
        /// <summary>
        /// The edit page for editing a subscription
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// The details page for showing the details of a subscription
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return NotFound();
            var subscription = await service.GetByID((int)id);
            if (subscription is null)
                return NotFound();
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = $"{subscription.SubscriptionType.AssetCategory.Category} Details";
            ViewData["Controller"] = @"\Subscription\Create";
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
        /// <summary>
        /// The delete page for deleting a subscription
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            if (id is null)
                return NotFound();
            var subscription = await service.GetByID((int)id);
            if (subscription is null)
                return NotFound();
            log.Debug("Using Delete in {0}", Table);
            await BuildMenu();
            ViewData["Controller"] = @$"\Subscription\Delete\{id}";
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
        /// <summary>
        /// The activate page for activating a subscription
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// The page for assigning an identity to a subscription
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AssignIdentity(IFormCollection values, int? id)
        {
            if (id is null)
                return NotFound();
            var subscription = await service.GetByID((int)id);
            if (subscription is null)
                return NotFound();
            log.Debug("Using AssignIdentity in {0}", Table);
            ViewData["Title"] = $"Assign {subscription.SubscriptionType.AssetCategory.Category} to Identity";
            ViewData["Controller"] = @$"\Subscription\AssignIdentity\{id}";
            await BuildMenu();
            ViewData["AssignIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignIdentity");
            ViewBag.Identities = await service.ListFreeIdentities();
            ViewData["backUrl"] = "Subscription";
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    var identity = await service.GetIdentity(Int32.Parse(values["Identity"]));
                    if (ModelState.IsValid)
                    {
                        await service.AssignIdentity(subscription, identity);
                        return RedirectToAction("AssignForm", "Subscription", new { id });
                    }
                }
                catch (Exception ex)
                {
                    log.Error("DB error: {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(subscription);
        }
        /// <summary>
        /// The page for assigning a mobile to a subscription
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AssignMobile(IFormCollection values, int? id)
        {
            if (id is null)
                return NotFound();
            var subscription = await service.GetByID((int)id);
            if (subscription is null)
                return NotFound();
            log.Debug("Using AssignMobile in {0}", Table);
            ViewData["Title"] = $"Assign {subscription.SubscriptionType.AssetCategory.Category} to Identity";
            ViewData["Controller"] = @$"\Subscription\AssignMobile\{id}";
            await BuildMenu();
            ViewData["AssignMobile"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "AssignMobile");
            ViewData["backUrl"] = "Subscription";
            ViewBag.Mobiles = await service.ListFreeMobiles();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                try 
                { 
                    var mobile = await service.GetMobile(Int32.Parse(values["Mobile"]));
                    if (ModelState.IsValid)
                    {
                        await service.AssignMobile(subscription,mobile);
                        return RedirectToAction("AssignForm", "Subscription", new { id });
                    }
                }
                catch (Exception ex)
                {
                    log.Error("DB error: {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(subscription);
        }
        /// <summary>
        /// The assign form page for assigning a form to a subscription
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AssignForm(IFormCollection values, int? id)
        {
            if (id is null)
                return NotFound();
            var subscription = await service.GetByID((int)id);
            if (subscription is null)
                return NotFound();
            log.Debug("Using AssignForm in {0}", Table);
            ViewData["Title"] = "Assign form";
            ViewData["backUrl"] = "Subscription";
            ViewData["Action"] = "AssignForm";
            await BuildMenu();
            var admin = await service.Admin();
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            IdentityDTO identity;
            if (subscription.Identity is not null)
            {
                identity = subscription.Identity;
            }
            else if (subscription.Mobile.Identity is not null)
            {
                identity = subscription.Mobile.Identity;
            }
            else
            {
                identity = new();
                ModelState.AddModelError("", "No identity found for mobile or subscription");
            }
            ViewData["Name"] = identity.Name;
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    if (subscription.Identity is not null)
                    {
                        await _PDFservice.SetUserinfo(UserId: identity.UserID,
                            ITEmployee: ITPerson,
                            Singer: Employee,
                            FirstName: identity.FirstName,
                            LastName: identity.LastName,
                            Receiver: identity.Name,
                            Language: identity.Language.Code);
                        await _PDFservice.GenratePDFFile(Table, subscription.SubscriptionId);
                        await _PDFservice.GenratePDFFile("identity",subscription.Identity.IdenId); 
                        await _PDFservice.SetSubscriptionInfo(subscription);
                    }
                    else if (subscription.Mobile.Identity is not null) 
                    {
                        await _PDFservice.SetUserinfo(UserId: identity.UserID,
                            ITEmployee: ITPerson,
                            Singer: Employee,
                            FirstName: identity.FirstName,
                            LastName: identity.LastName,
                            Receiver: identity.Name,
                            Language: identity.Language.Code);
                        await _PDFservice.GenratePDFFile(Table, subscription.SubscriptionId);
                        await _PDFservice.GenratePDFFile("mobile", subscription.Mobile.MobileId);
                        await _PDFservice.SetSubscriptionInfo(subscription);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }
        /// <summary>
        /// The page for releasing an identity from a subscription
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <param name="MobileId"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, int? id, int? MobileId)
        {
            if (id is null && MobileId is null)
                return NotFound();
            var subscription = await service.GetByID((int)id);
            var identity = await service.GetIdentity((int)MobileId);
            if (subscription is null && identity is null)
                return NotFound();
            log.Debug($"Using Release Idenity in {Table}");
            ViewData["Title"] = $"Release {subscription.SubscriptionType.AssetCategory.Category} from Identity";
            ViewData["ReleaseIdentity"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseIdentity");
            ViewData["backUrl"] = "Subscription";
            ViewData["Action"] = "ReleaseIdentity";
            await BuildMenu();
            var admin = await service.Admin();
            ViewData["Name"] = identity.Name;
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    await _PDFservice.SetUserinfo(UserId: identity.UserID,
                            ITEmployee: ITPerson,
                            Singer: Employee,
                            FirstName: identity.FirstName,
                            LastName: identity.LastName,
                            Receiver: identity.Name,
                            Language: identity.Language.Code,
                            "Release");
                    await service.ReleaseIdenity(subscription, identity);
                    await _PDFservice.SetSubscriptionInfo(subscription);
                    await _PDFservice.GenratePDFFile(Table, subscription.SubscriptionId);
                    await _PDFservice.GenratePDFFile("identity", identity.IdenId);
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }
        /// <summary>
        /// The page for releasing a mobile from a subscription
        /// </summary>
        /// <param name="values"></param>
        /// <param name="id"></param>
        /// <param name="MobileId"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReleaseMobile(IFormCollection values, int? id, int? MobileId)
        {
            if (id is null && MobileId is null)
                return NotFound();
            var subscription = await service.GetByID((int)id);
            var mobile = await service.GetMobile((int)MobileId);
            if (subscription is null && mobile is null)
                return NotFound();
            log.Debug($"Using Release Idenity in {Table}");
            ViewData["Title"] = $"Release {subscription.SubscriptionType.AssetCategory.Category} from Mobile";
            ViewData["ReleaseMobile"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "ReleaseMobile");
            ViewData["backUrl"] = "Subscription";
            ViewData["Action"] = "ReleaseMobile";
            await BuildMenu();
            var admin = await service.Admin();
            ViewData["Name"] = mobile.Identity.Name;
            ViewData["AdminName"] = admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    await _PDFservice.SetUserinfo(UserId: mobile.Identity.UserID,
                            ITEmployee: ITPerson,
                            Singer: Employee,
                            FirstName: mobile.Identity.FirstName,
                            LastName: mobile.Identity.LastName,
                            Receiver: mobile.Identity.Name,
                            Language: mobile.Identity.Language.Code,
                            "Release");
                    await service.ReleaseMobile(subscription, mobile);
                    await _PDFservice.SetSubscriptionInfo(subscription);
                    await _PDFservice.GenratePDFFile(Table, subscription.SubscriptionId);
                    await _PDFservice.GenratePDFFile("mobile", mobile.MobileId);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }
    }
}
