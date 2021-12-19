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
        private readonly ILogger<PermissionController> _logger;
        private readonly static string sitePart = "Permission";
        private readonly static string table = "permission";
        private new readonly PermissionService service;
        public PermissionController(CMDBContext context, ILogger<PermissionController> logger, IWebHostEnvironment env) : base(context, logger, env)
        {
            _logger = logger;
            service = new(context);
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Using list all for {0}", sitePart);
            BuildMenu();
            ViewData["Title"] = "Permissiont overview";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewData["actionUrl"] = @"\Permission\Search";
            return View();
        }
    }
}
