using CMDB.Infrastructure;
using CMDB.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    /// <summary>
    /// Controller for the permission
    /// </summary>
    public class PermissionController : CMDBController
    {
        private readonly PermissionService service;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        public PermissionController(IWebHostEnvironment env) : base(env)
        {
            service = new();
            SitePart = "Permission";
            Table = "permission";
        }
        /// <summary>
        /// The index page with the ovierview of the permissions
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", SitePart);
            await BuildMenu();
            ViewData["Title"] = "Permissiont overview";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["actionUrl"] = @"\Permission\Search";
            var permissions = await service.ListAll();
            return View(permissions);
        }
        /// <summary>
        /// The searrch page
        /// </summary>
        /// <param name="searchStr"></param>
        /// <returns></returns>
        public async Task<IActionResult> Search(string searchStr)
        {
            log.Debug("Searching for {0} in {1}", searchStr, SitePart);
            await BuildMenu();
            ViewData["Title"] = "Permissiont overview";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
            ViewData["actionUrl"] = @"\Permission\Search";
            var permissions = await service.ListAll(searchStr);
            return View("Index", permissions);
        }
    }
}
