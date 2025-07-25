using CMDB.Domain.CustomExeptions;
using CMDB.Infrastructure;
using CMDB.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    /// <summary>
    /// The LoginController is used to login to the CMDB system
    /// </summary>
    public class LoginController : CMDBController
    {
        private readonly CMDBServices service;
        /// <summary>
        /// The constructor is used to inject the IWebHostEnvironment
        /// </summary>
        /// <param name="env"></param>
        public LoginController(IWebHostEnvironment env) : base(env)
        {
            service = new();
        }
        /// <summary>
        /// The home page of the applocation
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// The login page
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<IActionResult> Login(IFormCollection values)
        {
            log.Debug("Using Login in {0}", "Login");
            string UserID = values["UserID"];
            string Pwd = values["Pwd"];
            try
            {
                Token = await service.Login(UserID, Pwd);
                TokenStore.Token = Token;
                if (Token is null)
                {
                    ModelState.AddModelError("", "User or password is incorrect");
                }
            }
            catch (NotAValidSuccessCode ex)
            {
                ModelState.AddModelError("", "Login failed: Invalid credentials or request.");
                log.Warn(ex, "Login failed with 400 Bad Request for user {0}", UserID);
            }
            catch (Exception ex)
            {
                // Handle other errors
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                log.Error(ex, "Unexpected error during login for user {0}", UserID);
            }
            if (ModelState.IsValid)
            {
                string stringFullUrl = @"\Home";
                return Redirect(stringFullUrl);
            }

            return View();
        }
    }
}
