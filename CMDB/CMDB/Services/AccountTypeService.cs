using CMDB.Infrastructure;
using System.Collections.Generic;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class AccountTypeService : LogService
    {
        public AccountTypeService(CMDBContext context) : base(context)
        {
        }
        public async Task<List<AccountType>> GetAccountTypeByID(int ID)
        {
            List<AccountType> accountTypes = await _context.Types.OfType<AccountType>().Where(x => x.TypeId == ID).ToListAsync();
            return accountTypes;
        }
        public async Task Create(AccountType accountType, string Table)
        {
            accountType.LastModfiedAdmin = Admin;
            _context.Types.Add(accountType);
            await _context.SaveChangesAsync();
            string Value = "Accounttype with type: " + accountType.Type + " and description: " + accountType.Description;
            await LogCreate(Table, accountType.TypeId, Value);
        }
        public async Task Update(AccountType accountType, string Type, string Description, string Table)
        {
            accountType.LastModfiedAdmin = Admin;
            var oldtype = accountType.Type;
            var olddescription = accountType.Description;
            if (String.Compare(accountType.Type, Type) != 0)
            {
                accountType.Type = Type;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, accountType.TypeId, "Type", oldtype, Type);
            }
            if (String.Compare(accountType.Description, Description) != 0)
            {
                accountType.Description = Description;
                await _context.SaveChangesAsync();
                await LogUpdate(Table, accountType.TypeId, "Description", olddescription, Description);
            }
        }
        public async Task Deactivate(AccountType accountType, string Reason, string Table)
        {
            accountType.Active = State.Inactive;
            accountType.DeactivateReason = Reason;
            accountType.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
            string Value = "Accounttype with type: " + accountType.Type + " and description: " + accountType.Description;
            await LogDeactivate(Table, accountType.TypeId, Value, Reason);
        }
        public async Task Activate(AccountType accountType, string Table)
        {
            accountType.Active = State.Active;
            accountType.DeactivateReason = "";
            accountType.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
            string Value = "Accounttype with type: " + accountType.Type + " and description: " + accountType.Description;
            await LogActivate(Table, accountType.TypeId, Value);
        }
        public async Task<List<AccountType>> ListAll()
        {
            List<AccountType> accountTypes = await _context.Types.OfType<AccountType>().ToListAsync();
            return accountTypes;
        }
        public async Task<List<AccountType>> ListAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            List<AccountType> accountTypes = await _context.Types
                .OfType<AccountType>()
                .Where(x => EF.Functions.Like(x.Type, searhterm) || EF.Functions.Like(x.Description, searhterm)).ToListAsync();
            return accountTypes;
        }
        public bool IsExisting(AccountType accountType, string Type = "", string Description = "")
        {
            bool result = false;
            if (String.IsNullOrEmpty(Type) && String.Compare(accountType.Type, Type) != 0 && 
                String.IsNullOrEmpty(Description) && String.Compare(accountType.Description, Description) != 0)
            {
                var accountTypes = _context.Types
                    .OfType<AccountType>()
                    .Where(x => x.Type == accountType.Type && x.Description == accountType.Description)
                    .ToList();
                if (accountTypes.Count > 0)
                    result = true;
            }
            else
            {
                var accountTypes = _context.Types
                    .OfType<AccountType>()
                    .Where(x => x.Type == Type && x.Description == Description)
                    .ToList();
                if (accountTypes.Count > 0)
                    result = true;
            }
            return result;
        }
    }
}
