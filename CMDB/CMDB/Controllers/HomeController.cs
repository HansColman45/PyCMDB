using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using CMDB.Services;

namespace CMDB.Controllers
{
    public class HomeController : CMDBController
    {
        public HomeController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
        }
        public IActionResult Index()
        {
            log.Debug("Using list all for {0}", "Home");
            BuildMenu();
            ViewData["Company"] = service.Company;
            return View();
        }
        public IActionResult LogOut()
        {
            log.Debug("Using Logout {0}", "Home");
            service.Admin = null;
            string stringFullUrl = @"\Login";
            return Redirect(stringFullUrl);
        }
        public IActionResult Privacy()
        {
            BuildMenu();
            return View();
        }
    }
}
