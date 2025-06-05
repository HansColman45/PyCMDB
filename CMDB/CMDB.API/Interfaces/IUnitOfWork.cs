using CMDB.API.Services;

namespace CMDB.API.Interfaces
{
    /// <summary>
    /// Unit of Work interface
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// The account repository
        /// </summary>
        public IAccountRepository AccountRepository { get; }
        /// <summary>
        /// The menu repository
        /// </summary>
        public IMenuRepository MenuRepository { get; }
        /// <summary>
        /// The accountType repository
        /// </summary>
        public IAccountTypeRepository AccountTypeRepository { get; }
        /// <summary>
        /// The admin repository
        /// </summary>
        public IAdminRepository AdminRepository { get; }
        /// <summary>
        /// The application repository
        /// </summary>
        public IApplicationRepository ApplicationRepository { get; }
        /// <summary>
        /// The identity repository
        /// </summary>
        public IIdentityRepository IdentityRepository { get; }
        /// <summary>
        /// The identity account repository
        /// </summary>
        public IIdenAccountRepository IdenAccountRepository { get; }
        /// <summary>
        /// The configuration repository
        /// </summary>
        public IConfigurationRepository ConfigurationRepository { get; }
        /// <summary>
        /// The identity type repository
        /// </summary>
        public IIdentityTypeRepository IdentityTypeRepository { get; }
        /// <summary>
        /// The language repository
        /// </summary>
        public ILanguageRepository LanguageRepository { get; }
        /// <summary>
        /// The device repository
        /// </summary>
        public IDeviceRepository DeviceRepository { get; }
        /// <summary>
        /// The assetType repository
        /// </summary>
        public IAssetTypeRepository AssetTypeRepository { get; }
        /// <summary>
        /// The assetCategory repository
        /// </summary>
        public IAssetCategoryRepository AssetCategoryRepository { get; }
        /// <summary>
        /// The mobile repository
        /// </summary>
        public IMobileRepository MobileRepository { get; }
        /// <summary>
        /// The subscription repository
        /// </summary>
        public ISubscriptionRepository SubscriptionRepository { get; }
        /// <summary>
        /// The subscriptionType repository
        /// </summary>
        public ISubscriptionTypeRepository SubscriptionTypeRepository { get; }
        /// <summary>
        /// The Kensington repository
        /// </summary>
        public IKensingtonRepository KensingtonRepository { get; }
        /// <summary>
        /// The permission repository
        /// </summary>
        public IPermissionRepository PermissionRepository { get; }
        /// <summary>
        /// The RolePermission repository
        /// </summary>
        public IRolePermissionRepository RolePermissionRepository { get; }
        /// <summary>
        /// This will save the changes to the database
        /// </summary>
        /// <returns></returns>
        public Task SaveChangesAsync();
    }
}
