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
        public List<AccountType> GetAccountTypeByID(int ID)
        {
            List<AccountType> accountTypes = _context.AccountTypes.Where(x => x.TypeID == ID).ToList();
            return accountTypes;
        }
        public async Task Create(AccountType accountType, string Table)
        {
            accountType.LastModfiedAdmin = Admin;
            _context.AccountTypes.Add(accountType);
            await _context.SaveChangesAsync();
            string Value = "Account type created with type: " + accountType.Type + " and description: " + accountType.Description;
            LogCreate(Table, accountType.TypeID, Value);
        }
        public async Task Update(AccountType accountType, string Type, string Description, string Table)
        {
            accountType.LastModfiedAdmin = Admin;
            if (String.Compare(accountType.Type, Type) != 0)
            {
                accountType.Type = Type;
                await _context.SaveChangesAsync();
                LogUpdate(Table, accountType.TypeID, "Type", accountType.Type, Type);
            }
            if (String.Compare(accountType.Description, Description) != 0)
            {
                accountType.Description = Description;
                await _context.SaveChangesAsync();
                LogUpdate(Table, accountType.TypeID, "Description", accountType.Description, Description);
            }
        }
        public async Task Deactivate(AccountType accountType, string Reason, string Table)
        {
            accountType.Active = "Inactive";
            accountType.DeactivateReason = Reason;
            accountType.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
            string Value = "Account type created with type: " + accountType.Type + " and description: " + accountType.Description;
            LogDeactivate(Table, accountType.TypeID, Value, Reason);
        }
        public async Task Activate(AccountType accountType, string Table)
        {
            accountType.Active = "Active";
            accountType.DeactivateReason = "";
            accountType.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
            string Value = "Account type created with type: " + accountType.Type + " and description: " + accountType.Description;
            LogActivate(Table, accountType.TypeID, Value);
        }
        public ICollection<AccountType> ListAll()
        {
            List<AccountType> accountTypes = _context.AccountTypes.ToList();
            return accountTypes;
        }
        public ICollection<AccountType> ListAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            List<AccountType> accountTypes = _context.AccountTypes
                .Where(x => EF.Functions.Like(x.Type, searhterm) || EF.Functions.Like(x.Description, searhterm)).ToList();
            return accountTypes;
        }
        public bool IsExisting(AccountType accountType, string Type = "", string Description = "")
        {
            bool result = false;
            if (String.IsNullOrEmpty(Type) && String.Compare(accountType.Type, Type) != 0)
            {
                var accountTypes = _context.AccountTypes.Where(x => x.Type == Type).ToList();
                if (accountTypes.Count > 0)
                    result = true;
            }
            else
            {
                var accountTypes = _context.AccountTypes.Where(x => x.Type == accountType.Type).ToList();
                if (accountTypes.Count > 0)
                    result = true;
            }
            if (String.IsNullOrEmpty(Description) && String.Compare(accountType.Description, Description) != 0)
            {
                var accountTypes = _context.AccountTypes.Where(x => x.Description == Description).ToList();
                if (accountTypes.Count > 0)
                    result = true;
            }
            else
            {
                var accountTypes = _context.AccountTypes.Where(x => x.Description == accountType.Description).ToList();
                if (accountTypes.Count > 0)
                    result = true;
            }
            return result;
        }
    }
}
