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
    public class CMDBController : Controller
    {
        protected CMDBServices service;
        protected CMDBContext _context;
        protected readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        protected readonly IWebHostEnvironment _env;
        protected string SitePart { get; set; }
        protected string Table { get; set; }
        protected string Token { get; set; }
        public CMDBController(CMDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            service = new(context);
        }
        protected async Task BuildMenu()
        {
            Token ??= TokenStore.Token;
            List<Menu> menul1 = (List<Menu>)await service.ListFirstMenuLevel();
            foreach (Menu m in menul1)
            {
                List<Menu> mL2 = (List<Menu>)await service.ListSecondMenuLevel(m.MenuId);
                m.Children = mL2;
                foreach (Menu m1 in mL2)
                {
                    m1.Children = await  service.ListPersonalMenu(Token,m1.MenuId);
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
