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
                ViewData["AssignIdentityAccess"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
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
            ViewData["Title"] = "Create Laptop";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            await BuildMenu();
            Mobile mobile = new();
            ViewBag.Types = service.ListAssetTypes(SitePart);
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    mobile.IMEI = Convert.ToInt32(values["IMEI"]);
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
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            service.GetLogs(Table, (int)id, mobile);
            service.GetAssignedIdentity(mobile);
            service.GetAssignedSubscription(mobile);
            return View(mobile);
        }
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {

            log.Debug("Using Edit in {0}", Table);
            ViewData["Title"] = "Edit mobile";
            await BuildMenu();
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            ViewBag.Types = service.ListAssetTypes(SitePart);
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            string FormSubmit = values["form-submitted"];
            if (!string.IsNullOrEmpty(FormSubmit))
            {
                int newImei = Convert.ToInt32(values["IMEI"]);
                int Type = Convert.ToInt32(values["MobileType.TypeID"]);
                var AssetType = service.ListAssetTypeById(Type);
                mobile.MobileType = AssetType;
                mobile.Category = AssetType.Category;
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
            log.Debug("Using Delete in {0}", Table);
            ViewData["Title"] = "Deactivate Mobile";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["backUrl"] = "Mobile";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string reason = values["reason"];
                    if (ModelState.IsValid)
                    {
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
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Mobile";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
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
            log.Debug("Using Assign identity in {0}", Table);
            ViewData["Title"] = "Assign identity to Mobile";
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
            ViewData["backUrl"] = "Mobile";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            return View(mobile);
        }
        public async Task<IActionResult> ReleaseIdentity(IFormCollection values, int? id)
        {
            log.Debug("Using Release identity in {0}", Table);
            ViewData["Title"] = "Release identity from Mobile";
            ViewData["ReleaseIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "ReleaseIdentity");
            ViewData["backUrl"] = "Mobile";
            ViewData["Action"] = "ReleaseIdentity";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            return View(mobile);
        }
        public async Task<IActionResult> AssignForm(IFormCollection values, int? id)
        {
            log.Debug("Using Assign form in {0}", Table);
            ViewData["Title"] = "Assign form";
            ViewData["backUrl"] = "Mobile";
            ViewData["Action"] = "AssignForm";
            await BuildMenu();
            if (id == null)
                return NotFound();
            var mobiles = service.GetMobileById((int)id);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            return View(mobile);
        }
    }
}
