using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using CMDB.Services;

namespace CMDB.Controllers
{
    public class MobileController : CMDBController
    {
        private readonly ILogger<MobileController> _logger;
        private readonly static string sitePart = "Mobile";
        private readonly static string table = "mobile";
        private new readonly MobileService service;
        public MobileController(CMDBContext context, ILogger<MobileController> logger, IWebHostEnvironment env) : base(context, logger, env)
        {
            _logger = logger;
            service = new(context);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
