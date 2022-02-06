using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;

namespace CMDB.Controllers
{

    public class SubscriptionController : CMDBController
    {
        public SubscriptionController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            SitePart = "Subscription";
            Table = "subscription";
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
