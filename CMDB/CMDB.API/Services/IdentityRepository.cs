using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public interface IIdentityRepository
    {
        Task<IdentityDTO?> GetById(int id);
        Task<IEnumerable<IdentityDTO>> GetAll();
        Task<IEnumerable<IdentityDTO>> GetAll(string searchStr);
        Task<IEnumerable<IdentityDTO>> ListAllFreeIdentities();
    }
    public class IdentityRepository : GenericRepository, IIdentityRepository
    {
        private readonly string table = "identity";
        public IdentityRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        public async Task<IdentityDTO?> GetById(int id)
        {
            var iden = await _context.Identities
                .Include(x => x.Type)
                .Include(x => x.Language)
                .Where(x => x.IdenId == id)
                .Select(x => ConvertIdentity(x))
                .FirstOrDefaultAsync();
            return iden;
        }
        public async Task<IEnumerable<IdentityDTO>> GetAll()
        {
            return await _context.Identities
                .Include(x => x.Type)
                .Include(x => x.Language)
                .Select(x => ConvertIdentity(x)).ToListAsync();
        }
        public async Task<IEnumerable<IdentityDTO>> GetAll(string searchStr)
        {
            string searhterm = "%" + searchStr + "%";
            return await _context.Identities
                .Include(x => x.Type)   
                .Where(x => EF.Functions.Like(x.Name, searhterm) || EF.Functions.Like(x.UserID, searhterm)
                    || EF.Functions.Like(x.EMail, searhterm) || EF.Functions.Like(x.Type.Type, searhterm))
                .Select(x => ConvertIdentity(x))
                .ToListAsync();
        }
        public async Task<IEnumerable<IdentityDTO>> ListAllFreeIdentities()
        {
            var identities = await _context.Identities
                .Include(x => x.Accounts)
                .Include(x => x.Type)
                .Include(x => x.Language)
                .Where(x => x.active == 1 && x.IdenId != 1)
                .Where(x => !x.Accounts.Any(y => y.ValidFrom <= DateTime.Now && y.ValidUntil >= DateTime.Now))
                .Select(x => ConvertIdentity(x))
                .ToListAsync();
            return identities;
        }
        public async Task<Identity> Update(IdentityDTO dTO)
        {
            var oldIden = await GetIdenByID(dTO.IdenId);
            oldIden.LastModifiedAdminId = TokenStore.Admin.AdminId;
            string logline;
            if(string.Compare(oldIden.Name,dTO.Name) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("Name", oldIden.Name, dTO.Name, TokenStore.Admin.Account.UserID, table);
                try
                {
                    string sql = $"Update {table} set LastModifiedAdminId = {TokenStore.Admin.AdminId}, Name='{dTO.Name}' where IdenId={dTO.IdenId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,IdentityId) values (GETDATE(),'{logline}',{dTO.IdenId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);
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
                    string sql = $"Update {table} set LastModifiedAdminId = {TokenStore.Admin.AdminId}, EMail='{dTO.EMail}' where IdenId={dTO.IdenId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,IdentityId) values (GETDATE(),'{logline}',{dTO.IdenId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);
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
                    string sql = $"Update {table} set LastModifiedAdminId = {TokenStore.Admin.AdminId}, Company='{dTO.Company}' where IdenId={dTO.IdenId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,IdentityId) values (GETDATE(),'{logline}',{dTO.IdenId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                }
                catch (Exception e)
                {
                    _logger.LogError("Db error {e}", e);
                    throw;
                }
            }
            if (oldIden.Type.TypeId != dTO.Type.TypeId)
            {
                logline = GenericLogLineCreator.UpdateLogLine("Type", oldIden.Type.Type, dTO.Type.Type, TokenStore.Admin.Account.UserID, table);
                try
                {
                    string sql = $"Update {table} set LastModifiedAdminId = {TokenStore.Admin.AdminId}, TypeId={dTO.Type.TypeId} where IdenId={dTO.IdenId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,IdentityId) values (GETDATE(),'{logline}',{dTO.IdenId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                }
                catch (Exception e)
                {
                    _logger.LogError("Db error {e}", e);
                    throw;
                }
            }
            if(string.Compare(oldIden.Language.Code, dTO.Language.Code) != 0)
            {
                logline = GenericLogLineCreator.UpdateLogLine("Language", oldIden.Language.Description, dTO.Language.Description, TokenStore.Admin.Account.UserID, table);
                try
                {
                    string sql = $"Update {table} set LastModifiedAdminId = {TokenStore.Admin.AdminId}, LanguageCode='{dTO.Language.Code}' where IdenId={dTO.IdenId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,IdentityId) values (GETDATE(),'{logline}',{dTO.IdenId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);
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
                    string sql = $"Update {table} set LastModifiedAdminId = {TokenStore.Admin.AdminId}, UserId='{dTO.UserID}' where IdenId={dTO.IdenId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,IdentityId) values (GETDATE(),'{logline}',{dTO.IdenId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                }
                catch (Exception e)
                {
                    _logger.LogError("Db error {e}", e);
                    throw;
                }
            }
            return oldIden;
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
        private async Task<Identity?> GetIdenByID(int id)
        {
            return await _context.Identities
                .Include(x => x.Accounts)
                .Include(x => x.Type)
                .Include(x => x.Language)
                .FirstOrDefaultAsync(x => x.IdenId == id);
        }
    }
}
