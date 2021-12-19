using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Identity = CMDB.Domain.Entities.Identity;

namespace CMDB.UI.Tests.Data
{
    public class DataContext
    {
        private readonly CMDBContext context;
        public DataContext()
        {
            string connectionstring = Settings.ConnectionString;
            var options = new DbContextOptionsBuilder<CMDBContext>()
                .UseSqlServer(connectionstring)
                .Options;
            context = new CMDBContext(options);
        }
        public Admin CreateNewAdmin(int level = 9)
        {
            var admin = AdminHelper.CreateCMDBAdmin(context, level);
            return admin;
        }
        public Identity GetIdentity(int IdenId)
        {
            var iden = context.Identities
                .Include(x => x.Type)
                .Include(x => x.Language)
                .Where(x => x.IdenId == IdenId)
                .FirstOrDefault();
            return iden;
        }
        public AssetType GetAssetType(int AssetTypeID)
        {
            var assetType = context.AssetTypes
                .Include(x => x.Category)
                .Where(x => x.TypeID == AssetTypeID)
                .FirstOrDefault();
            return assetType;
        }
        public Account GetAccount(int AccountId)
        {
            var account = context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Where(x => x.AccID == AccountId)
                .FirstOrDefault();
            return account;
        }
        public Account CreateAccount()
        {
            var Account = AccountHelper.CreateSimpleAccount(context);
            return Account;
        }
        public AssetCategory GetAssetCategory(string category)
        {
            var Category = context.AssetCategories
                .Where(x => x.Category == category)
                .FirstOrDefault();
            return Category;
        }
        public AssetCategory GetAssetCategory(int CatId)
        {
            var Category = context.AssetCategories
                .Where(x => x.Id == CatId)
                .FirstOrDefault();
            return Category;
        }
        public AssetType GetOrCreateAssetType(string vendor, string type, AssetCategory category)
        {
            var assetTypes = context.AssetTypes
                .Include(x => x.Category)
                .Where(x => x.Vendor == vendor && x.Type == type && x.CategoryId == category.Id).ToList();
            if (assetTypes.Count == 0)
            {
                // Create new
                AssetType assetType = new()
                {
                    Vendor = vendor,
                    Type = type,
                    Category = category
                };
                context.AssetTypes.Add(assetType);
                context.SaveChanges();
                return assetType;
            }
            else
                return assetTypes.FirstOrDefault();
        }
    }
}
