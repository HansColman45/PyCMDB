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

namespace CMDB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CMDBContext _context;
        private void BuildMenu()
        {
            List<Menu> menul1 = (List<Menu>)_context.ListFirstMenuLevel();
            foreach (Menu m in menul1)
            {
                if (m.Children is null)
                    m.Children = new List<Menu>();
                List<Menu> mL2 = (List<Menu>)_context.ListSecondMenuLevel(m.MenuId);
                foreach (Menu m1 in mL2)
                {
                    if (m1.Children is null)
                        m1.Children = new List<Menu>();
                    var mL3 = _context.ListPersonalMenu(_context.Admin.Level, m1.MenuId);
                    foreach (Menu menu in mL3)
                    {
                        m1.Children.Add(new Menu()
                        {
                            MenuId = menu.MenuId,
                            Label = menu.Label,
                            URL = menu.URL
                        });
                    }
                    m.Children.Add(m1);
                }
            }
            ViewBag.Menu = menul1;
        }
        public HomeController(ILogger<HomeController> logger, CMDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            BuildMenu();
            ViewData["Company"]= _context.Company;
            return View();
        }
        public IActionResult LogOut()
        {
            _context.Admin = null;
            string stringFullUrl = @"\Login";
            return Redirect(stringFullUrl);
        }
        public IActionResult Privacy()
        {
            BuildMenu();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogWarning(Activity.Current?.Id, HttpContext.TraceIdentifier);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
