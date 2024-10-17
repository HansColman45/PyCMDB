using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public class IdentityRepository : GenericRepository, IIdentityRepository
    {
        private readonly string table = "identity";
        public IdentityRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
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
                GetAssignedAccounts(id);
            }
            return iden;
        }
        public async Task LogPdfFile(string pdfFile, int id)
        {
            Identity iden = await _context.Identities.Where(x => x.IdenId == id).FirstAsync();
            iden.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
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
            string logLine = GenericLogLineCreator.CreateLogLine($"Identity width name: {identityDTO.Name}", TokenStore.Admin.Account.UserID, table);
            iden.Logs.Add(new()
            {
                LogDate = DateTime.UtcNow,
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
            if (string.Compare(oldIden.Name, dTO.Name) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("Name", oldIden.Name, dTO.Name, TokenStore.Admin.Account.UserID, table);
                try
                {
                    oldIden.Name = dTO.Name;
                    oldIden.Logs.Add(new()
                    {
                        LogDate = DateTime.UtcNow,
                        LogText = logline,
                    });
                    /*string sql = $"Update {table} set LastModifiedAdminId = {TokenStore.Admin.AdminId}, Name='{dTO.Name}' where IdenId={dTO.IdenId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,IdentityId) values (GETDATE(),'{logline}',{dTO.IdenId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);*/
                }
                catch (Exception e)
                {
                    _logger.LogError("Db error {e}", e);
                    throw;
                }
            }
            if (string.Compare(oldIden.EMail, dTO.EMail) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("EMail", oldIden.EMail, dTO.EMail, TokenStore.Admin.Account.UserID, table);
                try
                {
                    oldIden.EMail = dTO.EMail;
                    oldIden.Logs.Add(new()
                    {
                        LogDate = DateTime.UtcNow,
                        LogText = logline,
                    });
                    /*string sql = $"Update {table} set LastModifiedAdminId = {TokenStore.Admin.AdminId}, EMail='{dTO.EMail}' where IdenId={dTO.IdenId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,IdentityId) values (GETDATE(),'{logline}',{dTO.IdenId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);*/
                }
                catch (Exception e)
                {
                    _logger.LogError("Db error {e}", e);
                    throw;
                }
            }
            if (string.Compare(oldIden.Company, dTO.Company) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("Company", oldIden.Company, dTO.Company, TokenStore.Admin.Account.UserID, table);
                try
                {
                    oldIden.Company = dTO.Company;
                    oldIden.Logs.Add(new()
                    {
                        LogDate = DateTime.UtcNow,
                        LogText = logline,
                    });
                    /*string sql = $"Update {table} set LastModifiedAdminId = {TokenStore.Admin.AdminId}, Company='{dTO.Company}' where IdenId={dTO.IdenId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,IdentityId) values (GETDATE(),'{logline}',{dTO.IdenId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);*/
                }
                catch (Exception e)
                {
                    _logger.LogError("Db error {e}", e);
                    throw;
                }
            }
            if (oldIden.TypeId != dTO.Type.TypeId)
            {
                var oldType = _context.Types.OfType<IdentityType>()
                    .AsNoTracking()
                    .First(x => x.TypeId == oldIden.TypeId);
                logline = GenericLogLineCreator.UpdateLogLine("Type", oldType.Type, dTO.Type.Type, TokenStore.Admin.Account.UserID, table);
                try
                {
                    oldIden.TypeId = dTO.Type.TypeId;
                    oldIden.Logs.Add(new()
                    {
                        LogDate = DateTime.UtcNow,
                        LogText = logline,
                    });
                    /*string sql = $"Update {table} set LastModifiedAdminId = {TokenStore.Admin.AdminId}, TypeId={dTO.Type.TypeId} where IdenId={dTO.IdenId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,IdentityId) values (GETDATE(),'{logline}',{dTO.IdenId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);*/
                }
                catch (Exception e)
                {
                    _logger.LogError("Db error {e}", e);
                    throw;
                }
            }
            if (string.Compare(oldIden.LanguageCode, dTO.Language.Code) != 0)
            {
                var oldLang = _context.Languages.AsNoTracking().First(x => x.Code == oldIden.LanguageCode);
                logline = GenericLogLineCreator.UpdateLogLine("Language", oldLang.Description, dTO.Language.Description, TokenStore.Admin.Account.UserID, table);
                try
                {
                    oldIden.LanguageCode = dTO.Language.Code;
                    oldIden.Logs.Add(new()
                    {
                        LogDate = DateTime.UtcNow,
                        LogText = logline,
                    });
                    /*string sql = $"Update {table} set LastModifiedAdminId = {TokenStore.Admin.AdminId}, LanguageCode='{dTO.Language.Code}' where IdenId={dTO.IdenId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,IdentityId) values (GETDATE(),'{logline}',{dTO.IdenId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);*/
                }
                catch (Exception e)
                {
                    _logger.LogError("Db error {e}", e);
                    throw;
                }
            }
            if (string.Compare(oldIden.UserID, dTO.UserID) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("UserID", oldIden.UserID, dTO.UserID, TokenStore.Admin.Account.UserID, table);
                try
                {
                    oldIden.UserID = dTO.UserID;
                    oldIden.Logs.Add(new()
                    {
                        LogDate = DateTime.UtcNow,
                        LogText = logline,
                    });
                    /*string sql = $"Update {table} set LastModifiedAdminId = {TokenStore.Admin.AdminId}, UserId='{dTO.UserID}' where IdenId={dTO.IdenId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,IdentityId) values (GETDATE(),'{logline}',{dTO.IdenId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);*/
                }
                catch (Exception e)
                {
                    _logger.LogError("Db error {e}", e);
                    throw;
                }
            }
            _context.Identities.Update(oldIden);
            return dTO;
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
                LogText = GenericLogLineCreator.DeleteLogLine($"Identity width name: {identity.Name}", TokenStore.Admin.Account.UserID, reason, table),
                LogDate = DateTime.UtcNow,
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
                LogText = GenericLogLineCreator.ActivateLogLine($"Identity width name: {identity.Name}", TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.UtcNow,
            });
            _context.Identities.Update(iden);
            return identity;
        }
        private async Task<Identity> TrackedIden(int id)
        {
            return await _context.Identities.FirstAsync(x => x.IdenId == id);
        }
        private async Task<Identity?> GetIdenByID(int id)
        {
            return await _context.Identities.AsNoTracking()
                .Include(x => x.Accounts).AsNoTracking()
                .Include(x => x.Type).AsNoTracking()
                .Include(x => x.Language).AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdenId == id);
        }
        private void GetAssignedAccounts(int id)
        {
            var accounts = _context.Identities.AsNoTracking()
                .Include(x => x.Language).AsNoTracking()
                .Include(x => x.Accounts)
                .ThenInclude(d => d.Account).AsNoTracking()
                .SelectMany(x => x.Accounts).AsNoTracking()
                .Where(x => x.Identity.IdenId == id).AsNoTracking()
                .ToList();
        }
    }
}
