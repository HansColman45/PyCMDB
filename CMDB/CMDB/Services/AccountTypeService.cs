using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class AccountTypeService : LogService
    {
        public AccountTypeService() : base()
        {
        }
        public async Task<TypeDTO> GetAccountTypeByID(int ID)
        {
            BaseUrl = _url + $"api/AccountType/{ID}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<TypeDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl,response.StatusCode);
        }
        public async Task Create(TypeDTO typeDTO)
        {
            BaseUrl = _url + $"api/AccountType";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, typeDTO);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsJsonAsync<TypeDTO>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task Update(TypeDTO accountType, string Type, string Description, string Table)
        {
            /*accountType.LastModfiedAdmin = Admin;
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
            }*/
        }
        public async Task Deactivate(TypeDTO accountType, string Reason)
        {
           /* accountType.Active = State.Inactive;
            accountType.DeactivateReason = Reason;
            accountType.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
            string Value = "Accounttype with type: " + accountType.Type + " and description: " + accountType.Description;
            await LogDeactivate(Table, accountType.TypeId, Value, Reason);*/
        }
        public async Task Activate(TypeDTO accountType, string Table)
        {
           /* accountType.Active = State.Active;
            accountType.DeactivateReason = "";
            accountType.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
            string Value = "Accounttype with type: " + accountType.Type + " and description: " + accountType.Description;
            await LogActivate(Table, accountType.TypeId, Value);*/
        }
        public async Task<List<AccountType>> ListAll()
        {
            /*List<AccountType> accountTypes = await _context.Types.OfType<AccountType>().ToListAsync();
            return accountTypes;*/
            return [];
        }
        public async Task<List<AccountType>> ListAll(string searchString)
        {
            /* string searhterm = "%" + searchString + "%";
             List<AccountType> accountTypes = await _context.Types
                 .OfType<AccountType>()
                 .Where(x => EF.Functions.Like(x.Type, searhterm) || EF.Functions.Like(x.Description, searhterm)).ToListAsync();
             return accountTypes;*/
            return [];
        }
        public bool IsExisting(TypeDTO accountType, string Type = "", string Description = "")
        {
            bool result = false;
            /*if (String.IsNullOrEmpty(Type) && String.Compare(accountType.Type, Type) != 0 && 
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
            }*/
            return result;
        }
    }
}
