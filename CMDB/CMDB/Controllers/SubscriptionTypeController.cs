using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using CMDB.Services;

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
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Activate");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            ViewData["AssignMobile"] = service.HasAdminAccess(service.Admin, SitePart, "AssignMobile");
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, SitePart, "AssignIdentity");
            return View();
        }
    }
}
