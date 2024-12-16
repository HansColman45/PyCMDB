using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CMDB.API.Services
{
    public class IdentityRepository : GenericRepository, IIdentityRepository
    {
        private readonly string table = "identity";
        public IdentityRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
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
        public async Task<IdentityDTO?> GetById(int id)
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
                await GetAssignedAccounts(id);
                await GetAssignedDevices(iden);
            }
            return iden;
        }
        public async Task LogPdfFile(string pdfFile, int id)
        {
            Identity iden = await _context.Identities.Where(x => x.IdenId == id).FirstAsync();
            iden.Logs.Add(new()
            {
                LogDate = DateTime.Now,
                LogText = GenericLogLineCreator.LogPDFFileLine(pdfFile)
            });
            _context.Identities.Update(iden);
        }
        public async Task<IEnumerable<IdentityDTO>> GetAll()
        {
            return await _context.Identities.AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Include(x => x.Language).AsNoTracking()
                .Select(x => ConvertIdentity(x)).ToListAsync();
        }
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
                LogDate = DateTime.Now,
                LogText = logLine
            });
            _context.Identities.Add(iden);
            return identityDTO;
        }
        public async Task<IEnumerable<IdentityDTO>> ListAllFreeIdentities()
        {
            var identities = await _context.Identities.AsNoTracking()
                .Include(x => x.Accounts).AsNoTracking().AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Include(x => x.Language).AsNoTracking()
                .Where(x => x.active == 1 && x.IdenId != 1).AsNoTracking()
                .Where(x => !x.Accounts.Any(y => y.ValidFrom <= DateTime.Now && y.ValidUntil >= DateTime.Now)).AsNoTracking()
                .Select(x => ConvertIdentity(x))
                .ToListAsync();
            return identities;
        }
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
                    LogDate = DateTime.Now,
                    LogText = logline,
                });
            }
            if (string.Compare(oldIden.LastName, dTO.LastName) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("LastName", oldIden.LastName, dTO.LastName, TokenStore.Admin.Account.UserID, table);
                oldIden.LastName = dTO.LastName;
                oldIden.Logs.Add(new()
                {
                    LogDate = DateTime.Now,
                    LogText = logline,
                });
            }
            if (string.Compare(oldIden.EMail, dTO.EMail) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("EMail", oldIden.EMail, dTO.EMail, TokenStore.Admin.Account.UserID, table);
                oldIden.EMail = dTO.EMail;
                oldIden.Logs.Add(new()
                {
                    LogDate = DateTime.Now,
                    LogText = logline
                });
            }
            if (string.Compare(oldIden.Company, dTO.Company) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("Company", oldIden.Company, dTO.Company, TokenStore.Admin.Account.UserID, table);
                oldIden.Company = dTO.Company;
                oldIden.Logs.Add(new()
                {
                    LogDate = DateTime.Now,
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
                    LogDate = DateTime.Now,
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
                    LogDate = DateTime.Now,
                    LogText = logline,
                });
            }
            if (string.Compare(oldIden.UserID, dTO.UserID) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("UserID", oldIden.UserID, dTO.UserID, TokenStore.Admin.Account.UserID, table);
                oldIden.UserID = dTO.UserID;
                oldIden.Logs.Add(new()
                {
                    LogDate = DateTime.Now,
                    LogText = logline
                });
            }
            _context.Identities.Update(oldIden);
            return dTO;
        }
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
                    LogDate = DateTime.Now
                });
                _context.Devices.Update(device);
                iden.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(ideninfo, deviceinfo, TokenStore.Admin.Account.UserID, table),
                    LogDate = DateTime.Now
                });
            }
            _context.Identities.Update(iden);
        }
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
                    LogDate = DateTime.Now
                });
                _context.Devices.Update(device);
                iden.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(ideninfo, deviceinfo, TokenStore.Admin.Account.UserID, table),
                    LogDate = DateTime.Now
                });
            }
            _context.Identities.Update(iden);
        }
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
                    LogDate = DateTime.Now
                });
                _context.Mobiles.Update(mobile);
                iden.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.AssingDevice2IdenityLogLine(ideninfo, mobileInfo, TokenStore.Admin.Account.UserID, table),
                    LogDate = DateTime.Now
                });
                _context.Identities.Update(iden);
            }
        }
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
                    LogDate = DateTime.Now
                });
                _context.Mobiles.Update(mobile);
                iden.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(ideninfo, mobileInfo, TokenStore.Admin.Account.UserID, table),
                    LogDate = DateTime.Now
                });
                _context.Identities.Update(iden);
            }
        }
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
                LogDate = DateTime.Now
            });
            _context.Accounts.Update(acc);
            iden.LastModifiedAdminId = TokenStore.AdminId;
            iden.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.AssingAccount2IdenityLogLine(ideninfo, accountinfo, TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.Now
            });
            _context.Identities.Update(iden);
        }
        public async Task ReleaseAccount(IdenAccountDTO idenAccount)
        {
            var idenacc = await _context.IdenAccounts.Where(x => x.ID == idenAccount.Id).FirstAsync();
            idenacc.ValidUntil = DateTime.Now.AddDays(-1);
            idenacc.LastModifiedAdminId = TokenStore.AdminId;
            _context.IdenAccounts.Update(idenacc);
            var acc = await _context.Accounts.Where(x => x.AccID == idenAccount.Account.AccID).FirstAsync();
            var iden = await TrackedIden(idenAccount.Identity.IdenId);
            string ideninfo = $"Identity with name: {iden.Name}";
            string accountinfo = $"Account with UserID: {acc.UserID}";

        }
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
        public static Identity ConvertDTO(IdentityDTO dto)
        {
            var iden = new Identity()
            {
                active = dto.Active,
                Company = dto.Company,
                DeactivateReason = dto.DeactivateReason,
                EMail = dto.EMail,
                IdenId = dto.IdenId,
                Name = dto.Name,
                LastModifiedAdminId = dto.LastModifiedAdminId,
                UserID = dto.UserID,
                TypeId = dto.Type.TypeId,
                Type = new IdentityType()
                {
                    DeactivateReason = dto.Type.DeactivateReason,
                    Type = dto.Type.Type,
                    Description = dto.Type.Description,
                    LastModifiedAdminId = dto.Type.LastModifiedAdminId,
                    active = dto.Type.Active,
                    TypeId = dto.Type.TypeId
                },
                Language = new Language()
                {
                    Code = dto.Language.Code,
                    Description = dto.Language.Description
                }
            };
            return iden;
        }
        public async Task<IdentityDTO> Deactivate(IdentityDTO identity, string reason)
        {
            Identity iden = await TrackedIden(identity.IdenId);
            iden.active = 0;
            iden.DeactivateReason = reason;
            iden.LastModifiedAdminId = identity.LastModifiedAdminId;
            iden.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.DeleteLogLine($"Identity with name: {identity.Name}", TokenStore.Admin.Account.UserID, reason, table),
                LogDate = DateTime.Now
            });
            _context.Identities.Update(iden);
            return identity;
        }
        public async Task<IdentityDTO> Activate(IdentityDTO identity)
        {
            Identity iden = await TrackedIden(identity.IdenId);
            iden.active = 1;
            iden.DeactivateReason = "";
            iden.LastModifiedAdminId = identity.LastModifiedAdminId;
            iden.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.ActivateLogLine($"Identity with name: {identity.Name}", TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.Now
            });
            _context.Identities.Update(iden);
            return identity;
        }
        private async Task<Identity> TrackedIden(int id)
        {
            return await _context.Identities.FirstAsync(x => x.IdenId == id);
        }
        private async Task GetAssignedAccounts(int id)
        {
            var accounts = await _context.Identities.AsNoTracking()
                .Include(x => x.Language).AsNoTracking()
                .Include(x => x.Accounts)
                .ThenInclude(d => d.Account).AsNoTracking()
                .SelectMany(x => x.Accounts).AsNoTracking()
                .Where(x => x.Identity.IdenId == id).AsNoTracking()
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
            identity.Mobiles = await _context.Mobiles.AsNoTracking()
                .Include(x => x.Subscriptions).AsNoTracking()
                .Include(x => x.MobileType)
                .ThenInclude(x => x.Category)
                .Include(x => x.Identity).AsNoTracking()
                .Where(x => x.Identity.IdenId == identity.IdenId)
                .Select(x => MobileRepository.ConvertMobile(x))
                .ToListAsync();
            /*identity.Subscriptions = _context.Subscriptions
                .Include(x => x.SubscriptionType)
                .Include(x => x.Category)
                .Where(x => x.IdentityId == identity.IdenId)
                .ToList();*/
        }
    }
}
