using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using CMDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace CMDB.Controllers
{
    public class IdentityController : Controller
    {
        private readonly CMDBContext _context;
        private readonly ILogger<IdentityController> _logger;
        private readonly static string table = "identity";
        private readonly static string sitePart = "Identity";
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
        public IdentityController(CMDBContext context, ILogger<IdentityController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogDebug("Using List all in {0}", table);
            var list = _context.ListAllIdenties();
            ViewData["Title"] = "Identity overview";
            BuildMenu();
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Delete");
            ViewData["ActiveAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Activate");
            ViewData["UpdateAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Update");
            ViewData["AssignAccountAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignAccount");
            ViewData["AssignDeviceAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignDevice");
            return View(list);
        }
        public IActionResult Details(int? id)
        {
            _logger.LogDebug("Using details in {0}", table);
            ViewData["Title"] = "Identity Details";
            BuildMenu();
            ViewData["InfoAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Read");
            ViewData["AddAccess"] = _context.HasAdminAccess(_context.Admin, sitePart, "Add");
            ViewData["AccountOverview"] = _context.HasAdminAccess(_context.Admin, sitePart, "AccountOverview");
            ViewData["DeviceOverview"] = _context.HasAdminAccess(_context.Admin, sitePart, "DeviceOverview");
            ViewData["AssignDevice"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignDevice");
            ViewData["AssignAccount"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignAccount");
            ViewData["ReleaseAccount"] = _context.HasAdminAccess(_context.Admin, sitePart, "ReleaseAccount");
            ViewData["ReleaseDevice"] = _context.HasAdminAccess(_context.Admin, sitePart, "ReleaseDevice");
            ViewData["LogDateFormat"] = _context.LogDateFormat;
            ViewData["DateFormat"] = _context.DateFormat;
            if (id ==null)
            {
                return NotFound();
            }
            var list = _context.GetIdentityByID((int)id);
            _context.GetAssingedDevicesForIdentity(list.ElementAt<Identity>(0));
            _context.GetAssignedAccountsForIdentity(list.ElementAt<Identity>(0));
            _context.GetLogs(table, (int)id, list.ElementAt<Identity>(0));
            if (list == null)
            {
                return NotFound();
            }
            return View(list);
        }
        public IActionResult Create(IFormCollection values)
        {
            _logger.LogDebug("Using Create in {0}", table);
            ViewData["Title"] = "Create Identity";
            List<string> errors = new List<string>();
            List<IdentityType> identityTypes = _context.ListActiveIdentityTypes();
            var listItems = new List<SelectListItem>();
            foreach (IdentityType type in identityTypes)
            {
                listItems.Add(new SelectListItem(type.Type + " " + type.Description, type.TypeID.ToString()));
            }
            ViewBag.Types = listItems;
            string FormSubmit = values["form-submitted"];
            try
            {
                if (!String.IsNullOrEmpty(FormSubmit))
                {
                    string FirstName = values["FirstName"];
                    @ViewData["FirstName"] = FirstName;
                    string LastName = values["LastName"];
                    @ViewData["LastName"] = LastName;
                    string UserID = values["UserID"];
                    @ViewData["UserID"] = UserID;
                    string Company = values["Company"];
                    ViewData["Company"] = Company;
                    string Type = values["Type"];
                    ViewData["Type"] = Type;
                    string EMail = values["EMail"];
                    ViewData["EMail"] = EMail;
                    string Language = values["Language"];
                    ViewData["Language"] = Language;
                    try
                    {
                        ValidateRequiredParams(FirstName, LastName, UserID, Company, Type, Language,EMail);
                    }
                    catch (ValidationError e)
                    {
                        errors = e.Errors;
                        return View(errors);
                    }
                    if (ModelState.IsValid)
                    {
                        _context.CreateNewIdentity(FirstName,LastName,Convert.ToInt32(Type),UserID,Company,EMail,Language,_context.Admin,table);
                        _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (MySqlException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            BuildMenu();
            return View(errors);
        }
        public IActionResult Edit(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Edit in {0}", table);
            ViewData["Title"] = "Edit Identity";
            if (id == null)
            {
                return NotFound();
            }
            List<string> errors = new List<string>();
            List<IdentityType> identityTypes = _context.ListActiveIdentityTypes();
            var listItems = new List<SelectListItem>();
            foreach (IdentityType type in identityTypes)
            {
                listItems.Add(new SelectListItem(type.Type + " " + type.Description, type.TypeID.ToString()));
            }
            ViewBag.Types = listItems;
            string FormSubmit = values["form-submitted"];
            var list = _context.GetIdentityByID((int)id);
            Identity identity = list.ElementAt<Identity>(0);
            if (identity.IdenID == 1)
            {
                ViewData["FirstName"] = identity.Name;
                ViewData["LastName"] = "";
            }
            else
            {
                ViewData["FirstName"] = identity.Name.Split(",")[0];
                ViewData["LastName"] = identity.Name.Split(",")[1];
            }
            ViewData["UserID"] = identity.UserID;
            ViewData["Company"] = identity.Company;
            ViewData["EMail"] = identity.EMail;
            ViewData["Language"] = identity.Language;
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                string NewFirstName = values["FirstName"];
                string NewLastName = values["LastName"];
                string NewUserID = values["UserID"];
                string NewCompany = values["Company"];
                string NewType = values["Type"];
                string NewLanguage = values["Language"];
                string NewEMail = values["EMail"];
                try
                {
                    ValidateRequiredParams(NewFirstName, NewLastName, NewUserID, NewCompany, NewType, NewLanguage, NewEMail);
                }
                catch (ValidationError e)
                {
                    errors = e.Errors;
                    return View(errors);
                }
                if (ModelState.IsValid)
                {
                    _context.EditIdentity(identity, NewFirstName, NewLastName, Convert.ToInt32(NewType), NewUserID, NewCompany, NewEMail, NewLanguage, _context.Admin, table);
                    _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            BuildMenu();
            return View(errors);
        }
        public IActionResult Delete(IFormCollection values, int? id)
        {
            _logger.LogDebug("Using Edit in {0}", table);
            ViewData["Title"] = "Deactivate Identity";
            if (id == null)
            {
                return NotFound();
            }
            string FormSubmit = values["form-submitted"];
            var list = _context.GetIdentityByID((int)id);
            ViewData["backUrl"] = "Identity";
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                ViewData["reason"] = values["reason"];
            }
            BuildMenu();
            return View(list);
        }
        public IActionResult AssignAccount(IFormCollection values, int? id)
        {
            ViewData["Title"] = "Assign Account";
            ViewData["AssignAccount"] = _context.HasAdminAccess(_context.Admin, sitePart, "AssignAccount");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            string FormSubmit = values["form-submitted"];
            var list = _context.GetIdentityByID((int)id);
            var listItems = new List<SelectListItem>();
            List<Account> accounts = _context.ListAllFreeAccounts();
            foreach (Account account in accounts)
            {
                listItems.Add(new SelectListItem(account.UserID + " "+account.Application.Name,account.AccID.ToString()));
            }
            ViewBag.Accounts = listItems;
            if (!String.IsNullOrEmpty(FormSubmit))
            {

            }
            if (list == null)
            {
                return NotFound();
            }
            return View(list);
        }
        private void ValidateRequiredParams(string firstName, string lastName, string UserID, string Company, string Type, string Language, string EMail)
        {
            List<string> errors = new List<string>();
            if (String.IsNullOrEmpty(firstName))
            {
                ModelState.AddModelError("","First name is required");
                errors.Add("Firstname is required please fill in the Firstname");
                _logger.LogError("Validation error: First name is required");
            }
            if (String.IsNullOrEmpty(lastName))
            {
                ModelState.AddModelError("", "First name is required");
                errors.Add("Lastname is required please fill in the LastName");
                _logger.LogError("Validation error: Last name is required");
            }
            if (String.IsNullOrEmpty(UserID))
            {
                ModelState.AddModelError("", "First name is required");
                errors.Add("UserID is required please fill in the UserID");
                _logger.LogError("Validation error: UserID is required");
            }
            if (String.IsNullOrEmpty(Company))
            {
                ModelState.AddModelError("", "First name is required");
                errors.Add("Company is required please fill in the Company");
                _logger.LogError("Validation error: Company is required");
            }
            if (String.IsNullOrEmpty(Type))
            {
                ModelState.AddModelError("", "First name is required");
                errors.Add("Type is required please select the Type");
                _logger.LogError("Validation error: Type is required");
            }
            if (String.IsNullOrEmpty(Language))
            {
                ModelState.AddModelError("", "First name is required");
                errors.Add("Language is required please select the Language");
                _logger.LogError("Validation error: Language is required");
            }
            if (String.IsNullOrEmpty(EMail))
            {
                ModelState.AddModelError("", "First name is required");
                errors.Add("Email is required please enter a Email address");
                _logger.LogError("Validation error: EMail is required");
            }
            else if (ValidateEmail(EMail))
            {
                ModelState.AddModelError("", "First name is required");
                errors.Add("Email address is incorrect");
                _logger.LogError("Validation error: EMail is not correct");
            }
            if(errors.Count >0)
            {
                throw new ValidationError(errors);
            }
        }
        private bool ValidateEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
