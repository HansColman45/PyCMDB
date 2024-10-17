using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CMDB.API.Services
{
    public class IdentityTypeRepository : GenericRepository, IIdentityTypeRepository
    {
        private readonly string table = "identitytype";
        public IdentityTypeRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<TypeDTO> Activate(TypeDTO type)
        {
            var acctype = await GetTypeById(type.TypeId);
            string logline = GenericLogLineCreator.ActivateLogLine($"identity type with {type.Type} and {type.Description}", TokenStore.Admin.Account.UserID, table);
            try
            {
                acctype.active = 1;
                acctype.DeactivateReason = "";
                acctype.LastModifiedAdminId = TokenStore.Admin.AdminId;
                acctype.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = logline,
                }
                );
                _context.Types.Update(acctype);
            }
            catch (Exception e)
            {
                _logger.LogError("Db error {e}", e);
                throw;
            }
            return type;
        }
        public TypeDTO Create(TypeDTO typeDTO)
        {
            string logline = GenericLogLineCreator.CreateLogLine($"accounttype with {typeDTO.Type} and {typeDTO.Description}", TokenStore.Admin.Account.UserID, table);
            try
            {
                IdentityType type = ConvertDTO(typeDTO);
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
                acctype.active = 0;
                acctype.DeactivateReason = reason;
                acctype.LastModifiedAdminId = TokenStore.Admin.AdminId;
                acctype.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = logline,
                }
                );
                _context.Types.Update(acctype);
            }
            catch (Exception e)
            {
                _logger.LogError("Db error {e}", e);
                throw;
            }
            return type;
        }
        public async Task<List<TypeDTO>> GetAll()
        {
            List<TypeDTO> accountTypes = await _context.Types.OfType<IdentityType>().AsNoTracking()
               .Select(x => ConvertType(x))
               .ToListAsync();
            return accountTypes;
        }
        public async Task<List<TypeDTO>> GetAll(string searchStr)
        {
            string searhterm = "%" + searchStr + "%";
            List<TypeDTO> accountTypes = await _context.Types.OfType<IdentityType>()
                .Where(x => EF.Functions.Like(x.Type, searhterm) || EF.Functions.Like(x.Description, searhterm))
                .AsNoTracking()
                .Select(x => ConvertType(x))
                .ToListAsync();
            return accountTypes;
        }
        public async Task<TypeDTO?> GetById(int id)
        {
            var type = await _context.Types.OfType<IdentityType>().AsNoTracking()
                .Where(x => x.TypeId == id).AsNoTracking()
                .Select(x => ConvertType(x))
                .FirstAsync();
            if (type is not null)
            {
                GetLogs(table, id, type);
            }
            return type;
        }
        public async Task<bool> IsExisitng(TypeDTO type)
        {
            bool result = false;
            var oldtype = await GetTypeById(type.TypeId);
            if (oldtype is null)
            {
                var types = await _context.Types.OfType<IdentityType>()
                    .Where(x => x.Type == type.Type || x.Description == type.Description).AsNoTracking()
                    .ToListAsync();
                if (types.Count > 0)
                    result = true;
            }
            else
            {
                if (string.Compare(oldtype.Type, type.Type) != 0)
                {
                    var types = await _context.Types.OfType<IdentityType>()
                    .Where(x => x.Type == type.Type).AsNoTracking()
                    .ToListAsync();
                    if (types.Count > 0)
                        result = true;
                }
                else if (string.Compare(oldtype.Description, type.Description) != 0)
                {
                    var types = await _context.Types.OfType<IdentityType>()
                    .Where(x => x.Description == type.Description).AsNoTracking()
                    .ToListAsync();
                    if (types.Count > 0)
                        result = true;
                }
            }
            return result;
        }
        public async Task<TypeDTO> Update(TypeDTO type)
        {
            var oldType = await GetTypeById(type.TypeId);
            var newType = ConvertDTO(type);
            if (string.Compare(oldType.Type, newType.Type) != 0)
            {
                string logline = GenericLogLineCreator.UpdateLogLine("type", oldType.Type, newType.Type, TokenStore.Admin.Account.UserID, table);
                try
                {
                    oldType.Type = newType.Type;
                    oldType.LastModifiedAdminId = TokenStore.Admin.AdminId;
                    oldType.Logs.Add(new()
                    {
                        LogDate = DateTime.UtcNow,
                        LogText = logline,
                    }
                    );
                    _context.Types.Update(oldType);
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
                    oldType.Description = newType.Description;
                    oldType.LastModifiedAdminId = TokenStore.Admin.AdminId;
                    oldType.Logs.Add(new()
                    {
                        LogDate = DateTime.UtcNow,
                        LogText = logline,
                    }
                    );
                    _context.Types.Update(oldType);
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
        public static TypeDTO ConvertType(IdentityType type)
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
        public static IdentityType ConvertDTO(TypeDTO typeDTO)
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
        private async Task<IdentityType?> GetTypeById(int id)
        {
            return await _context.Types.OfType<IdentityType>()
                .Where(x => x.TypeId == id)
                .FirstAsync();
        }
    }
}
