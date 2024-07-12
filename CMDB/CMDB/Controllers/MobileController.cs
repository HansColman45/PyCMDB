using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using CMDB.Services;
using System.Threading.Tasks;
using System;
using CMDB.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq;
using CMDB.Util;
using QuestPDF.Fluent;

namespace CMDB.Controllers
{
    public class MobileController : CMDBController
    {
        private new readonly MobileService service;
        public MobileController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            SitePart = "Mobile";
            Table = "mobile";
            service = new(context);
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", SitePart);
            await BuildMenu();
            ViewData["Title"] = "Mobile overview";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
            ViewData["AssignSubscription"] = service.HasAdminAccess(service.Admin, SitePart, "AssignSubscription");
            ViewData["actionUrl"] = @"\Mobile\Search";
            var mobiles = await service.ListAll();
            return View(mobiles);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", SitePart);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["Title"] = "Mobile overview";
                await BuildMenu();
                var mobiles = await service.ListAll(search);
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
                ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
                ViewData["AssignSubscription"] = service.HasAdminAccess(service.Admin, SitePart, "AssignSubscription");
                ViewData["actionUrl"] = @"\Laptop\Search";
                return View(mobiles);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", SitePart);
            ViewData["Title"] = "Create Mobile";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            await BuildMenu();
            Mobile mobile = new();
            ViewBag.Types = service.ListAssetTypes(SitePart);
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string imei = values["IMEI"];
                    mobile.IMEI = Convert.ToInt64(values["IMEI"]);
                    int Type = Convert.ToInt32(values["MobileType"]);
                    var AssetType = service.ListAssetTypeById(Type);
                    mobile.MobileType = AssetType;
                    mobile.Category = AssetType.Category;
                    if(service.IsMobileExisting(mobile))
                        ModelState.AddModelError("", "Mobile already exist");
                    if (ModelState.IsValid)
                    {
                        await service.CreateNew(mobile,Table);
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
            return View(mobile);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            log.Debug($"Using details in {Table}");
            ViewData["Title"] = "Mobile details";
            await BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["IdentityOverview"] = service.HasAdminAccess(service.Admin, SitePart, "IdentityOverview");
            ViewData["SubscriptionOverview"] = service.HasAdminAccess(service.Admin, SitePart, "SubscriptionOverview");
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "ReleaseIdentity");
            ViewData["AssignSubscription"] = service.HasAdminAccess(service.Admin, SitePart, "AssignSubscription");
            ViewData["ReleaseSubscription"] = service.HasAdminAccess(service.Admin, SitePart, "ReleaseSubscription");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            service.GetLogs(Table, (int)id, mobile);
            service.GetAssignedIdentity(mobile);
            service.GetAssignedSubscription(mobile);
            return View(mobile);
        }
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            log.Debug("Using Edit in {0}", Table);
            ViewData["Title"] = "Edit mobile";
            await BuildMenu();
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            ViewBag.Types = service.ListAssetTypes(SitePart);
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                long newImei = Convert.ToInt64(values["IMEI"]);
                int Type = Convert.ToInt32(values["MobileType.TypeID"]);
                var AssetType = service.ListAssetTypeById(Type);
                if (service.IsMobileExisting(mobile, newImei))
                    ModelState.AddModelError("", "Mobile already exist");
                try
                {
                    if (ModelState.IsValid)
                    {
                        await service.Update(mobile, newImei, AssetType, Table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("DB error: {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(mobile);
        }
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            log.Debug("Using Delete in {0}", Table);
            ViewData["Title"] = "Deactivate Mobile";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["backUrl"] = "Mobile";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string reason = values["reason"];
                    if (ModelState.IsValid)
                    {
                        if (mobile.IdentityId > 1)
                        {
                            PDFGenerator PDFGenerator = new()
                            {
                                ITEmployee = service.Admin.Account.UserID,
                                Singer = mobile.Identity.Name,
                                UserID = mobile.Identity.UserID,
                                FirstName = mobile.Identity.FirstName,
                                LastName = mobile.Identity.LastName,
                                Language = mobile.Identity.Language.Code,
                                Receiver = mobile.Identity.Name
                            };
                            PDFGenerator.SetMobileInfo(mobile);
                            string pdfFile = PDFGenerator.GeneratePath(_env);
                            PDFGenerator.GeneratePdf(pdfFile);
                            await service.LogPdfFile("identity", mobile.Identity.IdenId, pdfFile);
                            await service.LogPdfFile(Table, mobile.MobileId, pdfFile);
                            await service.ReleaseIdenity(mobile, mobile.Identity, Table);
                        }
                        await service.Deactivate(mobile, reason, Table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("DB error: {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(mobile);
        }
        public async Task<IActionResult> Activate(int? id)
        {
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Mobile";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            await BuildMenu();
            if (service.HasAdminAccess(service.Admin, SitePart, "Activate"))
            {
                try
                {
                    await service.Activate(mobile, Table);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    log.Error("DB error: {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            else
                RedirectToAction(nameof(Index));
            return View();
        }
        public async Task<IActionResult> AssignIdentity(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            log.Debug("Using Assign identity in {0}", Table);
            ViewData["Title"] = "Assign identity to mobile";
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
            ViewData["backUrl"] = "Mobile";
            await BuildMenu();
            ViewBag.Identities = service.ListFreeIdentities();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit)) 
            {
                try
                {
                    if (!service.IsDeviceFree(mobile))
                        ModelState.AddModelError("", "Mobile can not be assigned to another user");
                    Identity identity = await service.GetIdentity(Int32.Parse(values["Identity"]));
                    if (ModelState.IsValid)
                    {
                        await service.AssignIdentity2Mobile(identity, mobile, Table);
                        return RedirectToAction("AssignForm", "Mobile", new { id });
                    }
                }
                catch (Exception ex)
                {
                    log.Error("DB error: {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(mobile);
        }
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            log.Debug("Using Release identity in {0}", Table);
            ViewData["Title"] = "Release identity from Mobile";
            ViewData["ReleaseIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "ReleaseIdentity");
            ViewData["backUrl"] = "Mobile";
            ViewData["Action"] = "ReleaseIdentity";
            await BuildMenu();
            service.GetAssignedIdentity(mobile);
            Identity identity = mobile.Identity;
            ViewData["Name"] = identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                try
                {
                    if (ModelState.IsValid)
                    {
                        PDFGenerator PDFGenerator = new()
                        {
                            ITEmployee = ITPerson,
                            Singer = Employee,
                            UserID = identity.UserID,
                            FirstName = identity.FirstName,
                            LastName = identity.LastName,
                            Language = identity.Language.Code,
                            Receiver = identity.Name,
                            Type = "Release"
                        };
                        PDFGenerator.SetMobileInfo(mobile);
                        string pdfFile = PDFGenerator.GeneratePath(_env);
                        PDFGenerator.GeneratePdf(pdfFile);
                        await service.LogPdfFile("identity", mobile.Identity.IdenId, pdfFile);
                        await service.LogPdfFile(Table, mobile.MobileId, pdfFile);
                        await service.ReleaseIdenity(mobile, identity, Table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("DB error: {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(mobile);
        }
        public async Task<IActionResult> ReleaseSubscription(IFormCollection values, int? id, int? subid)
        {
            if (id is null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile is null)
                return NotFound();
            log.Debug($"Using Release subscription in {Table}");
            await BuildMenu();
            return View(mobile);
        }
        public async Task<IActionResult> AssignSubscription(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            log.Debug("Using assign subscription in {0}", Table);
            ViewData["Title"] = "Assign subscription to mobile";
            ViewData["AssignSubscription"] = service.HasAdminAccess(service.Admin, SitePart, "AssignSubscription");
            ViewData["backUrl"] = "Mobile";
            await BuildMenu();
            ViewBag.Subscriptions = service.ListFreeMobileSubscriptions();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                Subscription subscription =await service.GetSubribtion(Int32.Parse(values["Subscriptions"]));
                if (!service.IsDeviceFree(mobile, true))
                    ModelState.AddModelError("", "Mobile can not be assigned to another user");
                if (ModelState.IsValid)
                {
                    await service.AssignSubscription(mobile, subscription,Table);
                    return RedirectToAction("AssignForm", "Mobile", new { id });
                }
            }
            return View(mobile);
        }
        public async Task<IActionResult> AssignForm(IFormCollection values, int? id)
        {
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            log.Debug("Using Assign form in {0}", Table);
            ViewData["Title"] = "Assign form";
            ViewData["backUrl"] = "Mobile";
            ViewData["Action"] = "AssignForm";
            await BuildMenu();
            service.GetAssignedIdentity(mobile);
            service.GetAssignedSubscription(mobile);
            ViewData["Name"] = mobile.Identity.Name;
            ViewData["AdminName"] = service.Admin.Account.UserID;
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid) {
                    if (mobile.Identity != null) {
                        PDFGenerator _PDFGenerator = new()
                        {
                            ITEmployee = ITPerson,
                            Singer = Employee,
                            UserID = mobile.Identity.UserID,
                            FirstName = mobile.Identity.FirstName,
                            LastName = mobile.Identity.LastName,
                            Language = mobile.Identity.Language.Code,
                            Receiver = mobile.Identity.Name
                        };
                        _PDFGenerator.SetMobileInfo(mobile);
                        string pdfFile = _PDFGenerator.GeneratePath(_env);
                        await service.LogPdfFile("identity", mobile.Identity.IdenId, pdfFile);
                        await service.LogPdfFile(Table, mobile.MobileId, pdfFile);
                    }
                    else if (mobile.Subscriptions.Count > 0)
                    {
                        PDFGenerator _PDFGenerator = new()
                        {
                            ITEmployee = ITPerson,
                            Singer = Employee
                        };
                        _PDFGenerator.SetMobileInfo(mobile);
                        _PDFGenerator.SetSubscriptionInfo(mobile.Subscriptions.First());
                        string pdfFile = _PDFGenerator.GeneratePath(_env);
                        _PDFGenerator.GeneratePdf(pdfFile);
                        await service.LogPdfFile(Table, mobile.MobileId, pdfFile);
                        await service.LogPdfFile("subscription", mobile.Subscriptions.First().SubscriptionId, pdfFile);
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(mobile);
        }
    }
}
