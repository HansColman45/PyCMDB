using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using CMDB.DbContekst;

namespace CMDB.Controllers
{
    public class AccountTypeController : Controller
    {
        private readonly CMDBContext _context;
        private readonly ILogger<AccountTypeController> _logger;
        private readonly static string sitePart = "Account Type";
        private readonly static string table = "accounttype";
        public AccountTypeController(CMDBContext context, ILogger<AccountTypeController> _logger)
        {
            _context = context;
            this._logger = _logger;
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Using list all for {0}", sitePart);
            BuildMenu();
            var types = _context.ListAllAccountTypes();
            ViewData["Title"] = "AccountType overview";
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            ViewData["actionUrl"] = @"\AccountType\Search";
            return View(types);
        }
        public IActionResult Create(IFormCollection values)
        {
            _logger.LogDebug("Using Create in {0}", sitePart);
            AccountType accountType= new AccountType();
            ViewData["Title"] = "Create AccountType";
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            BuildMenu(); 
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    accountType.Type = values["Type"];
                    accountType.Description = values["Description"];
                    if (ModelState.IsValid)
                    {
                        _context.CreateNewAccountType(accountType, table);
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
            return View(accountType);
        }
        public IActionResult Edit(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Edit in {0}", sitePart);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Edit AccountType";
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            var accountType = _context.GetAccountTypeByID((int)id);
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newTpe = values["Type"];
                    string newDescription = values["Description"];
                    if (ModelState.IsValid)
                    {
                        _context.UpdateAccountType(accountType.ElementAt<AccountType>(0), newTpe, newDescription, table);
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
            return View(accountType.ElementAt<AccountType>(0));
        }
        public IActionResult Delete(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Delete in {0}", sitePart);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Edit AccountType";
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["backUrl"] = "AccountType";
            var accountType = _context.GetAccountTypeByID((int)id);
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        _context.DeactivateAccountType(accountType.ElementAt<AccountType>(0), ViewData["reason"].ToString(), table);
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
            return View(accountType);
        }
        public IActionResult Activate(int? id)
        {
            _logger.LogDebug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate AccountType";
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var accountType = _context.GetAccountTypeByID((int)id);
            if (_context.HasAdminAccess(_context.Admin, sitePart, "Activate"))
            {
                _context.ActivateAccountType(accountType.ElementAt<AccountType>(0), table);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Details(int? id)
        {
            _logger.LogDebug("Using details in {0}", table);
            ViewData["Title"] = "AccountType Details";
            BuildMenu();
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["LogDateFormat"] = _context.LogDateFormat;
            ViewData["DateFormat"] = _context.DateFormat;
            if (id == null)
            {
                return NotFound();
            }
            var accountTypes = _context.GetAccountTypeByID((int)id);
            _context.GetLogs(table, (int)id, accountTypes.ElementAt<AccountType>(0));
            return View(accountTypes);
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
