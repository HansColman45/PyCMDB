using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public class MobileRepository : GenericRepository, IMobileRepository
    {
        private static readonly string table = "mobile";
        public MobileRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<MobileDTO> Activate(MobileDTO mobileDTO)
        {
            var mobile = await GetMobileById(mobileDTO.MobileId);
            mobile.active = 1;
            mobile.LastModifiedAdminId = TokenStore.AdminId;
            mobile.DeactivateReason = "";
            mobile.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.ActivateLogLine($"Mobile with type {mobileDTO.MobileType}", TokenStore.Admin.Account.UserID, table)
            });
            _context.Mobiles.Update(mobile);
            return mobileDTO;
        }
        public MobileDTO Create(MobileDTO mobileDTO)
        {
            Mobile mobile = new()
            {
                active = 1,
                CategoryId = mobileDTO.Category.Id,
                DeactivateReason = "",
                IdentityId = 1,
                IMEI = mobileDTO.IMEI,
                TypeId = mobileDTO.MobileType.TypeID,
                LastModifiedAdminId = TokenStore.AdminId,
            };
            mobile.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.CreateLogLine($"Mobile with type {mobileDTO.MobileType}", TokenStore.Admin.Account.UserID,table)
            });
            _context.Mobiles.Add(mobile);
            return mobileDTO;
        }
        public async Task<MobileDTO> Delete(MobileDTO mobileDTO, string reason)
        {
            var mobile = await GetMobileById(mobileDTO.MobileId);
            mobile.active = 0;
            mobile.LastModifiedAdminId = TokenStore.AdminId;
            mobile.DeactivateReason = reason;
            mobile.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.DeleteLogLine($"Mobile with type {mobileDTO.MobileType}", TokenStore.Admin.Account.UserID,reason, table)
            });
            _context.Mobiles.Update(mobile);
            return mobileDTO;
        }
        public async Task<MobileDTO> Update(MobileDTO mobileDTO)
        {
            var mobile = await GetMobileById(mobileDTO.MobileId);
            if(mobile.TypeId != mobileDTO.MobileType.TypeID)
            {
                var oldType = _context.AssetTypes
                    .AsNoTracking()
                    .Where(x =>x.TypeID ==  mobileDTO.MobileType.TypeID).AsNoTracking()
                    .First();
                string logtext = GenericLogLineCreator.UpdateLogLine("MobileType", $"{oldType}", $"{mobileDTO.MobileType}", TokenStore.Admin.Account.UserID,table);
                mobile.TypeId = mobileDTO.MobileType.TypeID;
                mobile.Logs.Add(new()
                {
                    LogDate = DateTime.Now,
                    LogText = logtext
                });
            }
            if(mobile.IMEI != mobileDTO.IMEI)
            {
                string logtext = GenericLogLineCreator.UpdateLogLine("IMEI",$"{mobile.IMEI}",$"{mobileDTO.IMEI}", TokenStore.Admin.Account.UserID, table);
                mobile.IMEI = mobileDTO.IMEI;
                mobile.Logs.Add(new()
                {
                    LogText = logtext,
                    LogDate = DateTime.UtcNow
                });
            }
            _context.Mobiles.Update(mobile);
            return mobileDTO;
        }
        public async Task<IEnumerable<MobileDTO>> GetAll()
        {
            var mobiles = await _context.Mobiles.AsNoTracking()
                .Include(x => x.MobileType).AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Select(x => ConvertMobile(x))
                .ToListAsync();
            return mobiles;
        }
        public async Task<IEnumerable<MobileDTO>> GetAll(string searchStr)
        {
            var mobiles = await _context.Mobiles.AsNoTracking()
                .Include(x => x.MobileType).AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Select(x => ConvertMobile(x))
                .ToListAsync();
            return mobiles;
        }
        public async Task<MobileDTO?> GetById(int id)
        {
            var mobile = await _context.Mobiles.AsNoTracking()
                .Include(x => x.MobileType).AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Where(x => x.MobileId == id).AsNoTracking()
                .Select(x => ConvertMobile(x))
                .FirstOrDefaultAsync();
            if (mobile is not null)
            {
                GetLogs(table,id,mobile);
                await GetIdenityInfo(mobile);
            }
            return mobile;
        }
        public async Task<IEnumerable<MobileDTO>> ListAllFreeMobiles()
        {
            var mobiles = await _context.Mobiles.AsNoTracking()
                 .Include(x => x.MobileType).AsNoTracking()
                 .Include(x => x.Category).AsNoTracking()
                 .Where(x => x.IdentityId == 1).AsNoTracking()
                 .Select(x => ConvertMobile(x))
                 .ToListAsync();
            return mobiles;
        }
        private async Task<Mobile> GetMobileById(int id)
        {
            var mobile = await _context.Mobiles.FirstAsync(x => x.MobileId == id);
            return mobile;
        }
        public static MobileDTO ConvertMobile(Mobile mobile) 
        {
            return new()
            {
                Active = mobile.active,
                IMEI = mobile.IMEI,
                MobileId = mobile.MobileId,
                DeactivateReason = mobile.DeactivateReason,
                LastModifiedAdminId = mobile.LastModifiedAdminId,
                Category = new()
                {
                    Active = mobile.Category.active,
                    Category = mobile.Category.Category,
                    Id = mobile.Category.Id,
                    DeactivateReason = mobile.Category.DeactivateReason,
                    LastModifiedAdminId = mobile.Category.LastModifiedAdminId,
                    Prefix = mobile.Category.Prefix,
                },
                MobileType = new()
                {
                    Active = mobile.MobileType.active,
                    LastModifiedAdminId= mobile.MobileType.LastModifiedAdminId,
                    DeactivateReason= mobile.MobileType.DeactivateReason,
                    CategoryId = mobile.MobileType.CategoryId,
                    Type = mobile.MobileType.Type,
                    TypeID = mobile.MobileType.TypeID,
                    Vendor = mobile.MobileType.Vendor
                }
            };
        }
        private async Task GetIdenityInfo(MobileDTO mobile)
        {
            mobile.Identity = await _context.Mobiles.AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Where(x => x.MobileId == mobile.MobileId)
                .Select(x => x.Identity)
                .Select(x => IdentityRepository.ConvertIdentity(x))
                .FirstAsync();
        }
        private async Task GetSubscriptionInfo(MobileDTO mobile)
        {
            /*mobile.Subscriptions = _context.Subscriptions
                .Include(x => x.SubscriptionType)
                .Include(x => x.Mobile)
                .Where(x => x.Mobile.MobileId == mobile.MobileId)
                .ToList();*/
        }
    }
}
