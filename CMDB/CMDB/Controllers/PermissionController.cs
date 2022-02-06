using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Util;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using CMDB.Services;

namespace CMDB.Controllers
{
    public class PermissionController : CMDBController
    {
        private new readonly PermissionService service;
        public PermissionController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
            SitePart = "Permission";
            Table = "permission";
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", SitePart);
            await BuildMenu();
            ViewData["Title"] = "Permissiont overview";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Delete");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, SitePart, "Update");
            ViewData["actionUrl"] = @"\Permission\Search";
            return View();
        }
    }
}
