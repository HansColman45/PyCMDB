using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using CMDB.Services;
using System.Threading.Tasks;

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
            return View();
        }
    }
}
