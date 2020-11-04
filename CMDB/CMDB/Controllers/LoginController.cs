using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using CMDB.Models;
using CMDB.Util;
using CMDB.DbContekst;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CMDB.Controllers
{
    public class LoginController : CMDBController
    {
        private readonly CMDBContext _context;
        private readonly ILogger<LoginController> _logger;
        private readonly IWebHostEnvironment _env;

        public LoginController(CMDBContext context, ILogger<LoginController> logger, IWebHostEnvironment env):base(context,logger,env)
        {
            _context = context;
            _logger = logger;
            _env = env; 
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(IFormCollection values)
        {
            string UserID = values["UserID"];
            string Pwd = values["Pwd"];
            Admin admin;
            try
            {
                admin= _context.Login(UserID, Pwd);
                if(admin.Account == null)
                {
                    ModelState.AddModelError("", "User or password is incorrect");
                }
                if (ModelState.IsValid)
                {
                    _context.Admin = admin;
                    string stringFullUrl = @"\Home";
                    return Redirect(stringFullUrl);
                }
            }
            catch(MySqlException e)
            {
                ModelState.AddModelError("DBA", e.ToString());
                throw e;
            }
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
