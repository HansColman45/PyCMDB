using CMDB.Domain.Entities;
using CMDB.Infrastructure;
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
            if (string.IsNullOrEmpty(TokenStore.Token))
            {
                string stringFullUrl = @"\Login";
                return Redirect(stringFullUrl);
            }
            await BuildMenu();
            ViewData["Company"] = service.Company;
            return View();
        }
        /// <summary>
        /// The home page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> LogOut()
        {
            log.Debug("Using Logout {0}", "Home");
            Admin admin = await service.Admin();
            string token = await service.Logout(admin);
            TokenStore.Token = token;
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
