using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class AssetTypeService : LogService
    {
        public AssetTypeService(CMDBContext context) : base(context)
        {
        }
        public async Task<List<AssetType>> ListAllAssetTypes()
        {
            var devices = await _context.AssetTypes
                .Include(x => x.Category)
                .ToListAsync();
            return devices;
        }
        public async Task<List<AssetType>> ListAllAssetTypes(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            var devices = await _context.AssetTypes
                .Include(x => x.Category)
                .Where(x => EF.Functions.Like(x.Vendor, searhterm)
                    || EF.Functions.Like(x.Type, searhterm)
                    || EF.Functions.Like(x.Category.Category, searhterm))
                .ToListAsync();
            return devices;
        }
        public async Task<List<AssetType>> ListById(int id)
        {
            var devices = await _context.AssetTypes
                .Include(x => x.Category)
                .Where(x => x.TypeID == id)
                .ToListAsync();
            return devices;
        }
        public async Task CreateNewAssetType(AssetType assetType, string Table)
        {
            assetType.LastModfiedAdmin = Admin;
            _context.AssetTypes.Add(assetType);
            await _context.SaveChangesAsync();
            string Value = $"{assetType.Category.Category} type Vendor: {assetType.Vendor} and type {assetType.Type}";
            await LogCreate(Table, assetType.TypeID, Value);
        }
        public async Task UpdateAssetType(AssetType assetType, string Vendor, string Type, string Table)
        {
            assetType.LastModfiedAdmin = Admin;
            string OldType = assetType.Type;
            string OldVendor = assetType.Vendor;
            if (String.Compare(assetType.Vendor, Vendor) != 0)
            {
                assetType.Vendor = Vendor;
                _context.AssetTypes.Update(assetType);
                await _context.SaveChangesAsync();
                await LogUpdate(Table, assetType.TypeID, "Vendor", OldVendor, Vendor);
            }
            if (String.Compare(assetType.Type, Type) != 0)
            {
                assetType.Type = Type;
                _context.AssetTypes.Update(assetType);
                await _context.SaveChangesAsync();
                await LogUpdate(Table, assetType.TypeID, "Type", OldType, Type);
            }
        }
        public async Task DeactivateAssetType(AssetType assetType, string reason, string Table)
        {
            assetType.Active = State.Inactive;
            assetType.DeactivateReason = reason;
            assetType.LastModfiedAdmin = Admin;
            _context.AssetTypes.Update(assetType);
            await _context.SaveChangesAsync();
            string Value = $"{assetType.Category.Category} type Vendor: {assetType.Vendor} and type {assetType.Type}";
            await LogDeactivate(Table, assetType.TypeID, Value, reason);
        }
        public async Task ActivateAssetType(AssetType assetType, string Table)
        {
            assetType.Active = State.Active;
            assetType.DeactivateReason = "";
            assetType.LastModfiedAdmin = Admin;
            _context.AssetTypes.Update(assetType);
            await _context.SaveChangesAsync();
            string Value = $"{assetType.Category.Category} type Vendor: {assetType.Vendor} and type {assetType.Type}";
            await LogActivate(Table, assetType.TypeID, Value);
        }
        public bool IsAssetTypeExisting(AssetType assetType, string Vendor = "", string type = "")
        {
            bool result = false;
            List<AssetType> assetTypes = new();
            if (String.IsNullOrEmpty(Vendor) && String.IsNullOrEmpty(type))
            {
                if (String.Compare(assetType.Type, type) != 0)
                    assetTypes = _context.AssetTypes
                        .Include(x => x.Category)
                        .Where(x => x.Type == assetType.Type && x.Category.Id == assetType.Category.Id).ToList();
                if (String.Compare(assetType.Vendor, Vendor) != 0)
                    assetTypes = _context.AssetTypes
                        .Include(x => x.Category)
                        .Where(x => x.Vendor == assetType.Vendor && x.Category.Id == assetType.Category.Id).ToList();
            }
            else
            {
                if (String.Compare(assetType.Type, type) != 0)
                    assetTypes = _context.AssetTypes
                        .Include(x => x.Category)
                        .Where(x => x.Type == type && x.Category.Id == assetType.Category.Id).ToList();
                if (String.Compare(assetType.Vendor, Vendor) != 0)
                    assetTypes = _context.AssetTypes
                        .Include(x => x.Category)
                        .Where(x => x.Vendor == Vendor && x.Category.Id == assetType.Category.Id).ToList();
            }
            if (assetTypes.Count >= 1)
            {
                result = true;
            }
            return result;
        }
        public List<SelectListItem> ListActiveCategories()
        {
            List<SelectListItem> assettypes = new();
            var Categories = _context.AssetCategories.Where(x => x.active == 1 && x.Id != 3 && x.Id != 4).ToList();
            foreach (var category in Categories)
            {
                assettypes.Add(new(category.Category, category.Id.ToString()));
            }
            return assettypes;
        }
        public List<AssetCategory> ListAssetCategoryByID(int id)
        {
            var categories = _context.AssetCategories.Where(x => x.Id == id).ToList();
            return categories;
        }
    }
}
