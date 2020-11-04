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
using Aspose.Html.Net;

namespace CMDB.Controllers
{
    public class AssetCategoryController : CMDBController
    {
        private readonly CMDBContext _context;
        private readonly ILogger<AssetCategoryController> _logger;
        private readonly static string sitePart = "Asset Category";
        private readonly static string table = "assetcategory";
        private readonly IWebHostEnvironment _env;
        public AssetCategoryController(CMDBContext context, ILogger<AssetCategoryController> logger, IWebHostEnvironment env) :base(context, logger, env)
        {
            _context = context;
            _logger = logger;
            this._env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
