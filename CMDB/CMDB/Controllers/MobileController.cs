using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using CMDB.Services;

namespace CMDB.Controllers
{
    public class MobileController : CMDBController
    {
        private readonly static string sitePart = "Mobile";
        private readonly static string table = "mobile";
        private new readonly MobileService service;
        public MobileController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
