using CMDB.API.Interfaces;
using CMDB.Domain.DTOs;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    /// <summary>
    /// This is the repository for the Identity
    /// </summary>
    public class IdentityRepository : GenericRepository, IIdentityRepository
    {
        private IdentityRepository()
        {
        }
        private readonly string table = "identity";
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public IdentityRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        /// <inheritdoc />
        public async Task<bool> IsExisting(IdentityDTO identity)
        {
            bool result = false;
            List<Identity> idens;
            var iden = await GetById(identity.IdenId);
            if(iden is null)
            {
                idens = _context.Identities.Where(x => x.UserID == identity.UserID).ToList();
                if(idens.Count > 0)
                    result = true;
            }
            else
            {
                if(string.Compare(iden.UserID, iden.UserID) != 0)
                {
                    idens = _context.Identities.Where(x => x.UserID == identity.UserID).ToList();
                    if (idens.Count > 0)
                        result = true;
                }
            }
            return result;
        }
        /// <inheritdoc />
        public async Task<IdentityDTO> GetById(int id)
        {
            var iden = await _context.Identities.AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Include(x => x.Language).AsNoTracking()
                .Where(x => x.IdenId == id).AsNoTracking()
                .Select(x => ConvertIdentity(x))
                .FirstOrDefaultAsync();
            if (iden is not null)
            {
                GetLogs(table, id, iden);
                await GetAssignedAccounts(iden);
                await GetAssignedDevices(iden);
            }
            return iden;
        }
        /// <inheritdoc />
        public async Task LogPdfFile(string pdfFile, int id)
        {
            Identity iden = await TrackedIden(id);
            iden.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile)
            });
            _context.Identities.Update(iden);
        }
        /// <inheritdoc />
        public async Task<IEnumerable<IdentityDTO>> GetAll()
        {
            return await _context.Identities.AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Include(x => x.Language).AsNoTracking()
                .Select(x => ConvertIdentity(x)).ToListAsync();
        }
        /// <inheritdoc />
        public async Task<IEnumerable<IdentityDTO>> GetAll(string searchStr)
        {
            string searhterm = "%" + searchStr + "%";
            return await _context.Identities.AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Include(x => x.Language).AsNoTracking()
                .Where(x => EF.Functions.Like(x.Name, searhterm) || EF.Functions.Like(x.UserID, searhterm)
                    || EF.Functions.Like(x.EMail, searhterm) || EF.Functions.Like(x.Type.Type, searhterm)).AsNoTracking()
                .Select(x => ConvertIdentity(x))
                .ToListAsync();
        }
        /// <inheritdoc />
        public IdentityDTO Create(IdentityDTO identityDTO)
        {
            Identity iden = new()
            {
                active = 1,
                Company = identityDTO.Company,
                EMail = identityDTO.EMail,
                Name = identityDTO.Name,
                UserID = identityDTO.UserID,
                LastModifiedAdminId = TokenStore.AdminId,
                TypeId = identityDTO.Type.TypeId,
                LanguageCode = identityDTO.Language.Code,
                DeactivateReason = ""
            };
            string logLine = GenericLogLineCreator.CreateLogLine($"Identity with name: {identityDTO.Name}", TokenStore.Admin.Account.UserID, table);
            iden.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
                LogText = logLine
            });
            _context.Identities.Add(iden);
            return identityDTO;
        }
        /// <inheritdoc />
        public async Task<IEnumerable<IdentityDTO>> ListAllFreeIdentities(string sitePart)
        {
            return sitePart switch
            {
                "account" => await _context.Identities.AsNoTracking()
                    .Include(x => x.Accounts).AsNoTracking().AsNoTracking()
                    .Include(x => x.Type).AsNoTracking()
                    .Include(x => x.Language).AsNoTracking()
                    .Where(x => x.active == 1 && x.IdenId != 1).AsNoTracking()
                    .Where(x => !x.Accounts.Any(y => y.ValidFrom <= DateTime.UtcNow && y.ValidUntil >= DateTime.UtcNow)).AsNoTracking()
                    .Select(x => ConvertIdentity(x))
                    .ToListAsync(),
                "laptop" => await _context.Identities
                    .Include(x => x.Type)
                    .Include(x => x.Language)
                    .Where(x => !x.Devices.OfType<Laptop>().Any(y => y.active == 1 && y.IdentityId != 1))
                    .Where(x => x.active == 1).AsNoTracking()
                    .Select(x => ConvertIdentity(x)).ToListAsync(),
                "desktop" => await _context.Identities
                    .Include(x => x.Type)
                    .Include(x => x.Language)
                    .Where(x => !x.Devices.OfType<Desktop>().Any(y => y.active == 1 && y.IdentityId != 1))
                    .Where(x => x.active == 1).AsNoTracking()
                    .Select(x => ConvertIdentity(x)).ToListAsync(),
                "token" => await _context.Identities
                    .Include(x => x.Type)
                    .Include(x => x.Language)
                    .Where(x => !x.Devices.OfType<Token>().Any(y => y.active == 1 && y.IdentityId != 1))
                    .Where(x => x.active == 1).AsNoTracking()
                    .Select(x => ConvertIdentity(x)).ToListAsync(),
                "screen" => await _context.Identities
                    .Include(x => x.Type)
                    .Include(x => x.Language)
                    .Where(x => !x.Devices.OfType<Screen>().Any(y => y.active == 1 && y.IdentityId != 1))
                    .Where(x => x.active == 1).AsNoTracking()
                    .Select(x => ConvertIdentity(x)).ToListAsync(),
                "docking" => await _context.Identities
                    .Include(x => x.Type)
                    .Include(x => x.Language)
                    .Where(x => !x.Devices.OfType<Docking>().Any(y => y.active == 1 && y.IdentityId != 1))
                    .Where(x => x.active == 1).AsNoTracking()
                    .Select(x => ConvertIdentity(x)).ToListAsync(),
                "mobile" => await _context.Identities
                    .Include(x => x.Type)
                    .Include(x => x.Language)
                    .Where(x => !x.Mobiles.Any(y => y.active ==1 && y.IdentityId != 1))
                    .Where(x => x.active == 1).AsNoTracking()
                    .Select(x => ConvertIdentity(x))
                    .ToListAsync(),
                "subscription" => await _context.Identities
                    .Include(x => x.Type)
                    .Include(x => x.Language)
                    .Where(x => x.IdenId != 1)
                    .Where(x => !x.Subscriptions.Any(y => y.IdentityId != 1 && y.active == 1)).AsNoTracking()
                    .Select(x => ConvertIdentity(x))
                    .ToListAsync(),
                _ => throw new NotImplementedException($"The return for {sitePart} is not implemented")
            };
        }
        /// <inheritdoc />
        public async Task<IdentityDTO> Update(IdentityDTO dTO)
        {
            var oldIden = await TrackedIden(dTO.IdenId);
            oldIden.LastModifiedAdminId = TokenStore.Admin.AdminId;
            string logline;
            if (string.Compare(oldIden.FirstName, dTO.FirstName) != 0) 
            {
                logline = GenericLogLineCreator.UpdateLogLine("FirstName", oldIden.FirstName, dTO.FirstName, TokenStore.Admin.Account.UserID, table);
                oldIden.FirstName = dTO.FirstName;
                oldIden.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = logline,
                });
            }
            if (string.Compare(oldIden.LastName, dTO.LastName) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("LastName", oldIden.LastName, dTO.LastName, TokenStore.Admin.Account.UserID, table);
                oldIden.LastName = dTO.LastName;
                oldIden.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = logline,
                });
            }
            if (string.Compare(oldIden.EMail, dTO.EMail) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("EMail", oldIden.EMail, dTO.EMail, TokenStore.Admin.Account.UserID, table);
                oldIden.EMail = dTO.EMail;
                oldIden.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = logline
                });
            }
            if (string.Compare(oldIden.Company, dTO.Company) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("Company", oldIden.Company, dTO.Company, TokenStore.Admin.Account.UserID, table);
                oldIden.Company = dTO.Company;
                oldIden.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = logline
                });
            }
            if (oldIden.TypeId != dTO.Type.TypeId)
            {
                var oldType = _context.Types.OfType<IdentityType>()
                    .AsNoTracking()
                    .First(x => x.TypeId == oldIden.TypeId);
                logline = GenericLogLineCreator.UpdateLogLine("Type", oldType.Type, dTO.Type.Type, TokenStore.Admin.Account.UserID, table);
                oldIden.TypeId = dTO.Type.TypeId;
                oldIden.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = logline,
                });
            }
            if (string.Compare(oldIden.LanguageCode, dTO.Language.Code) != 0)
            {
                var oldLang = _context.Languages.AsNoTracking().First(x => x.Code == oldIden.LanguageCode);
                logline = GenericLogLineCreator.UpdateLogLine("Language", oldLang.Description, dTO.Language.Description, TokenStore.Admin.Account.UserID, table);
                oldIden.LanguageCode = dTO.Language.Code;
                oldIden.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = logline,
                });
            }
            if (string.Compare(oldIden.UserID, dTO.UserID) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("UserID", oldIden.UserID, dTO.UserID, TokenStore.Admin.Account.UserID, table);
                oldIden.UserID = dTO.UserID;
                oldIden.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = logline
                });
            }
            _context.Identities.Update(oldIden);
            return dTO;
        }
        /// <inheritdoc />
        public async Task AssignDevices(IdentityDTO identity, List<string> assetTags)
        {
            string ideninfo = $"Identity with name: {identity.Name}";
            var iden = await TrackedIden(identity.IdenId);
            iden.LastModifiedAdminId = TokenStore.AdminId;
            foreach (string assetTag in assetTags)
            {
                var device = await _context.Devices
                    .Include(x => x.Category)
                    .Where(x => x.AssetTag == assetTag)
                    .FirstAsync();
                var deviceinfo = $"{device.Category.Category} with {device.AssetTag}";
                device.IdentityId = identity.IdenId;
                device.LastModifiedAdminId = TokenStore.AdminId;
                device.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, ideninfo,TokenStore.Admin.Account.UserID, $"{device.Category.Category.ToLower()}"),
                    LogDate = DateTime.UtcNow
                });
                _context.Devices.Update(device);
                iden.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(ideninfo, deviceinfo, TokenStore.Admin.Account.UserID, table),
                    LogDate = DateTime.UtcNow
                });
            }
            _context.Identities.Update(iden);
        }
        /// <inheritdoc />
        public async Task ReleaseDevices(IdentityDTO identity, List<string> assetTags)
        {
            string ideninfo = $"Identity with name: {identity.Name}";
            var iden = await TrackedIden(identity.IdenId);
            iden.LastModifiedAdminId = TokenStore.AdminId;
            foreach (string assetTag in assetTags)
            {
                var device = await _context.Devices
                    .Include(x => x.Category)
                    .Where(x => x.AssetTag == assetTag)
                    .FirstAsync();
                var deviceinfo = $"{device.Category.Category} with {device.AssetTag}";
                device.IdentityId = 1;
                device.LastModifiedAdminId = TokenStore.AdminId;
                device.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, ideninfo, TokenStore.Admin.Account.UserID, $"{device.Category.Category.ToLower()}"),
                    LogDate = DateTime.UtcNow
                });
                _context.Devices.Update(device);
                iden.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(ideninfo, deviceinfo, TokenStore.Admin.Account.UserID, table),
                    LogDate = DateTime.UtcNow
                });
            }
            _context.Identities.Update(iden);
        }
        /// <inheritdoc />
        public async Task AssignMobile(IdentityDTO identity, List<int> mobileIDs) 
        {
            string ideninfo = $"Identity with name: {identity.Name}";
            var iden = await TrackedIden(identity.IdenId);
            iden.LastModifiedAdminId = TokenStore.AdminId;
            foreach (var id in mobileIDs) 
            {
                var mobile = await _context.Mobiles
                    .Include(x => x.MobileType)
                    .Where(x => x.MobileId == id).FirstAsync();
                mobile.LastModifiedAdminId = TokenStore.AdminId;
                string mobileInfo = $"mobile with type {mobile.MobileType}";
                mobile.IdentityId = iden.IdenId;
                mobile.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(mobileInfo, ideninfo, TokenStore.Admin.Account.UserID, "mobile"),
                    LogDate = DateTime.UtcNow
                });
                _context.Mobiles.Update(mobile);
                iden.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(ideninfo, mobileInfo, TokenStore.Admin.Account.UserID, table),
                    LogDate = DateTime.UtcNow
                });
                _context.Identities.Update(iden);
            }
        }
        /// <inheritdoc />
        public async Task ReleaseMobile(IdentityDTO identity, List<int> mobileIDs)
        {
            string ideninfo = $"Identity with name: {identity.Name}";
            var iden = await TrackedIden(identity.IdenId);
            iden.LastModifiedAdminId = TokenStore.AdminId;
            foreach (var id in mobileIDs)
            {
                var mobile = await _context.Mobiles
                    .Include(x => x.MobileType)
                    .Where(x => x.MobileId == id).FirstAsync();
                mobile.LastModifiedAdminId = TokenStore.AdminId;
                string mobileInfo = $"mobile with type {mobile.MobileType}";
                mobile.IdentityId = 1;
                mobile.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(mobileInfo, ideninfo, TokenStore.Admin.Account.UserID, "mobile"),
                    LogDate = DateTime.UtcNow
                });
                _context.Mobiles.Update(mobile);
                iden.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(ideninfo, mobileInfo, TokenStore.Admin.Account.UserID, table),
                    LogDate = DateTime.UtcNow
                });
                _context.Identities.Update(iden);
            }
        }
        /// <inheritdoc />
        public async Task AssignSubscription(IdentityDTO identity, List<int> subscriptionIds)
        {
            string ideninfo = $"Identity with name: {identity.Name}";
            var iden = await TrackedIden(identity.IdenId);
            iden.LastModifiedAdminId = TokenStore.AdminId;
            foreach (var id in subscriptionIds) 
            {
                var subscription = await _context.Subscriptions
                    .Include(x => x.SubscriptionType)
                    .Where(x => x.SubscriptionId == id)
                    .FirstAsync();
                subscription.LastModifiedAdminId = TokenStore.AdminId;
                string subscriptionInfo = $"Subscription: {subscription.SubscriptionType} on {subscription.PhoneNumber}";
                subscription.IdentityId = iden.IdenId;
                subscription.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(subscriptionInfo,ideninfo, TokenStore.Admin.Account.UserID,"subscription")
                });
                _context.Subscriptions.Update(subscription);
                iden.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(ideninfo, subscriptionInfo, TokenStore.Admin.Account.UserID, table)
                });
                _context.Identities.Update(iden);
            }
        }
        /// <inheritdoc />
        public async Task ReleaseSubscription(IdentityDTO identity, List<int> subscriptionIds)
        {
            string ideninfo = $"Identity with name: {identity.Name}";
            var iden = await TrackedIden(identity.IdenId);
            iden.LastModifiedAdminId = TokenStore.AdminId;
            foreach (var id in subscriptionIds)
            {
                var subscription = await _context.Subscriptions
                    .Include(x => x.SubscriptionType)
                    .Where(x => x.SubscriptionId == id).FirstAsync();
                subscription.LastModifiedAdminId = TokenStore.AdminId;
                string subscriptionInfo = $"Subscription: {subscription.SubscriptionType} on {subscription.PhoneNumber}";
                subscription.IdentityId = 1;
                subscription.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = GenericLogLineCreator.ReleaseIdentityFromDeviceLogLine(ideninfo, subscriptionInfo, TokenStore.Admin.Account.UserID, "subscription")
                });
                _context.Subscriptions.Update(subscription);
                iden.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = GenericLogLineCreator.ReleaseIdentityFromDeviceLogLine(ideninfo, subscriptionInfo, TokenStore.Admin.Account.UserID, table)
                });
                _context.Identities.Update(iden);
            }
        }
        /// <inheritdoc />
        public async Task AssignAccount(IdenAccountDTO idenAccount)
        {
            var acc = await _context.Accounts.Where(x => x.AccID == idenAccount.Account.AccID).FirstAsync();
            var iden = await TrackedIden(idenAccount.Identity.IdenId);
            string ideninfo = $"Identity with name: {iden.Name}";
            string accountinfo = $"Account with UserID: {acc.UserID}";
            IdenAccount idenacc = new()
            {
                AccountId = idenAccount.Account.AccID,
                LastModifiedAdminId = TokenStore.AdminId,
                IdentityId  = idenAccount.Identity.IdenId,
                ValidFrom = idenAccount.ValidFrom,
                ValidUntil = idenAccount.ValidUntil
            };
            _context.IdenAccounts.Add(idenacc);
            acc.LastModifiedAdminId = TokenStore.AdminId;
            acc.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.AssingAccount2IdenityLogLine(accountinfo, ideninfo, TokenStore.Admin.Account.UserID, "account"),
                LogDate = DateTime.UtcNow
            });
            _context.Accounts.Update(acc);
            iden.LastModifiedAdminId = TokenStore.AdminId;
            iden.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.AssingAccount2IdenityLogLine(ideninfo, accountinfo, TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.UtcNow
            });
            _context.Identities.Update(iden);
        }
        /// <inheritdoc />
        public async Task ReleaseAccount(IdenAccountDTO idenAccount)
        {
            var idenacc = await _context.IdenAccounts.Where(x => x.ID == idenAccount.Id).FirstAsync();
            idenacc.ValidUntil = DateTime.UtcNow.AddDays(-1);
            idenacc.LastModifiedAdminId = TokenStore.AdminId;
            _context.IdenAccounts.Update(idenacc);
            var acc = await _context.Accounts.Where(x => x.AccID == idenAccount.Account.AccID).FirstAsync();
            var iden = await TrackedIden(idenAccount.Identity.IdenId);
            string ideninfo = $"Identity with name: {iden.Name}";
            string accountinfo = $"Account with UserID: {acc.UserID}";
            acc.LastModifiedAdminId = TokenStore.AdminId;
            acc.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.ReleaseAccountFromIdentityLogLine(accountinfo, ideninfo, TokenStore.Admin.Account.UserID, "account"),
                LogDate = DateTime.UtcNow
            });
            _context.Accounts.Update(acc);
            iden.LastModifiedAdminId = TokenStore.AdminId;
            iden.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.ReleaseAccountFromIdentityLogLine(ideninfo, accountinfo, TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.UtcNow
            });
            _context.Identities.Update(iden);
        }
        /// <summary>
        /// This will convert the identity to a DTO
        /// </summary>
        /// <param name="identity"><see cref="Identity"/></param>
        /// <returns><see cref="IdentityDTO"/></returns>
        public static IdentityDTO ConvertIdentity(in Identity identity)
        {
            return new IdentityDTO()
            {
                Active = identity.active,
                Company = identity.Company,
                DeactivateReason = identity.DeactivateReason,
                EMail = identity.EMail,
                IdenId = identity.IdenId,
                LastModifiedAdminId = identity.LastModifiedAdminId,
                Name = identity.Name,
                UserID = identity.UserID,
                Language = new LanguageDTO()
                {
                    Code = identity.Language.Code,
                    Description = identity.Language.Description
                },
                Type = new TypeDTO()
                {
                    Description = identity.Type.Description,
                    Active = identity.Type.active,
                    LastModifiedAdminId = identity.Type.LastModifiedAdminId,
                    DeactivateReason = identity.Type.DeactivateReason,
                    Type = identity.Type.Type,
                    TypeId = identity.Type.TypeId,
                }
            };
        }
        /// <inheritdoc />
        public async Task<IdentityDTO> Deactivate(IdentityDTO identity, string reason)
        {
            Identity iden = await TrackedIden(identity.IdenId);
            iden.active = 0;
            iden.DeactivateReason = reason;
            iden.LastModifiedAdminId = identity.LastModifiedAdminId;
            iden.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.DeleteLogLine($"Identity with name: {identity.Name}", TokenStore.Admin.Account.UserID, reason, table),
                LogDate = DateTime.UtcNow
            });
            _context.Identities.Update(iden);
            return identity;
        }
        /// <inheritdoc />
        public async Task<IdentityDTO> Activate(IdentityDTO identity)
        {
            Identity iden = await TrackedIden(identity.IdenId);
            iden.active = 1;
            iden.DeactivateReason = "";
            iden.LastModifiedAdminId = identity.LastModifiedAdminId;
            iden.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.ActivateLogLine($"Identity with name: {identity.Name}", TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.UtcNow
            });
            _context.Identities.Update(iden);
            return identity;
        }
        private async Task<Identity> TrackedIden(int id)
        {
            return await _context.Identities.FirstAsync(x => x.IdenId == id);
        }
        private async Task GetAssignedAccounts(IdentityDTO identity)
        {
            identity.Accounts = await _context.IdenAccounts
                .Include(x => x.Identity)
                .ThenInclude(y => y.Type)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Include(x => x.Account)
                .ThenInclude(x => x.Application)
                .Include(x => x.Account)
                .ThenInclude(x => x.Type)
                .Where(x => x.IdentityId == identity.IdenId).AsNoTracking()
                .Select(x => IdenAccountRepository.ConvertIdenenty(x))
                .ToListAsync();
        }
        private async Task GetAssignedDevices(IdentityDTO identity)
        {
            identity.Devices = await _context.Devices.AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language).AsNoTracking()
                .Include(x => x.Category).AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Where(x => x.Identity.IdenId == identity.IdenId).AsNoTracking()
                .Select(x => DeviceRepository.ConvertDevice(x))
                .ToListAsync();
            identity.Mobiles = await _context.Mobiles
                .Include(x => x.Identity)
                .ThenInclude(x => x.Type)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Include(x => x.Subscriptions)
                .Include(x => x.MobileType)
                .ThenInclude(x => x.Category)
                .Where(x => x.Identity.IdenId == identity.IdenId).AsNoTracking()
                .Select(x => MobileRepository.ConvertMobile(x))
                .ToListAsync();
            identity.Subscriptions =await _context.Subscriptions
                .Include(x => x.SubscriptionType)
                .Include(x => x.Category)
                .Include(x => x.Identity)
                .Where(x => x.IdentityId == identity.IdenId).AsNoTracking()
                .Select(x =>  SubscriptionRepository.ConvertSubscription(x))
                .ToListAsync();
        }
    }
}
