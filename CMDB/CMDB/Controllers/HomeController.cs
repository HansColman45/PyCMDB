using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using CMDB.Services;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    public class HomeController : CMDBController
    {
        public HomeController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
        }
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", "Home");
            await BuildMenu();
            ViewData["Company"] = service.Company;
            return View();
        }
        public IActionResult LogOut()
        {
            log.Debug("Using Logout {0}", "Home");
            string stringFullUrl = @"\Login";
            return Redirect(stringFullUrl);
        }
        public async Task<IActionResult> Privacy()
        {
            await BuildMenu();
            return View();
        }
    }
}
