using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMDB.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using CMDB.Domain.Entities;
using CMDB.Services;
using System.Threading.Tasks;

namespace CMDB.Controllers
{
    public class LaptopController : CMDBController
    {
        private readonly static string sitePart = "Laptop";
        private readonly static string table = "laptop";
        private new readonly DevicesService service;
        public LaptopController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
        }

        public async Task<IActionResult> Index()
        {
            log.Debug("Using List all in {0}", table);
            ViewData["Title"] = "Laptop overview";
            await BuildMenu();
            var Desktops = service.ListAll(sitePart);
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewData["AssignIdentityAccess"] = service.HasAdminAccess(service.Admin, sitePart, "AssignIdentity");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            ViewData["actionUrl"] = @"\Laptop\Search";
            return View(Desktops);
        }
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", sitePart);
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["Title"] = "Laptop overview";
                await BuildMenu();
                var Desktops = service.ListAll(sitePart, search);
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
                ViewData["AssignIdentityAccess"] = service.HasAdminAccess(service.Admin, sitePart, "AssignIdentity");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
                ViewData["actionUrl"] = @"\Laptop\Search";
                return View(Desktops);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", sitePart);
            ViewData["Title"] = "Create Laptop";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            await BuildMenu();
            Laptop laptop = new();
            ViewBag.Types = service.ListAssetTypes(sitePart);
            ViewBag.Rams = service.ListRams();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    laptop.AssetTag = values["AssetTag"];
                    laptop.SerialNumber = values["SerialNumber"];
                    laptop.RAM = values["RAM"];
                    int Type = Convert.ToInt32(values["Type"]);
                    var AssetType = service.ListAssetTypeById(Type);
                    laptop.Type = AssetType.ElementAt<AssetType>(0);
                    laptop.Category = AssetType.ElementAt<AssetType>(0).Category;
                    laptop.MAC = values["MAC"];
                    if (service.IsLaptopExisting(laptop))
                    {
                        ModelState.AddModelError("", "Asset already exist");
                    }
                    if (ModelState.IsValid)
                    {
                        await service.CreateNewLaptop(laptop, table);
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
            return View(laptop);
        }
        public async Task<IActionResult> Edit(IFormCollection values, string id)
        {
            log.Debug("Using Edit in {0}", sitePart);
            ViewData["Title"] = "Edit Laptop";
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            await BuildMenu();
            if (String.IsNullOrEmpty(id))
                return NotFound();
            var laptops = await service.ListLaptopByID(id);
            Laptop laptop = laptops.FirstOrDefault();
            if (laptop == null)
                return NotFound();
            ViewBag.AssetTypes = service.ListAssetTypes(sitePart);
            ViewBag.Rams = service.ListRams();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newSerialNumber = values["SerialNumber"];
                    string newRam = values["RAM"];
                    int Type = Convert.ToInt32(values["Type.TypeID"]);
                    var newAssetType = service.ListAssetTypeById(Type).ElementAt<AssetType>(0);
                    string newMAC = values["MAC"];
                    if (ModelState.IsValid)
                    {
                        await service.UpdateLaptop(laptop, newRam, newMAC, newAssetType, newSerialNumber, table);
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
            return View(laptop);
        }
        public async Task<IActionResult> Details(string id)
        {
            log.Debug("Using details in {0}", table);
            ViewData["Title"] = "Laptop details";
            await BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["IdentityOverview"] = service.HasAdminAccess(service.Admin, sitePart, "IdentityOverview");
            ViewData["AssignIdentity"] = service.HasAdminAccess(service.Admin, sitePart, "AssignIdentity");
            ViewData["ReleaseIdentity"] = service.HasAdminAccess(service.Admin, sitePart, "ReleaseIdentity");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
                return NotFound();
            var laptops = await service.ListLaptopByID(id);
            Laptop laptop = laptops.FirstOrDefault();
            if (laptop == null)
                return NotFound();
            service.GetLogs(table, laptop.AssetTag, laptop);
            service.GetAssignedIdentity(laptop);
            return View(laptop);
        }
        public async Task<IActionResult> Delete(IFormCollection values, string id)
        {
            log.Debug("Using Delete in {0}", sitePart);
            if (id == null)
                return NotFound();
            ViewData["Title"] = "Delete Laptop";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["backUrl"] = "Admin";
            await BuildMenu();
            string FormSubmit = values["form-submitted"];
            var laptops = await service.ListLaptopByID(id);
            Laptop laptop = laptops.FirstOrDefault();
            if (laptop == null)
                return NotFound();
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        await service.Deactivate(laptop, values["reason"], table);
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
            return View(laptop);
        }
        public async Task<IActionResult> Activate(string id)
        {
            log.Debug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Laptop";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var laptops = await service.ListLaptopByID(id);
            Laptop laptop = laptops.FirstOrDefault();
            if (laptop == null)
                return NotFound();
            if (service.HasAdminAccess(service.Admin, sitePart, "Activate"))
            {
                await service.Activate(laptop, table);
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
