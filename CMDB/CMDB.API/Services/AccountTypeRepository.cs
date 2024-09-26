using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public interface IAccountTypeRepository
    {
        Task<List<TypeDTO>> GetAll();
        Task<List<TypeDTO>> GetAll(string searchStr);
        Task<TypeDTO?> GetById(int id);
        TypeDTO Create(TypeDTO typeDTO);
        Task<TypeDTO> DeActivate(TypeDTO type, string reason);
        Task<TypeDTO> Activate(TypeDTO type);
        Task<TypeDTO> Update(TypeDTO type);
        Task<bool> IsExisitng(TypeDTO type);
    }
    public class AccountTypeRepository : GenericRepository, IAccountTypeRepository
    {
        private readonly string table = "accounttype";

        public AccountTypeRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        public async Task<List<TypeDTO>> GetAll()
        {
            List<TypeDTO> accountTypes = await _context.Types.OfType<AccountType>().AsNoTracking()
                .Select(x => ConvertType(x))
                .ToListAsync();
            return accountTypes;
        }
        public async Task<List<TypeDTO>> GetAll(string searchStr)
        {
            string searhterm = "%" + searchStr + "%";
            List<TypeDTO> accountTypes = await _context.Types.OfType<AccountType>()
                .Where(x => EF.Functions.Like(x.Type, searhterm) || EF.Functions.Like(x.Description, searhterm))
                .AsNoTracking()
                .Select(x => ConvertType(x))
                .ToListAsync();
            return accountTypes;
        }
        public async Task<TypeDTO?> GetById(int id)
        {
            var type =  await _context.Types.OfType<AccountType>().AsNoTracking()
                .Where(x => x.TypeId == id).AsNoTracking()
                .Select(x => ConvertType(x))
                .FirstAsync();
            if(type is not null)
            {
                GetLogs(table,id,type);
            }
            return type;
        }
        public TypeDTO Create(TypeDTO typeDTO)
        {
            string logline = GenericLogLineCreator.CreateLogLine($"accounttype with {typeDTO.Type} and {typeDTO.Description}", TokenStore.Admin.Account.UserID, table);
            try
            {
                AccountType type = ConvertDTO(typeDTO);
                type.LastModifiedAdminId = TokenStore.Admin.AdminId;
                type.active = 1;
                type.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = logline,
                });
                _context.Types.Add(type);
            }
            catch (Exception e)
            {
                _logger.LogError("Db error {e}", e);
                throw;
            }
            return typeDTO;
        }
        public async Task<TypeDTO> DeActivate(TypeDTO type, string reason)
        {
            var acctype = await GetTypeById(type.TypeId);
            string logline = GenericLogLineCreator.DeleteLogLine($"accounttype with {type.Type} and {type.Description}", TokenStore.Admin.Account.UserID, reason, table);
            try
            {
                string sql = $"update type set LastModifiedAdminId = {TokenStore.Admin.AdminId}, active = 0, Deactivate_reason = '{reason}' where TypeId = {type.TypeId}";
                await _context.Database.ExecuteSqlRawAsync(sql);
                sql = $"insert into log(LogDate,LogText,TypeId) values (GETDATE(),'{logline}',{type.TypeId})";
                await _context.Database.ExecuteSqlRawAsync(sql);
            }
            catch (Exception e)
            {
                _logger.LogError("Db error {e}", e);
                throw;
            }
            return type;
        }
        public async Task<TypeDTO> Activate(TypeDTO type)
        {
            var acctype = await GetTypeById(type.TypeId);
            string logline = GenericLogLineCreator.ActivateLogLine($"accounttype with {type.Type} and {type.Description}", TokenStore.Admin.Account.UserID, table);
            try
            {
                string sql = $"update type set LastModifiedAdminId = {TokenStore.Admin.AdminId}, active = 1, Deactivate_reason = '' where TypeId = {type.TypeId}";
                await _context.Database.ExecuteSqlRawAsync(sql);
                sql = $"insert into log(LogDate,LogText,TypeId) values (GETDATE(),'{logline}',{type.TypeId})";
                await _context.Database.ExecuteSqlRawAsync(sql);
            }
            catch (Exception e)
            {
                _logger.LogError("Db error {e}", e);
                throw;
            }
            return type;
        }
        public async Task<TypeDTO> Update(TypeDTO type)
        {
            var oldType = await GetTypeById(type.TypeId);
            var newType = ConvertDTO(type);
            if (string.Compare(oldType.Type, newType.Type)!= 0)
            {
                string logline = GenericLogLineCreator.UpdateLogLine("type", oldType.Type, newType.Type, TokenStore.Admin.Account.UserID, table);
                try
                {
                    string sql = $"update type set LastModifiedAdminId = {TokenStore.Admin.AdminId}, Type = '{newType.Type}' where TypeId = {type.TypeId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,TypeId) values (GETDATE(),'{logline}',{type.TypeId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                }
                catch (Exception e)
                {
                    _logger.LogError("Db error {e}", e);
                    throw;
                }
            }
            if (string.Compare(oldType.Description, newType.Description) != 0)
            {
                string logline = GenericLogLineCreator.UpdateLogLine("description", oldType.Description, newType.Description, TokenStore.Admin.Account.UserID, table);
                try
                {
                    string sql = $"update type set LastModifiedAdminId = {TokenStore.Admin.AdminId}, description = '{newType.Description}' where TypeId = {type.TypeId}";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    sql = $"insert into log(LogDate,LogText,TypeId) values (GETDATE(),'{logline}',{type.TypeId})";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                }
                catch (Exception e)
                {
                    _logger.LogError("Db error {e}", e);
                    throw;
                }
            }
            _context.Types.Update(oldType);
            return type;
        }
        public async Task<bool> IsExisitng(TypeDTO type)
        {
            bool result = false;
            var oldtype = await GetTypeById(type.TypeId);
            if (oldtype is null)
            {
                var types = await _context.Types.OfType<AccountType>()
                    .Where(x => x.Type == type.Type || x.Description == type.Description).AsNoTracking()
                    .ToListAsync();
                if (types.Count > 0)
                    result = true;
            }
            else
            {
                if(string.Compare(oldtype.Type,type.Type) != 0)
                {
                    var types = await _context.Types.OfType<AccountType>()
                    .Where(x => x.Type == type.Type).AsNoTracking()
                    .ToListAsync();
                    if (types.Count > 0)
                        result = true;
                }
                else if(string.Compare(oldtype.Description,type.Description) != 0)
                {
                    var types = await _context.Types.OfType<AccountType>()
                    .Where(x => x.Description == type.Description).AsNoTracking()
                    .ToListAsync();
                    if (types.Count > 0)
                        result = true;
                }
            }
            return result;
        }
        public static TypeDTO ConvertType(in AccountType type)
        {
            return new()
            {
                Active = type.active,
                DeactivateReason = type.DeactivateReason,
                Description = type.Description,
                LastModifiedAdminId = type.LastModifiedAdminId,
                Type = type.Type,
                TypeId = type.TypeId
            };
        }
        public static AccountType ConvertDTO(TypeDTO typeDTO) 
        {
            return new()
            {
                active = typeDTO.Active,
                DeactivateReason = typeDTO.DeactivateReason,
                Description = typeDTO.Description,
                LastModifiedAdminId = typeDTO.LastModifiedAdminId,
                Type = typeDTO.Type,
                TypeId = typeDTO.TypeId
            };
        }
        private async Task<AccountType?> GetTypeById(int id)
        {
            return await _context.Types.OfType<AccountType>()
                .Where(x => x.TypeId == id)
                .FirstAsync();
        }
    }
}
