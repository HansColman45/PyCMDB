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
    public class AssetTypeController : CMDBController
    {
        private readonly static string sitePart = "Asset Type";
        private readonly static string table = "assettype";
        private new readonly AssetTypeService service;
        public AssetTypeController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
        }
        /// <summary>
        /// The Default view
        /// </summary>
        /// <returns>View</returns>
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", sitePart);
            await BuildMenu();
            var accounts = service.ListAllAssetTypes();
            ViewData["Title"] = "Assettype overview";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            ViewData["actionUrl"] = @"\AssetType\Search";
            return View(accounts);
        }
        /// <summary>
        /// The search view
        /// </summary>
        /// <param name="search">The search param</param>
        /// <returns>View</returns>
        public async Task<IActionResult> Search(string search)
        {
            log.Debug("Using search for {0}", sitePart);
            await BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var accounts = service.ListAllAssetTypes(search);
                ViewData["Title"] = "Assettype overview";
                ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
                ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
                ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
                ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
                ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
                ViewData["actionUrl"] = @"\AssetType\Search";
                return View(accounts);
            }
            else
                return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// The create view
        /// </summary>
        /// <param name="values">The values of the create form</param>
        /// <returns>View</returns>
        public async Task<IActionResult> Create(IFormCollection values)
        {
            log.Debug("Using Create in {0}", sitePart);
            ViewData["Title"] = "Create assettype";
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            AssetType assetType = new();
            await BuildMenu();
            ViewBag.Catgories = service.ListActiveCategories();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    assetType.Vendor = values["Vendor"];
                    assetType.Type = values["Type"];
                    int id = Convert.ToInt32(values["Category"]);
                    var category = service.ListAssetCategoryByID(id);
                    assetType.Category = category.ElementAt<AssetCategory>(0);
                    if (service.IsAssetTypeExisting(assetType))
                    {
                        ModelState.AddModelError("", "Asset type already exist");
                    }
                    if (ModelState.IsValid)
                    {
                        await service.CreateNewAssetType(assetType, table);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    log.Debug("Database exception {0}", ex.ToString());
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return View(assetType);
        }
        public async Task<IActionResult> Edit(IFormCollection values, int? id)
        {
            log.Debug("Using Edit in {0}", sitePart);
            ViewData["Title"] = "Edit assettype";
            ViewData["UpdateAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Update");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var assetTypes = await service.ListById((int)id);
            AssetType assetType = assetTypes.FirstOrDefault();
            if (assetType == null)
                return NotFound();
            ViewBag.Catgories = service.ListActiveCategories();
            string FormSubmit = values["form-submitted"];
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    string newVendor = values["Vendor"];
                    string newType = values["Type"];
                    if (service.IsAssetTypeExisting(assetType, newVendor, newType))
                    {
                        ModelState.AddModelError("", "Asset type already exist");
                    }
                    if (ModelState.IsValid)
                    {
                        await service.UpdateAssetType(assetType, newVendor, newType, table);
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
            return View(assetType);
        }
        public async Task<IActionResult> Details(int? id)
        {
            log.Debug("Using details in {0}", table);
            ViewData["Title"] = "Assettype details";
            await BuildMenu();
            ViewData["InfoAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Read");
            ViewData["AddAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
                return NotFound();
            var assetTypes = await service.ListById((int)id);
            AssetType assetType = assetTypes.FirstOrDefault();
            if (assetType == null)
                return NotFound();
            service.GetLogs(table, assetType.TypeID, assetType);
            return View(assetType);
        }
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", table);
            ViewData["Title"] = "Deactivate Assettype";
            ViewData["DeleteAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Delete");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var assetTypes = await service.ListById((int)id);
            AssetType assetType = assetTypes.FirstOrDefault();
            if (assetType == null)
                return NotFound();
            string FormSubmit = values["form-submitted"];
            ViewData["backUrl"] = "AssetType";
            if (!String.IsNullOrEmpty(FormSubmit))
            {
                try
                {
                    ViewData["reason"] = values["reason"];
                    if (ModelState.IsValid)
                    {
                        await service.DeactivateAssetType(assetType, values["reason"].ToString(), table);
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
            return View(assetType);
        }
        public async Task<IActionResult> Activate(int? id)
        {
            log.Debug("Using Activate in {0}", table);
            ViewData["Title"] = "Activate Account";
            ViewData["ActiveAccess"] = service.HasAdminAccess(service.Admin, sitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var assetTypes = await service.ListById((int)id);
            AssetType assetType = assetTypes.FirstOrDefault();
            if (assetType == null)
                return NotFound();
            if (service.HasAdminAccess(service.Admin, sitePart, "Activate"))
            {
                await service.ActivateAssetType(assetType, table);
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
