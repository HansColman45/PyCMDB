using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using CMDB.DbContekst;
using CMDB.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography.Pkcs;

namespace CMDB.Controllers
{
    public class IdentityTypeController : Controller
    {
        private readonly CMDBContext _context;
        private readonly ILogger<IdentityTypeController> _logger;
        private readonly static string table = "identitytype";
        private readonly static string sitePart = "Identity Type";
        public IdentityTypeController(CMDBContext context, ILogger<IdentityTypeController> logger)
        {
            _context = context;
            this._logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Using List all in {0}", table);
            var list = _context.ListAllIdentyTypes();
            ViewData["Title"] = "Identitytype overview";
            BuildMenu();
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            ViewData["actionUrl"] = @"\IdentityType\Search";
            return View(list);
        }
        public IActionResult Search(string search)
        {
            _logger.LogDebug("Using List all in {0}", table);
            if (!String.IsNullOrEmpty(search)) {
                ViewData["search"] = search;
                var list = _context.ListAllIdentyTypes(search);
                ViewData["Title"] = "Identitytype overview";
                BuildMenu();
                ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
                ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
                ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
                ViewData["actionUrl"] = @"\IdentityType\Search";
                return View(list);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Details(int? id)
        {
            _logger.LogDebug("Using details in {0}", table);
            ViewData["Title"] = "Identitytype Details";
            BuildMenu();
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["LogDateFormat"] = _context.LogDateFormat;
            ViewData["DateFormat"] = _context.DateFormat;
            if (id == null)
            {
                return NotFound();
            }
            var idenType = _context.GetIdenityTypeByID((int)id);
            _context.GetLogs(table, (int)id, idenType.ElementAt<IdentityType>(0));
            if (idenType == null)
            {
                return NotFound();
            }
            return View(idenType);
        }
        public IActionResult Create(IFormCollection values)
        {
            _logger.LogDebug("Using Create in {0}", table);
            ViewData["Title"] = "Create Identitytype";
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            BuildMenu();
            IdentityType idenType = new IdentityType();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    idenType.Type = values["Type"];
                    idenType.Description = values["Description"];
                    if (ModelState.IsValid)
                    {
                        _context.CreateNewIdentityType(idenType, table);
                        _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (MySqlException ex)
                {
                    _logger.LogError("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(idenType);
        }
        public IActionResult Edit(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Edit in {0}", sitePart);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Edit Identitytype";
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            BuildMenu();
            var idenType = _context.GetIdenityTypeByID((int)id);
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newTpe = values["Type"];
                    string newDescription = values["Description"];
                    if (ModelState.IsValid)
                    {
                        _context.UpdateIdenityType(idenType.ElementAt<IdentityType>(0), newTpe, newDescription, table);
                        _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (MySqlException ex)
                {
                    _logger.LogError("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(idenType.ElementAt<IdentityType>(0));
        }
        public IActionResult Delete(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Delete in {0}", sitePart);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Delete Identitytype";
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["backUrl"] = "IdentityType";
            var idenType = _context.GetIdenityTypeByID((int)id);
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        _context.DeactivateIdenityType(idenType.ElementAt<IdentityType>(0), values["reason"], table);
                        _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (MySqlException ex)
                {
                    _logger.LogError("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(idenType);
        }
        public IActionResult Activate(int? id)
        {
            _logger.LogDebug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Identitytype";
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var idenType = _context.GetIdenityTypeByID((int)id);
            if (_context.HasAdminAccess(_context.Admin, sitePart, "Activate"))
            {
                _context.ActivateIdentityType(idenType.ElementAt<IdentityType>(0), table);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        private void BuildMenu()
        {
            List<Menu> menul1 = (List<Menu>)_context.ListFirstMenuLevel();
            foreach (Menu m in menul1)
            {
                if (m.Children is null)
                    m.Children = new List<Menu>();
                List<Menu> mL2 = (List<Menu>)_context.ListSecondMenuLevel(m.MenuId);
                foreach (Menu m1 in mL2)
                {
                    if (m1.Children is null)
                        m1.Children = new List<Menu>();
                    var mL3 = _context.ListPersonalMenu(_context.Admin.Level, m1.MenuId);
                    foreach (Menu menu in mL3)
                    {
                        m1.Children.Add(new Menu()
                        {
                            MenuId = menu.MenuId,
                            Label = menu.Label,
                            URL = menu.URL
                        });
                    }
                    m.Children.Add(m1);
                }
            }
            ViewBag.Menu = menul1;
        }
    }
}
