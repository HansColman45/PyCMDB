using Bright.ScreenPlay.Abilities;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Helpers;
using CMDB.Testing.Helpers.Devices;
using Microsoft.EntityFrameworkCore;

namespace CMDB.UI.Specflow.Abilities.Data
{
    public class DataContext: Ability
    {
        private readonly CMDBContext context;
        public DataContext() 
        {
            string connectionstring = "Server=.;Database=CMDB;User Id=sa;Password=Gr7k6VKW92dteZ5n;encrypt=false;";
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
        public async Task<Dictionary<string, Object>> DeleteAllCreatedOrUpdated(Admin admin)
        {
            return await AdminHelper.DeleteCascading(context, admin);
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
        /// <param name="active">Indicates if the identity needs to be active or not</param>
        /// <param name="admin">The admin</param>
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
        /// This function will create a desktop
        /// </summary>
        /// <param name="admin">The admin that created the desktop</param>
        /// <param name="active">Indicates if the desktop needs to be active or not</param>
        /// <returns>Desktop</returns>
        public async Task<Desktop> CreateDesktop(Admin admin, bool active = true)
        {
            Desktop desktop = await DesktopHelper.CreateSimpleDesktop(context, admin, active);
            return desktop;
        }
        /// <summary>
        /// This function will create a new Docking
        /// </summary>
        /// <param name="admin">The admin</param>
        /// <param name="active">Indicates if the Docking needs to be active or not</param>
        /// <returns>Docking</returns>
        public async Task<Docking> CreateDocking(Admin admin, bool active = true)
        {
            Docking docking = await DockingHelpers.CreateSimpleDocking(context, admin, active);
            return docking;
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
        /// This will create a new AccountType in the system
        /// </summary>
        /// <param name="admin">The admin</param>
        /// <param name="active">To indicate if accounttype needs to be active</param>
        /// <returns>AccountType</returns>
        public async Task<AccountType> CreateAccountType(Admin admin, bool active = true)
        {
            AccountType accountType = await AccountTypeHelper.CreateSimpleAccountType(context, admin, active);
            return accountType;
        }
        /// <summary>
        /// This will create an Account
        /// </summary>
        /// <param name="active">Indicates if the Account needs to be active or not</param>
        /// <param name="admin">The admin</param>
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
        /// <returns>AssetCategory</returns>
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
        /// <returns>Laptop</returns>
        public Laptop GetLaptop(string AssetTag)
        {
            var Laptop = context.Devices.OfType<Laptop>()
                .Include(x => x.Type)
                .Where(x => x.AssetTag == AssetTag).FirstOrDefault();
            return Laptop;
        }
        /// <summary>
        /// This function will create a new Laptop
        /// </summary>
        /// <returns>Laptop</returns>
        public async Task<Laptop> CreateLaptop(Admin admin, bool active = true)
        {
            return await LaptopHelper.CreateSimpleLaptop(context, admin, active);
        }
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
        /// <summary>
        /// This function will assign an Idenity to an Account
        /// </summary>
        /// <param name="identity">Identity</param>
        /// <param name="account">Account</param>
        /// <param name="admin">Admin</param>
        public async Task AssignIden2Account(Identity identity, Account account, Admin admin)
        {
            identity.LastModfiedAdmin = admin;
            account.LastModfiedAdmin = admin;
            context.IdenAccounts.Add(new()
            {
                Identity = identity,
                Account = account,
                ValidFrom = DateTime.Now.AddDays(-1),
                ValidUntil = DateTime.Now.AddYears(1),
                LastModifiedAdmin = admin
            });

            await context.SaveChangesAsync();
        }
        /// <summary>
        /// This function will create a new screen
        /// </summary>
        /// <param name="admin">The admin</param>
        /// <param name="active">Indicates if the Screen needs to be active or not</param>
        /// <returns>Screen</returns>
        public async Task<Screen> CreateMonitor(Admin admin, bool active = true)
        {
            Screen screen = await ScreenHelper.CreateScreen(context, admin, active);
            return screen;
        }
        /// <summary>
        /// This function will create a new token
        /// </summary>
        /// <param name="admin">The admin</param>
        /// <param name="active">Indicates if the Token needs to be active or not</param>
        /// <returns>Token</returns>
        public async Task<Token> CreateToken(Admin admin, bool active = true)
        {
            Token token = await TokenHelper.CreateNewToken(context, admin, active);
            return token;
        }
        /// <summary>
        /// This function will create a new AssetType
        /// </summary>
        /// <param name="admin">The admin</param>
        /// <param name="active">Indicates if the AssetType needs to be active or not</param>
        /// <returns>AssetType</returns>
        public async Task<AssetType> CreateAssetType(Admin admin, bool active = true)
        {
            AssetCategory category = context.AssetCategories.FirstOrDefault();
            AssetType assetType = await AssetTypeHelper.CreateSimpleAssetType(context, category, admin, active);
            return assetType;
        }
        /// <summary>
        /// This function will assign a given Identity to a given device
        /// </summary>
        /// <param name="admin">The admin</param>
        /// <param name="device">The device</param>
        /// <param name="identity">The identity</param>
        public async Task AssignIdentity2Device(Admin admin, Device device, Identity identity)
        {
            identity.LastModfiedAdmin = admin;
            device.LastModfiedAdmin = admin;
            identity.Devices.Add(device);
            await context.SaveChangesAsync();
        }
        /// <summary>
        /// This will create a IdenityType
        /// </summary>
        /// <param name="admin">The admin</param>
        /// <param name="active">Indicates if the Identitytype needs to be active or not</param>
        /// <returns>IdentityType</returns>
        public async Task<IdentityType> CreateIdentityType(Admin admin, bool active = true)
        {
            IdentityType type = await IdentityTypeHelper.CreateSimpleIdentityType(context, admin, active);
            return type;
        }
        /// <summary>
        /// This will create a new Mobile
        /// </summary>
        /// <param name="admin">The admin</param>
        /// <param name="active">Indicates if the Mobile needs to be active or not</param>
        /// <returns>Mobile</returns>
        public async Task<Mobile> CreateMobile(Admin admin, bool active = true)
        {
            Mobile mobile = await MobileHelper.CreateSimpleMobile(context, admin, active);
            return mobile;
        }
        /// <summary>
        /// This function will create a subscription type
        /// </summary>
        /// <param name="admin">The admin</param>
        /// <param name="actice">Indicates if the subscriptiontype needs to be active or not</param>
        /// <returns></returns>
        public async Task<SubscriptionType> CreateSubscriptionType(Admin admin, bool actice = true)
        {
            AssetCategory assetCategory = context.AssetCategories.Where(x => x.Category == "Internet Subscription").First();
            SubscriptionType subscriptionType = await SubscriptionTypeHelper.CreateSimpleSubscriptionType(context, assetCategory, admin, actice);
            return subscriptionType;
        }
        /// <summary>
        /// This function will check if there is a subscriptionType and if not create on
        /// </summary>
        /// <param name="admin">The Admin that will create it</param>
        /// <param name="type">The type</param>
        /// <returns></returns>
        public async Task<SubscriptionType> GetOrCreateSubscriptionType(Admin admin, string type)
        {
            SubscriptionType subscriptionType = context.SubscriptionTypes
                .Include(x => x.Category)
                .Where(x => x.Type == type).FirstOrDefault();
            subscriptionType ??= await CreateSubscriptionType(admin);

            return subscriptionType;
        }
    }
}
