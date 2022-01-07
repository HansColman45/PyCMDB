using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Services;

namespace CMDB.Controllers
{
    public class AccountTypeController : CMDBController
    {
        private readonly static string sitePart = "Account Type";
        private readonly static string table = "accounttype";
        private new readonly AccountTypeService service;
        public AccountTypeController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
        }
        public IActionResult Index()
        {
            log.Debug("Using list all for {0}", sitePart);
            BuildMenu();
            var types = service.ListAll();
            ViewData["Title"] = "Accounttype overview";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewData["actionUrl"] = @"\AccountType\Search";
            return View(types);
        }
        public IActionResult Search(string search)
        {
            log.Debug("Using search for {0}", sitePart);
            BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var types = service.ListAll(search);
                ViewData["Title"] = "Accounttype overview";
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
                ViewData["actionUrl"] = @"\AccountType\Search";
                return View(types);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", sitePart);
            AccountType accountType = new();
            ViewData["Title"] = "Create Accounttype";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    accountType.Type = values["Type"];
                    accountType.Description = values["Description"];
                    if (service.IsExisting(accountType))
                        ModelState.AddModelError("", "Account type existing");
                    if (ModelState.IsValid)
                    {
                        service.Create(accountType, table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(accountType);
        }
        public IActionResult Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit in {0}", sitePart);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Edit Accounttype";
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            var accountType = service.GetAccountTypeByID((int)id);
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newType = values["Type"];
                    string newDescription = values["Description"];
                    if (service.IsExisting(accountType.ElementAt<AccountType>(0), newType, newDescription))
                        ModelState.AddModelError("", "Account type existing");
                    if (ModelState.IsValid)
                    {
                        service.Update(accountType.ElementAt<AccountType>(0), newType, newDescription, table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(accountType.ElementAt<AccountType>(0));
        }
        public IActionResult Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", sitePart);
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Delete Accounttype";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["backUrl"] = "AccountType";
            var accountType = service.GetAccountTypeByID((int)id);
            BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        service.Deactivate(accountType.ElementAt<AccountType>(0), ViewData["reason"].ToString(), table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(accountType);
        }
        public IActionResult Activate(int? id)
        {
            log.Debug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Accounttype";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            BuildMenu();
            if (id == null)
            {
                return NotFound();
            }
            var accountType = service.GetAccountTypeByID((int)id);
            if (service.HasAdminAccess(service.Admin, sitePart, "Activate"))
            {
                service.Activate(accountType.ElementAt<AccountType>(0), table);
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
            log.Debug("Using details in {0}", table);
            ViewData["Title"] = "Accounttype details";
            BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
            {
                return NotFound();
            }
            var accountTypes = service.GetAccountTypeByID((int)id);
            service.GetLogs(table, (int)id, accountTypes.ElementAt<AccountType>(0));
            return View(accountTypes);
        }
    }
}
