namespace CMDB.API.Services
{
    public interface IUnitOfWork : IDisposable
    {
        public IAccountRepository AccountRepository { get; }
        public IMenuRepository MenuRepository { get; }
        public IAccountTypeRepository AccountTypeRepository { get; }
        public IAdminRepository AdminRepository { get; }
        public IApplicationRepository ApplicationRepository { get; }
        public IIdentityRepository IdentityRepository { get; }
        public IIdenAccountRepository IdenAccountRepository { get; }
        public IConfigurationRepository ConfigurationRepository { get; }
        public IIdentityTypeRepository IdentityTypeRepository { get; }
        public ILanguageRepository LanguageRepository { get; }
        public IDeviceRepository DeviceRepository { get; }
        public IAssetTypeRepository AssetTypeRepository { get; }
        public IAssetCategoryRepository AssetCategoryRepository { get; }
        public Task SaveChangesAsync();
    }
}
