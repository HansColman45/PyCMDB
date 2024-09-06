using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class AccountService : LogService
    {
        public AccountService(CMDBContext context) : base(context)
        {
        }
        public async Task<List<Account>> ListAll()
        {
            BaseUrl = _url + $"api/Account/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<Account>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<Account>> GetByID(int ID)
        {
            BaseUrl = _url + $"api/Account/{ID}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode) { 
                var account = await response.Content.ReadAsJsonAsync<Account>();
                return new List<Account> { account };
            }
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<Account>> ListAll(string searchString)
        {
            BaseUrl = _url + $"api/Account/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<Account>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task CreateNew(string UserID, int type, int application)
        {
            AccountDTO dto = new()
            {
                Active = 1,
                UserID = UserID,
                ApplicationId = application,
                TypeId = type,
                Application = await GetApplicationByID(application),
                Type = await GetAccountTypeByID(type),
            };
            BaseUrl = _url + "api/Account";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, dto);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");
        }
        public async Task Edit(Account account, string UserID, int type, int application)
        {
            AccountDTO dto = new()
            {
                AccID = account.AccID,
                Active = account.active,
                UserID = UserID,
                ApplicationId = application,
                TypeId = type,
                Application = await GetApplicationByID(application),
                Type = await GetAccountTypeByID(type),
            };
            BaseUrl = _url + "api/Account";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, dto);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");
        }
        public async Task Deactivate(Account account, string Reason)
        {
            AccountDTO dto = await SetDTO(account);
            BaseUrl = _url + $"api/Account/{Reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, dto);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");
        }
        public async Task Activate(Account account)
        {
            AccountDTO dto = await SetDTO(account);
            BaseUrl = _url + $"api/Account/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, dto);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");
        }
        public async Task<List<SelectListItem>> ListActiveAccountTypes()
        {
            List<SelectListItem> accounts = new();
            var accountTypes = await GetAllAccountTypes();
            foreach (var accountType in accountTypes.Where(x => x.Active == 1))
            {
                accounts.Add(new SelectListItem(accountType.Type, accountType.TypeId.ToString()));
            }
            return accounts;
        }
        /// <summary>
        /// This will check if the account is existing
        /// </summary>
        /// <param name="account"></param>
        /// <param name="userID"></param>
        /// <param name="application"></param>
        /// <param name="type"></param>
        /// <returns>bool</returns>
        public async Task<bool> IsAccountExisting(Account account, string userID = "", int application = 0, int type = 0)
        {
            bool result = false;
            BaseUrl = _url + $"api/AccountType";
            _Client.SetBearerToken(TokenStore.Token);
            var dto = await SetDTO(account, userID, application, type);
            var response = await _Client.PostAsJsonAsync(BaseUrl,dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<bool>();  
            }
            return result;
        }
        public async Task AssignIdentity2Account(Account account, int IdenID, DateTime ValidFrom, DateTime ValidUntil, string Table)
        {
            var identities = await GetIdentityByID(IdenID);
            var Identity = identities.First();
            IdenAccount IdenAcc = new()
            {
                Identity = Identity,
                Account = account,
                ValidFrom = ValidFrom,
                ValidUntil = ValidUntil,
                LastModifiedAdmin = Admin
            };
            account.LastModfiedAdmin = Admin;
            Identity.LastModfiedAdmin = Admin;
            _context.IdenAccounts.Add(IdenAcc);
            await _context.SaveChangesAsync();
            await LogAssignAccount2Identity(Table, account.AccID, account, Identity);
            await LogAssignIden2Account("identity", IdenID, Identity, account);
        }
        public async Task ReleaseIdentity4Acount(Account account, Identity identity, int idenAccountID, string Table, string pdfFile)
        {
            var IdenAccount = _context.IdenAccounts.Where(x => x.ID == idenAccountID).First();
            IdenAccount.LastModifiedAdmin = Admin;
            IdenAccount.ValidUntil = DateTime.Now.AddDays(-1);
            _context.IdenAccounts.Update(IdenAccount);
            await LogReleaseAccountFromIdentity("identity", identity.IdenId, identity, account);
            await LogReleaseIdentity4Account(Table, account.AccID, identity, account);
            await LogPdfFile("identity",identity.IdenId, pdfFile);
            await LogPdfFile(Table,account.AccID, pdfFile);
            account.LastModfiedAdmin = Admin;
            identity.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
        }
        public async Task<ApplicationDTO> GetApplicationByID(int id)
        {
            BaseUrl = _url + $"api/Application/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<ApplicationDTO>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<SelectListItem>> ListActiveApplications()
        {
            List<SelectListItem> accounts = new();
            var applications = await GetAllApplications();
            foreach (var appliction in applications.Where(x => x.Active == 1))
            {
                accounts.Add(new SelectListItem(appliction.Name, appliction.AppID.ToString()));
            }
            return accounts;
        }
        public async Task<TypeDTO> GetAccountTypeByID(int id)
        {
            BaseUrl = _url + $"api/AccountType/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<TypeDTO>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<SelectListItem>> ListAllFreeIdentities()
        {
            List<SelectListItem> accounts = new();
            BaseUrl = _url + $"api/Identity/ListAllFreeIdentities";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode) {
                var identities = await response.Content.ReadAsJsonAsync<List<Identity>>();
                foreach (var idenity in identities)
                {
                    accounts.Add(new SelectListItem(idenity.UserID + " " + idenity.Name, idenity.IdenId.ToString()));
                }
            }
            return accounts;
        }
        public bool IsPeriodOverlapping(int AccID, DateTime ValidFrom, DateTime ValidUntil)
        {
            bool result = false;
            var accounts = _context.IdenAccounts
                    .Include(x => x.Account)
                    .Where(x => x.Account.AccID == AccID && ValidFrom <= x.ValidFrom && x.ValidUntil >= ValidUntil)
                    .ToList();
            if (accounts.Count > 0)
                result = true;
            else
                result = false;
            return result;
        }
        public async Task<List<Identity>> GetIdentityByID(int id)
        {
            List<Identity> identities = await _context.Identities
                .Include(x => x.Type)
                .Where(x => x.IdenId == id)
                .ToListAsync();
            return identities;
        }
        public async Task<List<IdenAccount>> GetIdenAccountByID(int id)
        {
            var idenAccounts = await _context.IdenAccounts
                .Include(x => x.Account)
                .ThenInclude(x => x.Application)
                .Include(x => x.Identity)
                .ThenInclude(x => x.Language)
                .Where(x => x.ID == id)
                .ToListAsync();
            return idenAccounts;
        }
        public async Task LogPdfFile(string table, Account account, string pdfFile)
        {
            await LogPdfFile("identity", account.Identities.Last().Identity.IdenId, pdfFile);
            await LogPdfFile(table, account.AccID, pdfFile);
        }
        private async Task<List<TypeDTO>> GetAllAccountTypes()
        {
            BaseUrl = _url + $"api/AccountType";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<TypeDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        private async Task<List<ApplicationDTO>> GetAllApplications()
        {
            BaseUrl = _url + $"api/Application";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<ApplicationDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        private async Task<AccountDTO> SetDTO(Account account, string userID = "", int application = 0, int type = 0)
        {
            ApplicationDTO appdto;
            TypeDTO accountTypeDTO;
            if (application != 0)
                appdto = await GetApplicationByID(application);
            else
            {
                appdto = new()
                {
                    Active = account.Application.active,
                    AppID = account.Application.AppID,
                    DeactivateReason = account.Application.DeactivateReason,
                    LastModifiedAdminId = account.Application.LastModifiedAdminId,
                    Name = account.Application.Name
                };
            }
            if (type != 0)
                accountTypeDTO = await GetAccountTypeByID(type);
            else
            {
                accountTypeDTO = new()
                {
                    Active = account.Type.active,
                    TypeId = account.Type.TypeId,
                    Type =  account.Type.Type,
                    Description = account.Type.Description,
                    DeactivateReason = account.Type.DeactivateReason,
                    LastModifiedAdminId = account.Type.LastModifiedAdminId
                };
            }
            AccountDTO dto = new()
            {
                AccID = account.AccID,
                Active = account.active,
                ApplicationId = account.ApplicationId,
                TypeId = account.TypeId,
                UserID = userID == "" ? account.UserID : userID,
                Application = appdto,
                Type = accountTypeDTO
            };
            return dto;
        }
    }
}
