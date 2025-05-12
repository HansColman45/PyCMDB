using CMDB.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    /// <summary>
    /// Controller for the home page
    /// </summary>
    public class HomeController : CMDBController
    {
        private readonly CMDBServices service;
        /// <summary>
        /// The home page constructor
        /// </summary>
        /// <param name="env"></param>
        public HomeController(IWebHostEnvironment env) : base(env)
        {
            service = new();
        }
        /// <summary>
        /// The home page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", "Home");
            await BuildMenu();
            ViewData["Company"] = service.Company;
            return View();
        }
        /// <summary>
        /// The home page
        /// </summary>
        /// <returns></returns>
        public IActionResult LogOut()
        {
            log.Debug("Using Logout {0}", "Home");
            string stringFullUrl = @"\Login";
            return Redirect(stringFullUrl);
        }
        /// <summary>
        /// The privacy page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Privacy()
        {
            await BuildMenu();
            return View();
        }
    }
}
