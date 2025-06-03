using CMDB.Infrastructure;
using CMDB.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    /// <summary>
    /// The web controller for managing role permissions.
    /// </summary>
    public class RolePermissionController : CMDBController
    {
        private readonly RolePermService service;
        /// <summary>
        /// Constructor for the RolePermisionController.
        /// </summary>
        /// <param name="env"></param>
        public RolePermissionController(IWebHostEnvironment env) : base(env)
        {
            SitePart = "Permission";
            Table = "rolepermission";
            service = new RolePermService();
        }
        /// <summary>
        /// The index page with the ovierview of the permissions
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", SitePart);
            await BuildMenu();
            ViewData["Title"] = "Role Permission overview";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["actionUrl"] = @"\RolePermission\Search";
            var permissions = await service.ListAll();
            return View(permissions);
        }
    }
}
