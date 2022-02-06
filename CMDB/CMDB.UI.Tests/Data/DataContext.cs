using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
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
        /// <summary>
        /// This will create a new Admin
        /// </summary>
        /// <param name="level">The level you wan to have the admin to have by default 9</param>
        /// <returns>Admin</returns>
        public async Task<Admin> CreateNewAdmin(int level = 9)
        {
            var admin = await AdminHelper.CreateCMDBAdmin(context, level);
            return admin;
        }
        /// <summary>
        /// This will delete all Created or update entities the Admin has done
        /// </summary>
        /// <param name="admin">The Admin</param>
        public async Task DeleteAllCreatedOrUpdated(Admin admin)
        {
            await AdminHelper.DeleteCascading(context, admin);
        }
        /// <summary>
        /// This function will return an Identity using the Id
        /// </summary>
        /// <param name="IdenId">The identityId</param>
        /// <returns>Identity</returns>
        public Identity GetIdentity(int IdenId)
        {
            var iden = context.Identities
                .Include(x => x.Type)
                .Include(x => x.Language)
                .Where(x => x.IdenId == IdenId)
                .FirstOrDefault();
            return iden;
        }
        /// <summary>
        /// This will create a new Identity
        /// </summary>
        /// <returns>Identity</returns>
        public async Task<Identity> CreateIdentity(Admin admin, bool active = true)
        {
            Identity identity = await IdentityHelper.CreateSimpleIdentity(context, admin, active);
            return identity;
        }
        /// <summary>
        /// This will return an AssetType using the Id
        /// </summary>
        /// <param name="AssetTypeID">The AssetTypeId</param>
        /// <returns></returns>
        public AssetType GetAssetType(int AssetTypeID)
        {
            var assetType = context.AssetTypes
                .Include(x => x.Category)
                .Where(x => x.TypeID == AssetTypeID)
                .FirstOrDefault();
            return assetType;
        }
        /// <summary>
        /// This will return the Account using the Id
        /// </summary>
        /// <param name="AccountId">The AccountId</param>
        /// <returns>Account</returns>
        public Account GetAccount(int AccountId)
        {
            var account = context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Where(x => x.AccID == AccountId)
                .FirstOrDefault();
            return account;
        }
        /// <summary>
        /// This will create an Account
        /// </summary>
        /// <returns>Account</returns>
        public async Task<Account> CreateAccount(Admin admin, bool active = true)
        {
            var Account = await AccountHelper.CreateSimpleAccountAsync(context, admin, active);
            return Account;
        }
        /// <summary>
        /// This will return abn AssetCategory using the Category
        /// </summary>
        /// <param name="category">Category</param>
        /// <returns>AssetCategory</returns>
        public AssetCategory GetAssetCategory(string category)
        {
            var Category = context.AssetCategories
                .Where(x => x.Category == category)
                .FirstOrDefault();
            return Category;
        }
        /// <summary>
        /// This will return an AssetCategory using the Id
        /// </summary>
        /// <param name="CatId">The Category Id</param>
        /// <returns></returns>
        public AssetCategory GetAssetCategory(int CatId)
        {
            var Category = context.AssetCategories
                .Where(x => x.Id == CatId)
                .FirstOrDefault();
            return Category;
        }
        /// <summary>
        /// This will check or create a new AssetType
        /// </summary>
        /// <param name="vendor">The vendor</param>
        /// <param name="type">The Type</param>
        /// <param name="category">The category</param>
        /// <returns>AssetType</returns>
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
        /// <summary>
        /// This will return a laptop using the Asset Tag
        /// </summary>
        /// <param name="AssetTag">AssetTag</param>
        /// <returns></returns>
        public Laptop GetLaptop(string AssetTag)
        {
            var Laptop = context.Laptops
                .Include(x => x.Type)
                .Where(x => x.AssetTag == AssetTag).FirstOrDefault();
            return Laptop;
        }
        /// <summary>
        /// This function will create a new Laptop
        /// </summary>
        /// <returns>Laptop</returns>
        public async Task<Laptop> CreateLaptop(Admin admin, bool active = true) => await LaptopHelper.CreateSimpleLaptop(context, admin, active);
        /// <summary>
        /// This function will return the RAM info
        /// </summary>
        /// <param name="display">The display value</param>
        /// <returns>RAM</returns>
        public RAM GetRAM(string display)
        {
            var ram = context.RAMs.Where(x => x.Display == display).FirstOrDefault();
            return ram;
        }
    }
}
