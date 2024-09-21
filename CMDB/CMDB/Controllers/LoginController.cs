using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    public class LoginController : CMDBController
    {
        public LoginController(IWebHostEnvironment env) : base(env)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Login(IFormCollection values)
        {
            log.Debug("Using Login in {0}", "Login");
            string UserID = values["UserID"];
            string Pwd = values["Pwd"];
            Token = await service.Login(UserID, Pwd);
            TokenStore.Token = Token;
            if (Token is null)
            {
                ModelState.AddModelError("", "User or password is incorrect");
            }
            if (ModelState.IsValid)
            {
                //_context.Admin = admin;
                string stringFullUrl = @"\Home";
                return Redirect(stringFullUrl);
            }

            return View();
        }
    }
}
