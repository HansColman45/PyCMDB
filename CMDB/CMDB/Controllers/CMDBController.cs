using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CMDB.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Services;
using System.Linq;

namespace CMDB.Controllers
{
    public class CMDBController : Controller
    {
        protected CMDBServices service;
        protected CMDBContext _context;
        private readonly ILogger<Controller> _logger;
        private readonly IWebHostEnvironment _env;

        public CMDBController(CMDBContext context, ILogger<Controller> logger, IWebHostEnvironment env)
        {
            _context = context;
            _logger = logger;
            _env = env;
            service = new(context);
        }
        protected void BuildMenu()
        {
            List<Menu> menul1 = (List<Menu>)service.ListFirstMenuLevel();
            foreach (Menu m in menul1)
            {
                if (m.Children is null)
                    m.Children = new List<Menu>();
                List<Menu> mL2 = (List<Menu>)service.ListSecondMenuLevel(m.MenuId);
                foreach (Menu m1 in mL2)
                {
                    if (m1.Children is null)
                        m1.Children = new List<Menu>();
                    service.ListPersonalMenu(service.Admin.Level, m1.MenuId);
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
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogWarning(Activity.Current?.Id, HttpContext.TraceIdentifier);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
