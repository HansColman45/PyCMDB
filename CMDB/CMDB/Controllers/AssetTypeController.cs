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
        private new readonly AssetTypeService service;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="env"></param>
        public AssetTypeController(CMDBContext context, IWebHostEnvironment env) : base(context, env)
        {
            service = new(context);
            SitePart = "Asset Type";
            Table = "assettype";
        }
        /// <summary>
        /// The Default view
        /// </summary>
        /// <returns>View</returns>
        public async Task<IActionResult> Index()
        {
            log.Debug("Using list all for {0}", SitePart);
            await BuildMenu();
            var accounts = await service.ListAllAssetTypes();
            ViewData["Title"] = "Assettype overview";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
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
            log.Debug("Using search for {0}", SitePart);
            await BuildMenu();
            if (!String.IsNullOrEmpty(search))
            {
                ViewData["search"] = search;
                var accounts = await service.ListAllAssetTypes(search);
                ViewData["Title"] = "Assettype overview";
                ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
                ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
                ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
                ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
                ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
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
            log.Debug("Using Create in {0}", SitePart);
            ViewData["Title"] = "Create assettype";
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
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
                    assetType.Category = category.First();
                    if (service.IsAssetTypeExisting(assetType))
                    {
                        ModelState.AddModelError("", "Asset type already exist");
                    }
                    if (ModelState.IsValid)
                    {
                        await service.CreateNewAssetType(assetType, Table);
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
            log.Debug("Using Edit in {0}", SitePart);
            ViewData["Title"] = "Edit assettype";
            ViewData["UpdateAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Update");
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
                        await service.UpdateAssetType(assetType, newVendor, newType, Table);
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
            log.Debug("Using details in {0}", Table);
            ViewData["Title"] = "Assettype details";
            await BuildMenu();
            ViewData["InfoAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Read");
            ViewData["AddAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Add");
            ViewData["LogDateFormat"] = service.LogDateFormat;
            ViewData["DateFormat"] = service.DateFormat;
            if (id == null)
                return NotFound();
            var assetTypes = await service.ListById((int)id);
            AssetType assetType = assetTypes.FirstOrDefault();
            if (assetType == null)
                return NotFound();
            service.GetLogs(Table, assetType.TypeID, assetType);
            return View(assetType);
        }
        public async Task<IActionResult> Delete(IFormCollection values, int? id)
        {
            log.Debug("Using Delete in {0}", Table);
            ViewData["Title"] = "Deactivate Assettype";
            ViewData["DeleteAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Delete");
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
                        await service.DeactivateAssetType(assetType, values["reason"].ToString(), Table);
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
            log.Debug("Using Activate in {0}", Table);
            ViewData["Title"] = "Activate Account";
            ViewData["ActiveAccess"] = await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate");
            await BuildMenu();
            if (id == null)
                return NotFound();
            var assetTypes = await service.ListById((int)id);
            AssetType assetType = assetTypes.FirstOrDefault();
            if (assetType == null)
                return NotFound();
            if (await service.HasAdminAccess(TokenStore.AdminId, SitePart, "Activate"))
            {
                await service.ActivateAssetType(assetType, Table);
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
