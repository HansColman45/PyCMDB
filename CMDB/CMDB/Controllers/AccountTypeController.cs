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
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", sitePart);
            await BuildMenu();
            var types = await service.ListAll();
            ViewData["Title"] = "Accounttype overview";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewData["actionUrl"] = @"\AccountType\Search";
            return View(types);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", sitePart);
            await BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var types = await service.ListAll(search);
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
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", sitePart);
            AccountType accountType = new();
            ViewData["Title"] = "Create Accounttype";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            await BuildMenu();
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
                        await service.Create(accountType, table);
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
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit in {0}", sitePart);
            if (id == null)
                return NotFound();
            ViewData["Title"] = "Edit Accounttype";
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            var accountTypes = await service.GetAccountTypeByID((int)id);
            var accountType = accountTypes.FirstOrDefault();
            if (accountType == null)
                return NotFound();
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newType = values["Type"];
                    string newDescription = values["Description"];
                    if (service.IsExisting(accountType, newType, newDescription))
                        ModelState.AddModelError("", "Account type existing");
                    if (ModelState.IsValid)
                    {
                        await service.Update(accountType, newType, newDescription, table);
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
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", sitePart);
            if (id == null)
                return NotFound();
            ViewData["Title"] = "Delete Accounttype";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["backUrl"] = "AccountType";
            var accountTypes = await service.GetAccountTypeByID((int)id);
            var accountType = accountTypes.FirstOrDefault();
            if (accountType == null)
                return NotFound();
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        await service.Deactivate(accountType, ViewData["reason"].ToString(), table);
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
            return View(accountTypes);
        }
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Accounttype";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var accountTypes = await service.GetAccountTypeByID((int)id);
            var accountType = accountTypes.FirstOrDefault();
            if (accountType == null)
                return NotFound();
            if (service.HasAdminAccess(service.Admin, sitePart, "Activate"))
            {
                await service.Activate(accountType, table);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details in {0}", table);
            ViewData["Title"] = "Accounttype details";
            await BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
                return NotFound();
            var accountTypes = await service.GetAccountTypeByID((int)id);
            var accountType = accountTypes.FirstOrDefault();
            if (accountType == null)
                return NotFound();
            service.GetLogs(table, (int)id, accountType);
            return View(accountTypes);
        }
    }
}
