using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace CMDB.API.Services
{
    public class AccountTypeService : LogService, IAccountTypeService
    {
        public AccountTypeService(CMDBContext context) : base(context)
        {
        }

        public async Task<List<AccountTypeDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountTypeDTO>> GetAll(string searchStr)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountTypeDTO> GetById(int id)
        {
            AccountType type = await _context.Types.OfType<AccountType>().Where(x => x.TypeId == id).FirstAsync();
            return ConvertDTO(type);
        }
        private AccountTypeDTO ConvertDTO(AccountType type)
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
