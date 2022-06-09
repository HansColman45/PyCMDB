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
                        ModelState.AddModelError("", "Asset already exist");
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
        public async Task<IActionResult> Details(int imei)
        {
            log.Debug("Using details in {0}", Table);
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
            if (imei == 0)
                return NotFound();
            var mobiles = service.GetMobileById(imei);
            Mobile mobile = mobiles.FirstOrDefault();
            if (mobile == null)
                return NotFound();
            service.GetLogs(Table, imei, mobile);
            service.GetAssignedIdentity(mobile);
            return View(mobile);
        }
    }
}
