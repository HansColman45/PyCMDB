using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CMDB.Models;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Services;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    /// <summary>
    /// Base controller for CMDB
    /// </summary>
    public class CMDBController : Controller
    {
        /// <summary>
        /// Logger
        /// </summary>
        protected readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Environment
        /// </summary>
        protected readonly IWebHostEnvironment _env;
        /// <summary>
        /// The sitepart
        /// </summary>
        protected string SitePart { get; set; }
        /// <summary>
        /// The table
        /// </summary>
        protected string Table { get; set; }
        /// <summary>
        /// The token that is been used
        /// </summary>
        protected string Token { get; set; }
        private readonly CMDBServices service;
        private CMDBController()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        public CMDBController(IWebHostEnvironment env)
        {
            _env = env;
            service = new();
        }
        /// <summary>
        /// Build the menu
        /// </summary>
        /// <returns></returns>
        protected async Task BuildMenu()
        {
            Token = TokenStore.Token;
            List<Menu> menul1 = (List<Menu>)await service.ListFirstMenuLevel();
            foreach (Menu m in menul1)
            {
                List<Menu> mL2 = (List<Menu>)await service.ListSecondMenuLevel(m.MenuId);
                m.Children = mL2;
                foreach (Menu m1 in mL2)
                {
                    m1.Children = await service.ListPersonalMenu(m1.MenuId);
                }
            }
            ViewBag.Menu = menul1;
            ViewBag.BackIcon = "fa fa-arrow-left";
            ViewBag.NewIcon = "fas fa-plus";
            ViewBag.EditIcon = "fa fa-pencil";
            ViewBag.DeactivateIcon = "fas fa-trash-alt";
            ViewBag.ActivateIcon = "fas fa-plus-circle";
            ViewBag.AddIdentityIcon = "fa fa-user-plus";
            ViewBag.AddDeviceIcon = "fa fa-laptop";
            ViewBag.InfoIcon = "fa fa-info";
            ViewBag.ReleaseIdenIcon = "fa fa-user-minus";
            ViewBag.MobileIcon = "fas fa-mobile-alt";
            ViewBag.SubscriptionIcon = "fas fa-file-invoice-dollar";
            ViewBag.SearchIcon = "fas fa-search";
            ViewBag.KeyIcon = "fa-solid fa-unlock-keyhole";
        }
        /// <summary>
        /// Error page
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
