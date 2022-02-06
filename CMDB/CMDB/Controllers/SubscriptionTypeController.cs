using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;

namespace CMDB.Controllers
{
    public class SubscriptionTypeController : CMDBController
    {
        public SubscriptionTypeController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            SitePart = "Subscription Type";
            Table = "subscriptiontype";
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
