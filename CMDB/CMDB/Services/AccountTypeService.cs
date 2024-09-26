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
        public async Task Update(TypeDTO accountType, string Type, string Description)
        {
            BaseUrl = _url + $"api/AccountType";
            _Client.SetBearerToken(TokenStore.Token);
            accountType.Type = Type;
            accountType.Description = Description;
            var response = await _Client.PutAsJsonAsync(BaseUrl,accountType);
            if(!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task Deactivate(TypeDTO accountType, string Reason)
        {
            BaseUrl = _url + $"api/AccountType/{Reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, accountType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task Activate(TypeDTO accountType)
        {
            BaseUrl = _url + $"api/AccountType/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, accountType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<List<TypeDTO>> ListAll()
        {
            BaseUrl = _url + $"api/AccountType/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<TypeDTO>>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<List<TypeDTO>> ListAll(string searchString)
        {
            BaseUrl = _url + $"api/AccountType/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<TypeDTO>>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<bool> IsExisting(TypeDTO accountType, string Type = "", string Description = "")
        {
            bool result = false;
            accountType.Type = Type == "" ? accountType.Type : Type ;
            accountType.Description = Description == "" ? accountType.Description : Description;
            BaseUrl = _url + $"api/AccountType/IsTypeExisting";
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
