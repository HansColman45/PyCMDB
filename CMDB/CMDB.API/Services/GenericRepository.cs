using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    /// <summary>
    /// Generic repository
    /// </summary>
    public class GenericRepository
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        protected GenericRepository()
        {
        }
        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILogger _logger;
        /// <summary>
        /// The Db context
        /// </summary>
        protected readonly CMDBContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public GenericRepository(CMDBContext context, ILogger logger)
        {
            _logger = logger;
            _context = context;
        }
        #region log functions
        /// <summary>
        /// This will get the logs for the given table and ID
        /// </summary>
        /// <param name="table"></param>
        /// <param name="ID"></param>
        /// <param name="model"></param>
        /// <exception cref="Exception"></exception>
        public void GetLogs(string table, int ID, ModelDTO model)
        {
            model.Logs = table switch
            {
                "identity" => _context.Logs.AsNoTracking().Include(x => x.Identity).Where(x => x.Identity.IdenId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "identitytype" => _context.Logs.AsNoTracking().Include(x => x.Type).Where(x => x.Type.TypeId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "account" => _context.Logs.AsNoTracking().Include(x => x.Account).Where(x => x.Account.AccID == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "accounttype" => _context.Logs.AsNoTracking().Include(x => x.Type).Where(x => x.Type.TypeId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "role" => _context.Logs.AsNoTracking().Include(x => x.Role).Where(x => x.Role.RoleId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "roletype" => _context.Logs.AsNoTracking().Include(x => x.Type).Where(x => x.Type.TypeId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "assettype" => _context.Logs.AsNoTracking().Include(x => x.AssetType).Where(x => x.AssetType.TypeID == ID)
                    .OrderByDescending(x => x.LogDate).Select(x => Convert2DTO(x)).ToList(),
                "menu" => _context.Logs.Include(x => x.Menu).Where(x => x.Menu.MenuId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "application" => _context.Logs.AsNoTracking().Include(x => x.Application).Where(x => x.Application.AppID == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "kensington" => _context.Logs.AsNoTracking().Include(x => x.Kensington).Where(x => x.Kensington.KeyID == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "admin" => _context.Logs.AsNoTracking().Include(x => x.Admin).Where(x => x.Admin.AdminId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "mobile" => _context.Logs.AsNoTracking().Include(x => x.Mobile).Where(x => x.Mobile.MobileId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "subscriptiontype" => _context.Logs.AsNoTracking().Include(x => x.SubscriptionType).Where(x => x.SubscriptionType.Id == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "subscription" => _context.Logs.AsNoTracking().Include(x => x.Subscription).Where(x => x.Subscription.SubscriptionId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "assetcategory" => _context.Logs.AsNoTracking().Include(x => x.Category).Where(x => x.Category.Id == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                _ => throw new Exception("No get log statement created for table: " + table),
            };
        }
        /// <summary>
        /// This will get the logs for the given table and AssetTag
        /// </summary>
        /// <param name="table"></param>
        /// <param name="AssetTag"></param>
        /// <param name="model"></param>
        /// <exception cref="Exception"></exception>
        public void GetLogs(string table, string AssetTag, ModelDTO model)
        {
            model.Logs = table switch
            {
                "Laptop" => _context.Logs.AsNoTracking().Include(x => x.Device).Where(x => x.Device.AssetTag == AssetTag).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "Desktop" => _context.Logs.AsNoTracking().Include(x => x.Device).Where(x => x.Device.AssetTag == AssetTag).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "Docking station" => _context.Logs.AsNoTracking().Include(x => x.Device).Where(x => x.Device.AssetTag == AssetTag).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "Token" => _context.Logs.AsNoTracking().Include(x => x.Device).Where(x => x.Device.AssetTag == AssetTag).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "Screen" => _context.Logs.AsNoTracking().Include(x => x.Device).Where(x => x.Device.AssetTag == AssetTag).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                "Monitor" => _context.Logs.AsNoTracking().Include(x => x.Device).Where(x => x.Device.AssetTag == AssetTag).OrderByDescending(x => x.LogDate)
                    .Select(x => Convert2DTO(x)).ToList(),
                _ => throw new Exception("No get log statement created for table: " + table),
            };
        }
        #endregion
        /// <summary>
        /// This will convert a Log entity to a LogDTO
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static LogDTO Convert2DTO(Log log)
        {
            return new LogDTO
            {
                Id = log.Id,
                LogDate = log.LogDate,
                LogText = log.LogText
            };
        }
    }
}
