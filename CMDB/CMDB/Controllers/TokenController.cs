using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using CMDB.Domain.Entities;
using CMDB.Services;

namespace CMDB.Controllers
{
    public class TokenController : CMDBController
    {
        private readonly ILogger<TokenController> _logger;
        private readonly static string sitePart = "Token";
        private readonly static string table = "token";
        private readonly IWebHostEnvironment _env;
        private new readonly DevicesService service;
        public TokenController(CMDBContext context, ILogger<TokenController> logger, IWebHostEnvironment env) : base(context, logger, env)
        {
            _logger = logger;
            _env = env;
            service = new(context);
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Using List all in {0}", table);
            ViewData["Title"] = "Token overview";
            BuildMenu();
            var Desktops = service.ListAll("Token");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewData["AssignIdentityAccess"] = service.HasAdminAccess(service.Admin, sitePart, "AssignIdentity");
            ViewData["actionUrl"] = @"\Token\Search";
            return View(Desktops);
        }
        public IActionResult Search(string search)
        {
            _logger.LogDebug("Using search for {0}", sitePart);
            BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                _logger.LogDebug("Using List all in {0}", table);
                ViewData["Title"] = "Token overview";
                BuildMenu();
                var Desktops = service.ListAll(sitePart, search);
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
                ViewData["AssignIdentityAccess"] = service.HasAdminAccess(service.Admin, sitePart, "AssignIdentity");
                ViewData["actionUrl"] = @"\Token\Search";
                return View(Desktops);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Delete(IFormCollection values, string id)
        {
            _logger.LogDebug("Using Delete in {0}", sitePart);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Delete Token";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["backUrl"] = "Admin";
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            Token token = service.ListTokenByID(id).ElementAt<Token>(0);
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        service.Deactivate(token, values["reason"], table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    _logger.LogError("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(token);
        }
        public IActionResult Activate(string id)
        {
            _logger.LogDebug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Token";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            BuildMenu();
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            Token token = service.ListTokenByID(id).ElementAt<Token>(0);
            if (service.HasAdminAccess(service.Admin, sitePart, "Activate"))
            {
                service.Activate(token, table);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
