using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using CMDB.Util;
using CMDB.DbContekst;
using Microsoft.AspNetCore.Hosting;

namespace CMDB.Controllers
{
    public class AccountController : Controller
    {
        private readonly CMDBContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly static string sitePart = "Account";
        private readonly static string table = "account";
        private readonly IWebHostEnvironment env;
        public AccountController(ILogger<AccountController> logger, CMDBContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            this.env = env;
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Using list all for {0}",sitePart);
            BuildMenu();
            var accounts = _context.ListAllAccounts();
            ViewData["Title"] = "Account overview";
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            ViewData["AssignIdentity"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignIdentity");
            ViewData["actionUrl"] = @"\Account\Search";
            return View(accounts);
        }
        public IActionResult Search(string search)
        {
            _logger.LogDebug("Using search for {0}", sitePart);
            BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var accounts = _context.ListAllAccounts(search);
                ViewData["Title"] = "Account overview";
                ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
                ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
                ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
                ViewData["AssignIdentity"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignIdentity");
                ViewData["actionUrl"] = @"\Account\Search";
                return View(accounts);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Create(IFormCollection values)
        {
            _logger.LogDebug("Using Create in {0}", sitePart);
            ViewData["Title"] = "Create Account";
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            BuildMenu();
            ViewBag.Types = _context.ListActiveAccountTypes();
            ViewBag.Applications = _context.ListActiveApplications();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string UserID = values["UserID"];
                    ViewData["UserID"] = UserID;
                    string Type = values["type"];
                    string Application = values["Application"];
                    if (ModelState.IsValid)
                    {
                        _context.CreateNewAccount(UserID, Convert.ToInt32(Type), Convert.ToInt32(Application), table);
                        _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (MySqlException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    _logger.LogError("Database exception {0}",ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View();
        }
        public IActionResult Edit(IFormCollection values,int? id)
        {
            _logger.LogDebug("Using Edit in {0}", sitePart);
            ViewData["Title"] = "Edit Account";
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            BuildMenu();
            ViewBag.Types = _context.ListActiveAccountTypes();
            ViewBag.Applications = _context.ListActiveApplications();
            string FormSubmit = values["form-submitted"];
            if (id == null)
            {
                return NotFound();
            }
            var accounts = _context.GetAccountByID((int)id);
            Account account = accounts.ElementAt<Account>(0);
            ViewData["UserID"] = account.UserID;
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string NewUserID = values["UserID"];
                    string Type =values["type"];
                    string Application = values["Application"];
                    if (ModelState.IsValid)
                    {
                        _context.EditAccount(account, NewUserID, Convert.ToInt32(Type), Convert.ToInt32(Application), table);
                        _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (MySqlException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    _logger.LogError("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(account);
        }
        public IActionResult Details(int? id)
        {
            _logger.LogDebug("Using details in {0}", table);
            ViewData["Title"] = "Account Details";
            BuildMenu();
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["IdentityOverview"] = _context.HasAdminAccess(_context.Admin, sitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = _context.HasAdminAccess(_context.Admin, sitePart, "ReleaseIdentity");
            ViewData["LogDateFormat"] = _context.LogDateFormat;
            ViewData["DateFormat"] = _context.DateFormat;
            if (id == null)
            {
                return NotFound();
            }
            var accounts = _context.GetAccountByID((int)id);
            _context.GetLogs(table, (int)id, accounts.ElementAt<Account>(0));
            _context.GetAssignedIdentitiesForAccount(accounts.ElementAt<Account>(0));
            if (accounts == null)
            {
                return NotFound();
            }
            return View(accounts);
        }
        public IActionResult Delete(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Delete in {0}", table);
            ViewData["Title"] = "Deactivate Account";
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var accounts = _context.GetAccountByID((int)id);
            Account account = accounts.ElementAt<Account>(0);
            if (accounts == null)
            {
                return NotFound();
            }
            string FormSubmit = values["form-submitted"];
            ViewData["backUrl"] = "Account";
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
                try
                {
                    _context.DeactivateAccount(account, ViewData["reason"].ToString(), table);
                    _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (MySqlException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    _logger.LogError("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(accounts);
        }
        public IActionResult Activate(int? id)
        {
            _logger.LogDebug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Account";
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var accounts = _context.GetAccountByID((int)id);
            Account account = accounts.ElementAt<Account>(0);
            if (accounts == null)
            {
                return NotFound();
            }
            if (_context.HasAdminAccess(_context.Admin, sitePart, "Activate"))
            {
                _context.ActivateAccount(account, table);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult AssignIdentity(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Assign Identity in {0}", table);
            ViewData["Title"] = "Assign Identity";
            ViewData["AssignIdentity"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignIdentity");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var accounts = _context.GetAccountByID((int)id);
            Account account = accounts.ElementAt<Account>(0);
            ViewBag.Account = account;
            ViewBag.Identities = _context.ListAllFreeIdentities();
            if (accounts == null)
            {
                return NotFound();
            }
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                int IdenID = Convert.ToInt32(values["Identity"]);
                DateTime from = DateTime.Parse(values["ValidFrom"]);
                DateTime until = DateTime.Parse(values["ValidUntil"]);
                _context.IsPeriodOverlapping(null,id,from,until);
                if (ModelState.IsValid)
                {
                    _context.AssignIdentity2Account(account, IdenID, from, until, table);
                    _context.SaveChangesAsync();
                    return RedirectToAction("AssignFrom", "Account", new { id });
                }
            }
            return View();
        }
        public IActionResult ReleaseIdentity(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Assign Identity in {0}", table);
            ViewData["Title"] = "Release Identity";
            ViewData["AssignIdentity"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignIdentity");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var idenAccount = _context.GetIdenAccountByID((int)id);
            ViewData["backUrl"] = "Account";
            ViewData["Action"] = "ReleaseIdentity";
            ViewBag.Identity = idenAccount.ElementAt<IdenAccount>(0).Identity;
            ViewBag.Account = idenAccount.ElementAt<IdenAccount>(0).Account;
            ViewData["Name"] = idenAccount.ElementAt<IdenAccount>(0).Identity.Name;
            ViewData["AdminName"] = _context.Admin.Account.UserID;
            ViewData["ReleaseIdentity"] = _context.HasAdminAccess(_context.Admin, sitePart, "ReleaseIdentity");
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                if (ModelState.IsValid)
                {
                    _context.ReleaseIdentity4Acount(idenAccount.ElementAt<IdenAccount>(0).Account, idenAccount.ElementAt<IdenAccount>(0).Identity, (int)id,table);
                    idenAccount = _context.GetIdenAccountByID((int)id);
                    PDFGenerator PDFGenerator = new PDFGenerator
                    {
                        ITEmployee = ITPerson,
                        Singer = Employee,
                        UserID = idenAccount.ElementAt<IdenAccount>(0).Identity.UserID,
                        Language = idenAccount.ElementAt<IdenAccount>(0).Identity.Language.Code,
                        Receiver = idenAccount.ElementAt<IdenAccount>(0).Identity.Name,
                        Type = "Release"
                    };
                    PDFGenerator.SetAccontInfo(idenAccount.ElementAt<IdenAccount>(0));
                    PDFGenerator.GeneratePDF(env);
                    _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
        public IActionResult AssignFrom(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Assign Form in {0}", table);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Assign Identity";
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            var accounts = _context.GetAccountByID((int)id);
            _context.GetLogs(table, (int)id, accounts.ElementAt<Account>(0));
            _context.GetAssignedIdentitiesForAccount(accounts.ElementAt<Account>(0));
            ViewData["LogDateFormat"] = _context.LogDateFormat;
            ViewData["DateFormat"] = _context.DateFormat;
            ViewData["backUrl"] = "Account";
            ViewData["Action"] = "AssignFrom";
            ViewData["Name"] = accounts.ElementAt<Account>(0).Identities.ElementAt<IdenAccount>(0).Identity.Name;
            ViewData["AdminName"] = _context.Admin.Account.UserID;
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string Employee = values["Employee"];
                string ITPerson = values["ITEmp"];
                PDFGenerator PDFGenerator = new PDFGenerator
                {
                    ITEmployee = ITPerson,
                    Singer = Employee,
                    UserID = accounts.ElementAt<Account>(0).UserID,
                    Language = accounts.ElementAt<Account>(0).Identities.ElementAt<IdenAccount>(0).Identity.Language.Code,
                    Receiver = accounts.ElementAt<Account>(0).Identities.ElementAt<IdenAccount>(0).Identity.Name
                };
                PDFGenerator.SetAccontInfo(accounts.ElementAt<Account>(0).Identities.ElementAt<IdenAccount>(0));
                PDFGenerator.GeneratePDF(env);
            }
            return View(accounts);
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
