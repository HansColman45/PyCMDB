using CMDB.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    public class HomeController : CMDBController
    {
        private readonly CMDBServices service;
        public HomeController(IWebHostEnvironment env) : base(env)
        {
            service = new();
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
