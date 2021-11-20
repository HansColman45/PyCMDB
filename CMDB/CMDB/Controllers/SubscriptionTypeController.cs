using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;

namespace CMDB.Controllers
{
    public class SubscriptionTypeController : CMDBController
    {
        private readonly ILogger<SubscriptionTypeController> _logger;
        private readonly static string sitePart = "Subscription Type";
        private readonly static string table = "subscriptiontype";
        private readonly IWebHostEnvironment _env;
        public SubscriptionTypeController(CMDBContext context, ILogger<SubscriptionTypeController> logger, IWebHostEnvironment env) : base(context, logger, env)
        {
            _logger = logger;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
