using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMDB.Controllers
{
    public class AccountTypeController : Controller
    {
        private readonly CMDBContext _context;

        public AccountTypeController(CMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var types = _context.ListAllAccountTypes();
            return View(types);
        }
    }
}
