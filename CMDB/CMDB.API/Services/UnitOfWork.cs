using CMDB.API.Interfaces;
using CMDB.Infrastructure;

namespace CMDB.API.Services
{
    /// <summary>
    /// Unit of Work class for managing repositories and database context
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private UnitOfWork()
        {
        }
        private bool _disposed = false;
        private readonly CMDBContext _context;
        private readonly ILogger _logger;
        /// <summary>
        /// Constructor for the UnitOfWork class
        /// </summary>
        /// <param name="context"></param>
        /// <param name="factory"></param>
        /// <param name="jwtService"></param>
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
            MobileRepository = new MobileRepository(_context, _logger);
            SubscriptionRepository = new SubscriptionRepository(_context, _logger);
            SubscriptionTypeRepository = new SubscriptionTypeRepository(_context, _logger);
            KensingtonRepository = new KensingtonRepository(_context, _logger);
            RolePermissionRepository = new RolePermissionRepository(_context, _logger);
            PermissionRepository = new PermissionRepository(_context, _logger);
        }
        /// <inheritdoc/>
        public IAccountRepository AccountRepository { get; private set; }
        /// <inheritdoc/>
        public IMenuRepository MenuRepository { get; private set; }
        /// <inheritdoc/>
        public IAccountTypeRepository  AccountTypeRepository { get; private set; }
        /// <inheritdoc/>
        public IAdminRepository AdminRepository { get;private set; }
        /// <inheritdoc/>
        public IApplicationRepository ApplicationRepository{ get; private set; }
        /// <inheritdoc/>
        public IIdenAccountRepository IdenAccountRepository { get; private set; }
        /// <inheritdoc/>
        public IIdentityRepository IdentityRepository { get; private set; }
        /// <inheritdoc/>
        public IConfigurationRepository ConfigurationRepository { get; private set; }
        /// <inheritdoc/>
        public IIdentityTypeRepository IdentityTypeRepository { get; private set; }
        /// <inheritdoc/>
        public ILanguageRepository LanguageRepository { get; private set; }
        /// <inheritdoc/>
        public IDeviceRepository DeviceRepository { get; private set; }
        /// <inheritdoc/>
        public IAssetTypeRepository AssetTypeRepository { get; private set; }
        /// <inheritdoc/>
        public IAssetCategoryRepository AssetCategoryRepository { get; private set; }
        /// <inheritdoc/>
        public IMobileRepository MobileRepository { get; private set; }
        /// <inheritdoc/>
        public ISubscriptionRepository SubscriptionRepository { get; private set; }
        /// <inheritdoc/>
        public ISubscriptionTypeRepository SubscriptionTypeRepository { get; private set; }
        /// <inheritdoc/>
        public IKensingtonRepository KensingtonRepository { get; private set; }
        /// inheritdoc/>
        public IRolePermissionRepository RolePermissionRepository { get; private set; }
        /// inheritdoc/>
        public IPermissionRepository PermissionRepository { get; private set; }

        /// <inheritdoc/>
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
        /// <summary>
        /// Disposes the UnitOfWork and its resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Disposes the UnitOfWork and its resources
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
