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
        AccountType Create(TypeDTO typeDTO);
        Task<AccountType> DeActivate(TypeDTO type, string reason);
        Task<AccountType> Activate(TypeDTO type);
        Task<AccountType> Update(TypeDTO type);
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
            return await _context.Types.OfType<AccountType>().AsNoTracking()
                .Where(x => x.TypeId == id).AsNoTracking()
                .Select(x => ConvertType(x))
                .FirstAsync();
        }
        public AccountType Create(TypeDTO typeDTO)
        {
            var type = ConvertDTO(typeDTO);
            type.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.CreateLogLine($"accounttype with {type.Type} and {type.Description}",TokenStore.Admin.Account.UserID,table),
                LogDate = DateTime.UtcNow,
            });
            _context.Types.Add(type);
            return type;
        }
        public async Task<AccountType> DeActivate(TypeDTO type, string reason)
        {
            var acctype = await GetTypeById(type.TypeId);
            acctype.DeactivateReason = reason;
            acctype.Active = State.Inactive;
            acctype.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.DeleteLogLine($"accounttype with {type.Type} and {type.Description}", 
                    TokenStore.Admin.Account.UserID, reason, table),
                LogDate = DateTime.UtcNow,
            });
            _context.Types.Update(acctype);
            return acctype;
        }
        public async Task<AccountType> Activate(TypeDTO type)
        {
            var acctype = await GetTypeById(type.TypeId);
            acctype.DeactivateReason = null;
            acctype.Active = State.Active;
            acctype.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.ActivateLogLine($"accounttype with {type.Type} and {type.Description}",
                    TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.UtcNow
            });
            _context.Types.Update(acctype);
            return acctype;
        }
        public async Task<AccountType> Update(TypeDTO type)
        {
            var oldType = await GetTypeById(type.TypeId);
            var newType = ConvertDTO(type);
            if (string.Compare(oldType.Type, newType.Type)!= 0)
            {
                oldType.Type = newType.Type;
                newType.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = GenericLogLineCreator.UpdateLogLine("type",oldType.Type,newType.Type,TokenStore.Admin.Account.UserID,table)
                });
            }
            if (string.Compare(newType.Description, newType.Description) != 0)
            {
                oldType.Description = newType.Description;
                newType.Logs.Add(new()
                {
                    LogDate = DateTime.UtcNow,
                    LogText = GenericLogLineCreator.UpdateLogLine("description", oldType.Description, newType.Description, TokenStore.Admin.Account.UserID, table)
                });
            }
            _context.Types.Update(oldType);
            return oldType;
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
