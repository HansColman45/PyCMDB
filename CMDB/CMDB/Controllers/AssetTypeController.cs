using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using CMDB.Util;
using CMDB.DbContekst;
using Microsoft.AspNetCore.Hosting;

namespace CMDB.Controllers
{
    public class AssetTypeController : CMDBController
    {
        private readonly CMDBContext _context;
        private readonly ILogger<AssetTypeController> _logger;
        private readonly static string sitePart = "Asset Typey";
        private readonly static string table = "assettype";
        private readonly IWebHostEnvironment _env;

        public AssetTypeController(CMDBContext context, ILogger<AssetTypeController> logger, IWebHostEnvironment env):base(context, logger,env)
        {
            _context = context;
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
