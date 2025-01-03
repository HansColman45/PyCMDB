using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class AccountService : LogService
    {
        public AccountService() : base()
        {
        }
        public async Task<List<AccountDTO>> ListAll()
        {
            BaseUrl = _url + $"api/Account/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<AccountDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<AccountDTO> GetByID(int ID)
        {
            BaseUrl = _url + $"api/Account/{ID}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode) {
                var account = await response.Content.ReadAsJsonAsync<AccountDTO>();
                return account;
            }
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<AccountDTO>> ListAll(string searchString)
        {
            BaseUrl = _url + $"api/Account/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<AccountDTO>>();
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
        public async Task Edit(AccountDTO account, string UserID, int type, int application)
        {
            account.Application = await GetApplicationByID(application);
            account.Type = await GetAccountTypeByID(type);
            account.UserID = UserID;
            BaseUrl = _url + "api/Account";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, account);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");
        }
        public async Task Deactivate(AccountDTO account, string Reason)
        {
            BaseUrl = _url + $"api/Account/{Reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, account);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");
        }
        public async Task Activate(AccountDTO account)
        {
            BaseUrl = _url + $"api/Account/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, account);
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
        public async Task<bool> IsAccountExisting(AccountDTO account, string userID = "", int application = 0, int type = 0)
        {
            bool result = false;
            BaseUrl = _url + $"api/Account/IsExisting";
            _Client.SetBearerToken(TokenStore.Token);
            var dto = await SetDTO(account, userID, application, type);
            var response = await _Client.PostAsJsonAsync(BaseUrl,dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<bool>();  
            }
            return result;
        }
        public async Task AssignIdentity2Account(AccountDTO account, int IdenID, DateTime ValidFrom, DateTime ValidUntil)
        {
            var Identity = await GetIdentityByID(IdenID);
            IdenAccountDTO IdenAcc = new()
            {
                Identity = Identity,
                Account = account,
                ValidFrom = ValidFrom,
                ValidUntil = ValidUntil
            };
            BaseUrl = _url + $"api/Account/AssingIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, IdenAcc);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task ReleaseIdentity4Acount(IdenAccountDTO idenAccount)
        {
            BaseUrl = _url + $"api/Account/ReleaseIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, idenAccount);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
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
            var apps = await GetAllApplications();
            foreach (var app in apps.Where(x => x.Active == 1))
            {
                accounts.Add(new SelectListItem(app.Name, app.AppID.ToString()));
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
                var identities = await response.Content.ReadAsJsonAsync<List<IdentityDTO>>();
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
            /*var accounts = _context.IdenAccounts
                    .Include(x => x.Account)
                    .Where(x => x.Account.AccID == AccID && ValidFrom <= x.ValidFrom && x.ValidUntil >= ValidUntil)
                    .ToList();
            if (accounts.Count > 0)
                result = true;
            else
                result = false;*/
            return result;
        }
        public async Task<IdenAccountDTO> GetIdenAccountByID(int id)
        {
            BaseUrl = _url + $"api/IdenAccount/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<IdenAccountDTO>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl,response.StatusCode);
        }
        private async Task<List<TypeDTO>> GetAllAccountTypes()
        {
            BaseUrl = _url + $"api/AccountType/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<TypeDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        private async Task<IdentityDTO> GetIdentityByID(int id)
        {
            BaseUrl = _url + $"api/Identity/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var identity = await response.Content.ReadAsJsonAsync<IdentityDTO>();
                return identity;
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
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
        private async Task<AccountDTO> SetDTO(AccountDTO account, string userID = "", int application = 0, int type = 0)
        {
            ApplicationDTO appdto;
            TypeDTO accountTypeDTO;
            if (application != 0)
                appdto = await GetApplicationByID(application);
            else
            {
                appdto = new()
                {
                    Active = account.Application.Active,
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
                    Active = account.Type.Active,
                    TypeId = account.Type.TypeId,
                    Type =  account.Type.Type,
                    Description = account.Type.Description,
                    DeactivateReason = account.Type.DeactivateReason,
                    LastModifiedAdminId = account.Type.LastModifiedAdminId
                };
            }
            account.UserID = userID == "" ? account.UserID : userID;
            account.Application = appdto;
            account.Type = accountTypeDTO;
            return account;
        }
    }
}
