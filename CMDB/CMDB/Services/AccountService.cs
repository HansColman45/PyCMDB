using CMDB.Domain.CustomExeptions;
using CMDB.Domain.DTOs;
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
    /// <summary>
    /// This service is used to manage accounts
    /// </summary>
    public class AccountService : CMDBServices
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AccountService() : base()
        {
        }
        /// <summary>
        /// This will return all accounts
        /// </summary>
        /// <returns>List of <see cref="AccountDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<AccountDTO>> ListAll()
        {
            BaseUrl = Url + $"api/Account/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<AccountDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This willr return a secific account by ID
        /// </summary>
        /// <param name="ID">The key of the account</param>
        /// <returns><see cref="AccountDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<AccountDTO> GetByID(int ID)
        {
            BaseUrl = Url + $"api/Account/{ID}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode) {
                var account = await response.Content.ReadAsJsonAsync<AccountDTO>();
                return account;
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return all accounts containing the search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>List of <see cref="AccountDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<AccountDTO>> ListAll(string searchString)
        {
            BaseUrl = Url + $"api/Account/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<AccountDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will create a new account
        /// </summary>
        /// <param name="dTO">The new Account <see cref="AccountDTO"/></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task CreateNew(AccountDTO dTO)
        {
            BaseUrl = Url + "api/Account";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, dTO);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");
        }
        /// <summary>
        /// This will edit an existing account
        /// </summary>
        /// <param name="account">The existing Account <see cref="AccountDTO"/></param>
        /// <param name="UserID">The new UserId</param>
        /// <param name="type">The new TypeID</param>
        /// <param name="application">The new applicationId</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Edit(AccountDTO account, string UserID, int type, int application)
        {
            account.Application = await GetApplicationByID(application);
            account.Type = await GetAccountTypeByID(type);
            account.UserID = UserID;
            BaseUrl = Url + "api/Account";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, account);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");
        }
        /// <summary>
        /// This will deactivate an account
        /// </summary>
        /// <param name="account">The exisiting Account <see cref="AccountDTO"/></param>
        /// <param name="Reason">The reason</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Deactivate(AccountDTO account, string Reason)
        {
            BaseUrl = Url + $"api/Account/{Reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, account);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");
        }
        /// <summary>
        /// This will activate an account
        /// </summary>
        /// <param name="account">The existing account <see cref="AccountDTO"/></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Activate(AccountDTO account)
        {
            BaseUrl = Url + $"api/Account/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, account);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");
        }
        /// <summary>
        /// This will return all account types
        /// </summary>
        /// <returns>List of <see cref="SelectListItem"/></returns>
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
        /// <param name="account"><see cref="AccountDTO"/></param>
        /// <param name="userID"></param>
        /// <param name="application"></param>
        /// <param name="type"></param>
        /// <returns>bool</returns>
        public async Task<bool> IsAccountExisting(AccountDTO account, string userID = "", int application = 0, int type = 0)
        {
            bool result = false;
            _Client.SetBearerToken(TokenStore.Token);
            var dto = await SetDTO(account, userID, application, type);
            BaseUrl = Url + $"api/Account/IsExisting";
            var response = await _Client.PostAsJsonAsync(BaseUrl,dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<bool>();  
            }
            return result;
        }
        /// <summary>
        /// This will assign an identity to an account
        /// </summary>
        /// <param name="account"><see cref="AccountDTO"/></param>
        /// <param name="IdenID"></param>
        /// <param name="ValidFrom"></param>
        /// <param name="ValidUntil"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
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
            BaseUrl = Url + $"api/Account/AssingIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, IdenAcc);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will release an identity from an account
        /// </summary>
        /// <param name="idenAccount"><see cref="IdenAccountDTO"/></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task ReleaseIdentity4Acount(IdenAccountDTO idenAccount)
        {
            BaseUrl = Url + $"api/Account/ReleaseIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, idenAccount);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return the application by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ApplicationDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<ApplicationDTO> GetApplicationByID(int id)
        {
            BaseUrl = Url + $"api/Application/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<ApplicationDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return all applications
        /// </summary>
        /// <returns>List of <see cref="SelectListItem"/></returns>
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
        /// <summary>
        /// This will return the accounttype by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="TypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<TypeDTO> GetAccountTypeByID(int id)
        {
            BaseUrl = Url + $"api/AccountType/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<TypeDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return all free identities
        /// </summary>
        /// <returns>List of <see cref="SelectListItem"/></returns>
        public async Task<List<SelectListItem>> ListAllFreeIdentities()
        {
            List<SelectListItem> accounts = new();
            BaseUrl = Url + $"api/Identity/ListAllFreeIdentities/account";
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
        /// <summary>
        /// This will check if the period is overlapping with an existing account
        /// </summary>
        /// <param name="AccID"></param>
        /// <param name="ValidFrom"></param>
        /// <param name="ValidUntil"></param>
        /// <returns></returns>
        public bool IsPeriodOverlapping(int AccID, DateTime ValidFrom, DateTime ValidUntil)
        {
            bool result = false;
            //TODO: Fix this
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
        /// <summary>
        /// This will return the IdenAccount by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="IdenAccountDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<IdenAccountDTO> GetIdenAccountByID(int id)
        {
            BaseUrl = Url + $"api/IdenAccount/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<IdenAccountDTO>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl,response.StatusCode);
        }
        /// <summary>
        /// This will return all accounttypes
        /// </summary>
        /// <returns>List of <see cref="TypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        private async Task<List<TypeDTO>> GetAllAccountTypes()
        {
            BaseUrl = Url + $"api/AccountType/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<TypeDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return the identity by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="IdentityDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        private async Task<IdentityDTO> GetIdentityByID(int id)
        {
            BaseUrl = Url + $"api/Identity/{id}";
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
        /// <summary>
        /// This will return all applications
        /// </summary>
        /// <returns>List of <see cref="ApplicationDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        private async Task<List<ApplicationDTO>> GetAllApplications()
        {
            BaseUrl = Url + $"api/Application";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<ApplicationDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will set the DTO for the account
        /// </summary>
        /// <param name="account"></param>
        /// <param name="userID"></param>
        /// <param name="application"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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
