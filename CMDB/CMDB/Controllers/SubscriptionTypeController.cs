using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;

namespace CMDB.Controllers
{
    public class SubscriptionTypeController : CMDBController
    {
        private readonly static string sitePart = "Subscription Type";
        private readonly static string table = "subscriptiontype";
        public SubscriptionTypeController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
