using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Infrastructure;
using CMDB.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDB.Services
{
    /// <summary>
    /// This is the AccountType service
    /// </summary>
    public class AccountTypeService : CMDBServices
    {
        /// <summary>
        /// Constructor for the AccountType service
        /// </summary>
        public AccountTypeService() : base()
        {
        }
        /// <summary>
        /// This method gets the account type by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns><see cref="TypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<TypeDTO> GetAccountTypeByID(int ID)
        {
            BaseUrl = Url + $"api/AccountType/{ID}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<TypeDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl,response.StatusCode);
        }
        /// <summary>
        /// This method creates a new account type
        /// </summary>
        /// <param name="typeDTO"><see cref="TypeDTO"/></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Create(TypeDTO typeDTO)
        {
            BaseUrl = Url + $"api/AccountType";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, typeDTO);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsJsonAsync<TypeDTO>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method updates an account type
        /// </summary>
        /// <param name="accountType"><see cref="TypeDTO"/></param>
        /// <param name="Type"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Update(TypeDTO accountType, string Type, string Description)
        {
            BaseUrl = Url + $"api/AccountType";
            _Client.SetBearerToken(TokenStore.Token);
            accountType.Type = Type;
            accountType.Description = Description;
            var response = await _Client.PutAsJsonAsync(BaseUrl,accountType);
            if(!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method deactivates an account type
        /// </summary>
        /// <param name="accountType"><see cref="TypeDTO"/></param>
        /// <param name="Reason"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Deactivate(TypeDTO accountType, string Reason)
        {
            BaseUrl = Url + $"api/AccountType/{Reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, accountType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method activates an account type
        /// </summary>
        /// <param name="accountType"><see cref="TypeDTO"/></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Activate(TypeDTO accountType)
        {
            BaseUrl = Url + $"api/AccountType/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, accountType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method gets all account types
        /// </summary>
        /// <returns>List of <see cref="TypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<TypeDTO>> ListAll()
        {
            BaseUrl = Url + $"api/AccountType/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<TypeDTO>>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method gets all account types with a search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>list of <see cref="TypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<TypeDTO>> ListAll(string searchString)
        {
            BaseUrl = Url + $"api/AccountType/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<TypeDTO>>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method checks if an account type already exists
        /// </summary>
        /// <param name="accountType"><see cref="TypeDTO"/></param>
        /// <param name="Type"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public async Task<bool> IsExisting(TypeDTO accountType, string Type = "", string Description = "")
        {
            bool result = false;
            accountType.Type = Type == "" ? accountType.Type : Type ;
            accountType.Description = Description == "" ? accountType.Description : Description;
            BaseUrl = Url + $"api/AccountType/IsExisting";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, accountType);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsJsonAsync<bool>();
            }
            return result;
        }
    }
}
