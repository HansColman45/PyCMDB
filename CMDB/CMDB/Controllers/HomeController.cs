using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using CMDB.DbContekst;
using Microsoft.AspNetCore.Hosting;

namespace CMDB.Controllers
{
    public class HomeController : CMDBController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CMDBContext _context;
        private readonly IWebHostEnvironment _env;
        public HomeController(ILogger<HomeController> logger, CMDBContext context, IWebHostEnvironment env):base (context, logger, env)
        {
            _logger = logger;
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Using list all for {0}", "Home");
            BuildMenu();
            ViewData["Company"]= _context.Company;
            return View();
        }
        public IActionResult LogOut()
        {
            _logger.LogDebug("Using Logout {0}", "Home");
            _context.Admin = null;
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
