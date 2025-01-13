using CMDB.API.Models;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public class GenericRepository
    {
        protected readonly ILogger _logger;
        protected readonly CMDBContext _context;

        public GenericRepository(CMDBContext context, ILogger logger)
        {
            _logger = logger;
            _context = context;
        }
        #region log functions
        public void GetLogs(string table, int ID, ModelDTO model)
        {
            model.Logs = table switch
            {
                "identity" => _context.Logs.AsNoTracking().Include(x => x.Identity).Where(x => x.Identity.IdenId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "identitytype" => _context.Logs.AsNoTracking().Include(x => x.Type).Where(x => x.Type.TypeId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "account" => _context.Logs.AsNoTracking().Include(x => x.Account).Where(x => x.Account.AccID == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "accounttype" => _context.Logs.AsNoTracking().Include(x => x.Type).Where(x => x.Type.TypeId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "role" => _context.Logs.AsNoTracking().Include(x => x.Role).Where(x => x.Role.RoleId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "roletype" => _context.Logs.AsNoTracking().Include(x => x.Type).Where(x => x.Type.TypeId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "assettype" => _context.Logs.AsNoTracking().Include(x => x.AssetType).Where(x => x.AssetType.TypeID == ID)
                    .OrderByDescending(x => x.LogDate).Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "menu" => _context.Logs.Include(x => x.Menu).Where(x => x.Menu.MenuId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "permissions" => _context.Logs.AsNoTracking().Include(x => x.Permission).Where(x => x.Permission.Id == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "application" => _context.Logs.AsNoTracking().Include(x => x.Application).Where(x => x.Application.AppID == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "kensington" => _context.Logs.AsNoTracking().Include(x => x.Kensington).Where(x => x.Kensington.KeyID == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "admin" => _context.Logs.AsNoTracking().Include(x => x.Admin).Where(x => x.Admin.AdminId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "mobile" => _context.Logs.AsNoTracking().Include(x => x.Mobile).Where(x => x.Mobile.MobileId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "subscriptiontype" => _context.Logs.AsNoTracking().Include(x => x.SubscriptionType).Where(x => x.SubscriptionType.Id == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "subscription" => _context.Logs.AsNoTracking().Include(x => x.Subscription).Where(x => x.Subscription.SubscriptionId == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "assetcategory" => _context.Logs.AsNoTracking().Include(x => x.Category).Where(x => x.Category.Id == ID).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                _ => throw new Exception("No get log statement created for table: " + table),
            };
        }
        public void GetLogs(string table, string AssetTag, ModelDTO model)
        {
            model.Logs = table switch
            {
                "Laptop" => _context.Logs.AsNoTracking().Include(x => x.Device).Where(x => x.Device.AssetTag == AssetTag).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "Desktop" => _context.Logs.AsNoTracking().Include(x => x.Device).Where(x => x.Device.AssetTag == AssetTag).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "Docking station" => _context.Logs.AsNoTracking().Include(x => x.Device).Where(x => x.Device.AssetTag == AssetTag).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "Token" => _context.Logs.AsNoTracking().Include(x => x.Device).Where(x => x.Device.AssetTag == AssetTag).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "Screen" => _context.Logs.AsNoTracking().Include(x => x.Device).Where(x => x.Device.AssetTag == AssetTag).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                "Monitor" => _context.Logs.AsNoTracking().Include(x => x.Device).Where(x => x.Device.AssetTag == AssetTag).OrderByDescending(x => x.LogDate)
                    .Select(x => new LogDTO() { Id = x.Id, LogDate = x.LogDate, LogText = x.LogText }).ToList(),
                _ => throw new Exception("No get log statement created for table: " + table),
            };
        }
        #endregion
    }
}
