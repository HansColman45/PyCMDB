using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using CMDB.Domain.Entities;
using System;
using CMDB.Services;
using Microsoft.AspNetCore.Identity;

namespace CMDB.Controllers
{
    public class LoginController : CMDBController
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IWebHostEnvironment _env;
        public LoginController(CMDBContext context, ILogger<LoginController> logger, IWebHostEnvironment env) : base(context, logger, env)
        {
            _logger = logger;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(IFormCollection values)
        {
            _logger.LogDebug("Using Login in {0}", "Login");
            string UserID = values["UserID"];
            string Pwd = values["Pwd"];
            Admin admin;
            admin = service.Login(UserID, Pwd);
            if (admin == null)
            {
                ModelState.AddModelError("", "User or password is incorrect");
            }
            if (ModelState.IsValid)
            {
                _context.Admin = admin;
                string stringFullUrl = @"\Home";
                return Redirect(stringFullUrl);
            }

            return View();
        }
    }
}
