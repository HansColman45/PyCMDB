using CMDB.Infrastructure;

namespace CMDB.API.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private readonly CMDBContext _context;
        private readonly ILogger _logger;

        public UnitOfWork(CMDBContext context, ILoggerFactory factory, JwtService jwtService)
        {
            _context = context;
            _logger = factory.CreateLogger("logs");
            AccountRepository = new AccountRepository(_context, _logger);
            MenuRepository = new MenuRepository(_context, _logger);
            AccountTypeRepository = new AccountTypeRepository(_context, _logger);
            AdminRepository = new AdminRepository(_context, _logger, jwtService);
            ApplicationRepository = new ApplicationRepository(_context, _logger);
            IdentityRepository = new IdentityRepository(_context, _logger);
            IdenAccountRepository = new IdenAccountRepository(_context, _logger);
            ConfigurationRepository = new ConfigurationRepository(_context, _logger);
            IdentityTypeRepository = new IdentityTypeRepository(_context, _logger);
            LanguageRepository = new LanguageRepository(_context, _logger);
            DeviceRepository = new DeviceRepository(_context, _logger);
            AssetTypeRepository = new AssetTypeRepository(_context, _logger);
            AssetCategoryRepository = new AssetCategoryRepository(_context, _logger);
        }
        public IAccountRepository AccountRepository { get; private set; }
        public IMenuRepository MenuRepository { get; private set; }
        public IAccountTypeRepository  AccountTypeRepository { get; private set; }
        public IAdminRepository AdminRepository { get;private set; }
        public IApplicationRepository ApplicationRepository{ get; private set; }
        public IIdenAccountRepository IdenAccountRepository { get; private set; }
        public IIdentityRepository IdentityRepository { get; private set; }
        public IConfigurationRepository ConfigurationRepository { get; private set; }
        public IIdentityTypeRepository IdentityTypeRepository { get; private set; }
        public ILanguageRepository LanguageRepository { get; private set; }
        public IDeviceRepository DeviceRepository { get; private set; }
        public IAssetTypeRepository AssetTypeRepository { get; private set; }
        public IAssetCategoryRepository AssetCategoryRepository { get; private set; }
        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("DB error {e}", e);
                throw;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _logger.LogInformation("Dispossed context");
                }

                _disposed = true;
            }
        }
    }
}
