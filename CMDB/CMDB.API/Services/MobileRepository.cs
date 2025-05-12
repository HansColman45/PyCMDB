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
                .ThenInclude(x => x.Category)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Select(x => ConvertMobile(x))
                .ToListAsync();
            return mobiles;
        }
        public async Task<IEnumerable<MobileDTO>> GetAll(string searchStr)
        {
            string searhterm = "%" + searchStr + "%";
            var mobiles = await _context.Mobiles
                .Include(x => x.MobileType)
                .ThenInclude(x => x.Category)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Where(x => EF.Functions.Like(x.IMEI.ToString(), searhterm) || EF.Functions.Like(x.MobileType.Type, searhterm) || EF.Functions.Like(x.MobileType.Vendor, searhterm))
                .AsNoTracking()
                .Select(x => ConvertMobile(x))
                .ToListAsync();
            return mobiles;
        }
        public async Task<MobileDTO> GetById(int id)
        {
            var mobile = await _context.Mobiles
                .Include(x => x.MobileType)
                .ThenInclude(x => x.Category)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Where(x => x.MobileId == id)
                .AsNoTracking()
                .Select(x => ConvertMobile(x))
                .FirstOrDefaultAsync();
            if (mobile is not null)
            {
                GetLogs(table,id,mobile);
                await GetIdenityInfo(mobile);
                await GetSubscriptionInfo(mobile);
            }
            return mobile;
        }
        public async Task<IEnumerable<MobileDTO>> ListAllFreeMobiles(string sitePart)
        {
            return sitePart switch
            {
                "identitiy" => await _context.Mobiles
                    .Include(x => x.MobileType)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Type)
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Language)
                    .Where(x => x.IdentityId == 1).AsNoTracking()
                    .Select(x => ConvertMobile(x))
                    .ToListAsync(),
                "subscription" => await _context.Mobiles
                    .Include(x => x.MobileType)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Type)
                    .Include(x => x.Identity)
                    .ThenInclude(x => x.Language)
                    .Where(x => !x.Subscriptions.Any(y => y.active ==1 && y.IdentityId != 1))
                    .Where(x => x.IdentityId > 1).AsNoTracking()
                    .Select(x => ConvertMobile(x))
                    .ToListAsync(),
                _ => throw new NotImplementedException($"The return for {sitePart} is not implemented")
            };
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
        public async Task AssignSubscription(AssignMobileSubscriptionRequest request)
        {
            var mobile = await GetMobileById(request.MobileId);
            var assetType = await _context.AssetTypes.AsNoTracking().Where(x => x.TypeID == mobile.TypeId).FirstAsync();
            string mobileInfo = $"mobile with type {assetType}";
            var subscription = await _context.Subscriptions
                   .Include(x => x.SubscriptionType)
                   .Where(x => x.SubscriptionId == request.SubscriptionId)
                   .FirstAsync();
            string subscriptionInfo = $"Subscription: {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            subscription.LastModifiedAdminId = TokenStore.AdminId;
            subscription.MobileId = mobile.MobileId;
            subscription.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(subscriptionInfo, mobileInfo, TokenStore.Admin.Account.UserID, "subscription")
            });
            _context.Subscriptions.Update(subscription);
            mobile.LastModifiedAdminId = TokenStore.AdminId;
            mobile.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(mobileInfo, subscriptionInfo, TokenStore.Admin.Account.UserID, table)
            });
            _context.Mobiles.Update(mobile);
        }
        public async Task ReleaseSubscription(AssignMobileSubscriptionRequest request)
        {
            var mobile = await GetMobileById(request.MobileId);
            var assetType = await _context.AssetTypes.AsNoTracking().Where(x => x.TypeID == mobile.TypeId).FirstAsync();
            string mobileInfo = $"mobile with type {assetType}";
            int subid = request.SubscriptionId;
            var subscription = await _context.Subscriptions
                   .Include(x => x.SubscriptionType)
                   .Where(x => x.SubscriptionId == subid)
                   .FirstAsync();
            string subscriptionInfo = $"Subscription: {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            subscription.LastModifiedAdminId = TokenStore.AdminId;
            subscription.MobileId = null;
            subscription.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.ReleaseIdentityFromDeviceLogLine(subscriptionInfo, mobileInfo, TokenStore.Admin.Account.UserID, "subscription")
            });
            _context.Subscriptions.Update(subscription);
            mobile.LastModifiedAdminId = TokenStore.AdminId;
            mobile.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(mobileInfo, subscriptionInfo, TokenStore.Admin.Account.UserID, table)
            });
            _context.Mobiles.Update(mobile);
        }
        public async Task LogPdfFile(string pdfFile, int id)
        {
            var mobile = await GetMobileById(id);
            mobile.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile),
                LogDate = DateTime.Now
            });
            _context.Mobiles.Update(mobile);
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
                    DeactivateReason = mobile.Identity.DeactivateReason,
                    LastModifiedAdminId = mobile.Identity.LastModifiedAdminId,
                    Type = new()
                    {
                        Active = mobile.Identity.Type.active,
                        LastModifiedAdminId = mobile.Identity.Type.LastModifiedAdminId,
                        DeactivateReason = mobile.Identity.Type.DeactivateReason,
                        Description = mobile.Identity.Type.Description, 
                        TypeId = mobile.Identity.Type.TypeId,
                        Type = mobile.Identity.Type.Type
                    },
                    Language = new() 
                    { 
                        Code = mobile.Identity.Language.Code,
                        Description = mobile.Identity.Language.Description
                    }
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
            mobile.Subscription = await _context.Subscriptions
                .Include(x => x.Category)
                .Include(x => x.SubscriptionType)
                .Include(x => x.Mobile)
                .Where(x => x.Mobile.MobileId == mobile.MobileId).AsNoTracking()
                .Select(x => SubscriptionRepository.ConvertSubscription(x)).FirstOrDefaultAsync();
        }
    }
}
