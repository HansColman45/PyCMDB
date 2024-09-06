using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;


namespace CMDB.API.Services
{
    public class AccountTypeService : CMDBService, IAccountTypeService
    {
        private readonly ILogger<AccountTypeService> _logger;
        private ILogService _logService;
        public AccountTypeService(CMDBContext context, ILogService logService, ILogger<AccountTypeService> logger) : base(context)
        {
            _logger = logger;
            _logService = logService;
        }

        public Task<TypeDTO> Activate(TypeDTO typeDTO)
        {
            throw new NotImplementedException();
        }

        public Task<TypeDTO> Create(TypeDTO typeDTO)
        {
            throw new NotImplementedException();
        }

        public Task<TypeDTO> Delete(TypeDTO typeDTO, string reason)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TypeDTO>> GetAll()
        {
            List<TypeDTO> accountTypes = await _context.Types.OfType<AccountType>()
                .Select(x => new TypeDTO() 
                    {
                        Type = x.Type,
                        Active = x.active,
                        Description  = x.Description,
                        DeactivateReason = x.DeactivateReason,
                        LastModifiedAdminId = x.LastModifiedAdminId,
                        TypeId = x.TypeId
                    }
                )
                .ToListAsync();
            return accountTypes;
        }

        public async Task<List<TypeDTO>> GetAll(string searchStr)
        {
            string searhterm = "%" + searchStr + "%";
            List<TypeDTO> accountTypes = await _context.Types
                .OfType<AccountType>()
                .Where(x => EF.Functions.Like(x.Type, searhterm) || EF.Functions.Like(x.Description, searhterm))
                .Select(x => new TypeDTO()
                    {
                        Type = x.Type,
                        Active = x.active,
                        Description = x.Description,
                        DeactivateReason = x.DeactivateReason,
                        LastModifiedAdminId = x.LastModifiedAdminId,
                        TypeId = x.TypeId
                    }
                )
                .ToListAsync();
            return accountTypes;
        }

        public async Task<TypeDTO> GetById(int id)
        {
            AccountType type = await _context.Types.OfType<AccountType>().Where(x => x.TypeId == id).FirstAsync();
            return ConvertDTO(type);
        }

        public Task<TypeDTO> Update(TypeDTO typeDTO)
        {
            throw new NotImplementedException();
        }

        private TypeDTO ConvertDTO(AccountType type)
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
    }
}
