using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
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
        public async Task<bool> IsMobileExisting(MobileDTO mobileDTO)
        {
            bool result = false;
            var oldMobile = await GetMobileById(mobileDTO.MobileId);
            if (oldMobile != null)
            {
                var mobiles = _context.Mobiles.Where(x => x.IMEI == mobileDTO.IMEI).ToList();
                if (mobiles.Count > 0)
                    result = true;
            }
            else if (oldMobile.IMEI != mobileDTO.IMEI)
            {
                var mobiles = _context.Mobiles.Where(x => x.IMEI == mobileDTO.IMEI).ToList();
                if (mobiles.Count > 0)
                    result = true;
            }
            return result;
        }
        public MobileDTO Create(MobileDTO mobileDTO)
        {
            Mobile mobile = new()
            {
                active = 1,
                CategoryId = mobileDTO.MobileType.CategoryId,
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
                    .Where(x =>x.TypeID ==  mobile.TypeId).AsNoTracking()
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
                    LogDate = DateTime.Now
                });
            }
            _context.Mobiles.Update(mobile);
            return mobileDTO;
        }
        public async Task<IEnumerable<MobileDTO>> GetAll()
        {
            var mobiles = await _context.Mobiles.AsNoTracking()
                .Include(x => x.MobileType)
                .ThenInclude(x => x.Category).AsNoTracking()
                .Include(x => x.Identity).AsNoTracking()
                .Select(x => ConvertMobile(x))
                .ToListAsync();
            return mobiles;
        }
        public async Task<IEnumerable<MobileDTO>> GetAll(string searchStr)
        {
            string searhterm = "%" + searchStr + "%";
            var mobiles = await _context.Mobiles.AsNoTracking()
                .Include(x => x.MobileType)
                .ThenInclude(x => x.Category).AsNoTracking()
                .Include(x => x.Identity).AsNoTracking()
                .Where(x => EF.Functions.Like(x.IMEI.ToString(), searhterm) || EF.Functions.Like(x.MobileType.Type, searhterm) || EF.Functions.Like(x.MobileType.Vendor, searhterm))
                .Select(x => ConvertMobile(x))
                .ToListAsync();
            return mobiles;
        }
        public async Task<MobileDTO?> GetById(int id)
        {
            var mobile = await _context.Mobiles.AsNoTracking()
                .Include(x => x.MobileType)
                .ThenInclude(x => x.Category).AsNoTracking()
                .Include(x => x.Identity).AsNoTracking()
                .Where(x => x.MobileId == id).AsNoTracking()
                .Select(x => ConvertMobile(x))
                .FirstOrDefaultAsync();
            if (mobile is not null)
            {
                GetLogs(table,id,mobile);
                await GetIdenityInfo(mobile);
                //await GetSubscriptionInfo(mobile);
            }
            return mobile;
        }
        public async Task<IEnumerable<MobileDTO>> ListAllFreeMobiles()
        {
            var mobiles = await _context.Mobiles.AsNoTracking()
                 .Include(x => x.MobileType)
                .ThenInclude(x => x.Category).AsNoTracking()
                 .Where(x => x.IdentityId == 1).AsNoTracking()
                 .Select(x => ConvertMobile(x))
                 .ToListAsync();
            return mobiles;
        }
        public async Task AssignIdentity(AssignMobileRequest request)
        {
            var mobileid = request.MobileIds.First();
            var mobile = await GetMobileById(mobileid);
            var assetType = await _context.AssetTypes.AsNoTracking().Where(x=> x.TypeID == mobile.TypeId).FirstAsync();
            Identity identity = await _context.Identities.Where(x => x.IdenId == request.IdentityId).FirstAsync();
            mobile.IdentityId = request.IdentityId;
            mobile.LastModifiedAdminId = TokenStore.AdminId;
            string mobileInfo = $"mobile with type {assetType}";
            string IdenInfo = $"Identity with name: {identity.Name}";
            mobile.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(mobileInfo, IdenInfo, TokenStore.Admin.Account.UserID, table)
            });
            _context.Mobiles.Update(mobile);
            identity.LastModifiedAdminId = TokenStore.AdminId;
            identity.Logs.Add(new()
            {
                LogDate= DateTime.Now,
                LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(IdenInfo, mobileInfo, TokenStore.Admin.Account.UserID, "identity")
            });
            _context.Identities.Update(identity);
        }
        public async Task ReleaseIdentity(AssignMobileRequest request)
        {
            var mobileid = request.MobileIds.First();
            var mobile = await GetMobileById(mobileid);
            var assetType = await _context.AssetTypes.AsNoTracking().Where(x => x.TypeID == mobile.TypeId).FirstAsync();
            Identity identity = await _context.Identities.Where(x => x.IdenId == request.IdentityId).FirstAsync();
            mobile.IdentityId = 1;
            mobile.LastModifiedAdminId = TokenStore.AdminId;
            string mobileInfo = $"mobile with type {assetType}";
            string IdenInfo = $"Identity with name: {identity.Name}";
            mobile.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(mobileInfo, IdenInfo, TokenStore.Admin.Account.UserID, table)
            });
            _context.Mobiles.Update(mobile);
            identity.LastModifiedAdminId = TokenStore.AdminId;
            identity.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(IdenInfo, mobileInfo, TokenStore.Admin.Account.UserID, "identity")
            });
            _context.Identities.Update(identity);
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
                MobileType = new()
                {
                    Active = mobile.MobileType.active,
                    LastModifiedAdminId= mobile.MobileType.LastModifiedAdminId,
                    DeactivateReason= mobile.MobileType.DeactivateReason,
                    CategoryId = mobile.MobileType.CategoryId,
                    Type = mobile.MobileType.Type,
                    TypeID = mobile.MobileType.TypeID,
                    Vendor = mobile.MobileType.Vendor,
                    AssetCategory = new()
                    {
                        Active = mobile.MobileType.Category.active,
                        Category = mobile.MobileType.Category.Category,
                        Id = mobile.MobileType.Category.Id,
                        DeactivateReason = mobile.MobileType.Category.DeactivateReason,
                        LastModifiedAdminId = mobile.MobileType.Category.LastModifiedAdminId,
                        Prefix = mobile.MobileType.Category.Prefix,
                    },
                },
                Identity = new()
                {
                    Active = mobile.Identity.active,
                    UserID = mobile.Identity.UserID,
                    Company = mobile.Identity.Company,
                    EMail  = mobile.Identity.EMail,
                    Name = mobile.Identity.Name,
                    IdenId = mobile.Identity.IdenId,
                    DeactivateReason = mobile.Identity.DeactivateReason
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
        private async Task<Mobile> GetMobileById(int id)
        {
            var mobile = await _context.Mobiles.FirstAsync(x => x.MobileId == id);
            return mobile;
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
