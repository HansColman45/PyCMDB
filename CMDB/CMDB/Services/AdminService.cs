using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CMDB.Services
{
    /// <summary>
    /// The admin service
    /// </summary>
    public class AdminService : CMDBServices
    {
        /// <summary>
        /// The constructor
        /// </summary>
        public AdminService() : base()
        {
        }
        /// <summary>
        /// List all admins
        /// </summary>
        /// <returns>List of <see cref="Admin"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<Admin>> ListAll()
        {
            BaseUrl = Url + $"api/Admin/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<Admin>>();
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// List all admins with a search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<Admin>> ListAll(string searchString)
        {
            BaseUrl = Url + $"api/Admin/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<Admin>>();
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// Get an admin by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<AdminDTO> GetByID(int id)
        {
            BaseUrl = Url + $"api/Admin/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<AdminDTO>();
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// List all levels
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListAllLevels()
        {
            List<SelectListItem> Levels = new();
            for (int i = 0; i < 9; i++)
            {
                Levels.Add(new SelectListItem(i.ToString(), i.ToString()));
            }
            return Levels;
        }
        /// <summary>
        /// This will return a list of all CMDB accounts
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<SelectListItem>> ListActiveCMDBAccounts()
        {
            List<SelectListItem> Levels = new();
            BaseUrl = Url + $"api/Account/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode) { 
                var accounts = await response.Content.ReadAsJsonAsync<List<AccountDTO>>();
                foreach (var account in accounts.Where(x => x.Application.Name == "CMDB" && x.Active == 1))
                {
                    Levels.Add(new SelectListItem(account.UserID, account.AccID.ToString()));
                }
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
            return Levels;
        }
        /// <summary>
        /// This will create a new admin
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public async Task Create(AdminDTO admin)
        {
            BaseUrl = Url + $"api/Admin";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, admin);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will update an existing admin
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public async Task Update(AdminDTO admin, int level)
        {
            admin.Level = level;
            BaseUrl = Url + $"api/Admin";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, admin);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async Task Deactivate(AdminDTO admin, string reason)
        {
            BaseUrl = Url + $"api/Admin/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, admin);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public async Task Activate(AdminDTO admin)
        {
            BaseUrl = Url + $"api/Admin/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, admin);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will check if the admin already exists
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool IsExisting(AdminDTO admin)
        {
            bool result = false;
            /*var admins = _context.Admins.Include(x => x.Account).Where(x => x.Account.UserID == admin.Account.UserID);
            if(admins.Any())
                result = true;*/
            return result;
        }
        /// <summary>
        /// This will return the Account Info
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<AccountDTO> GetAccountByID(int ID)
        {
            BaseUrl = Url + $"api/Account/{ID}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var account = await response.Content.ReadAsJsonAsync<AccountDTO>();
                return account;
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
    }
}
