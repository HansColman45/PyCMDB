using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using CMDB.Services;

namespace CMDB.Controllers
{
    public class HomeController : CMDBController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        public HomeController(CMDBContext context, ILogger<HomeController> logger, IWebHostEnvironment env) : base(context, logger, env)
        {
            _logger = logger;
            _env = env;
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Using list all for {0}", "Home");
            BuildMenu();
            ViewData["Company"] = service.Company;
            return View();
        }
        public IActionResult LogOut()
        {
            _logger.LogDebug("Using Logout {0}", "Home");
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
