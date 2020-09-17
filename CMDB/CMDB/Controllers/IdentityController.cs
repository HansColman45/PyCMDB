using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace CMDB.Controllers
{
    public class IdentityController : Controller
    {
        private IConfiguration Configuration { get; }
        private readonly CMDBContext _context;
        private readonly Identity Identity = new Identity();
        private readonly static string table = "identity";
        public IdentityController(IConfiguration config, CMDBContext context)
        {
            Configuration = config;
            _context = context;
        }
        public IActionResult Index()
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            MySqlConnection Connection = new MySqlConnection(connectionString);
            var list = Identity.ListAll(Connection);
            return View(list);
        }
        public IActionResult Details(int? id)
        {
            ViewData["Title"] = "Identity Details";
            if (id ==null)
            {
                return NotFound();
            }
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            MySqlConnection Connection = new MySqlConnection(connectionString);
            var list = Identity.ListByID(Connection, (int)id);
            list.ElementAt<Identity>(0).GetAssignedDevices(Connection, (int)id);
            list.ElementAt<Identity>(0).GetLogs(table, (int)id, Connection);
            if (list == null)
            {
                return NotFound();
            }
            return View(list);
        }
        public IActionResult Create(IFormCollection values)
        {
            ViewData["Title"] = "Create Identity";
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            MySqlConnection Connection = new MySqlConnection(connectionString);
            List<string> errors = new List<string>();
            List<IdentityType> identityTypes = Identity.ActiveIdentityTypes(Connection);
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
                        ValidateRequiredParams(FirstName, LastName, UserID, Company, Type, Language);
                    }
                    catch (ValidationError e)
                    {
                        errors = e.Errors;
                        return View(errors);
                    }
                    if (ModelState.IsValid)
                    {
                        Identity.Name = FirstName + ", " + LastName;
                        Identity.UserID = UserID;
                        Identity.Company = Company;
                        Identity.TypeID = Convert.ToInt32(Type);
                        Identity.Language = Language;
                        Identity.Create(Connection,"ROOT");
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
            return View(errors);
        }
        private void ValidateRequiredParams(string firstName, string lastName, string UserID, string Company, string Type, string Language)
        {
            List<string> errors = new List<string>();
            if (String.IsNullOrEmpty(firstName))
            {
                ModelState.AddModelError("","First name is required");
                errors.Add("Firstname is required please fill in the Firstname");
            }
            if (String.IsNullOrEmpty(lastName))
            {
                ModelState.AddModelError("", "First name is required");
                errors.Add("Lastname is required please fill in the LastName");
            }
            if (String.IsNullOrEmpty(UserID))
            {
                ModelState.AddModelError("", "First name is required");
                errors.Add("UserID is required please fill in the UserID");
            }
            if (String.IsNullOrEmpty(Company))
            {
                ModelState.AddModelError("", "First name is required");
                errors.Add("Company is required please fill in the Company");
            }
            if (String.IsNullOrEmpty(Type))
            {
                ModelState.AddModelError("", "First name is required");
                errors.Add("Type is required please select the Type");
            }
            if (String.IsNullOrEmpty(Language))
            {
                ModelState.AddModelError("", "First name is required");
                errors.Add("Language is required please select the Language");
            }
            if(errors.Count >0)
            {
                throw new ValidationError(errors);
            }
        }
    }
}
