using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CMDB.Controllers
{
    public class AccountController : Controller
    {
        private readonly CMDBContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly static string sitePart = "Account";
        public AccountController(ILogger<AccountController> logger, CMDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Using list all for {0}",sitePart);
            var accounts = _context.ListAllAccounts();
            return View(accounts);
        }
    }
}
