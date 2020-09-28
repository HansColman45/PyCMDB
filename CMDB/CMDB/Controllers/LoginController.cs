using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace CMDB.Controllers
{
    public class LoginController : Controller
    {
        private IConfiguration Configuration { get; }
        private readonly CMDBContext _context;

        private readonly Admin admin = new Admin();
        public LoginController(IConfiguration config, CMDBContext context)
        {
            Configuration = config;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(IFormCollection values)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            MySqlConnection Connection = new MySqlConnection(connectionString);
            string UserID = values["UserID"];
            string Pwd = values["Pwd"];
            Admin admin;
            try
            {
                admin= this.admin.Login(UserID, Pwd, Connection);
                if(admin.Account == null)
                {
                    ModelState.AddModelError("", "PWD is incorrect");
                }
            }catch(MySqlException e)
            {
                ModelState.AddModelError("DBA", e.ToString());
                throw e;
            }
            if (ModelState.IsValid)
            {
                _context.Admin = admin;
                string stringFullUrl = @"\Home";
                return Redirect(stringFullUrl);
            }
            return Redirect(nameof(Index));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
