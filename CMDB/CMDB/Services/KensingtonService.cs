using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.Extensions.Primitives;
using Microsoft.Graph.Models.Security;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class KensingtonService : LogService
    {
        public async Task<List<KensingtonDTO>> ListAll()
        {
            BaseUrl = _url + $"api/Kensington/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<KensingtonDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<KensingtonDTO>> Search(string search)
        {
            BaseUrl = _url + $"api/Kensington/GetAll/{search}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<KensingtonDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<AssetTypeDTO> GetAssetTypeById(int id)
        {
            BaseUrl = _url + $"api/AssetType/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<AssetTypeDTO>();
            }
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Create(string serialNumber, bool hasLock, int amountOfKeys, AssetTypeDTO assetType)
        {
            KensingtonDTO kensington = new()
            {
                SerialNumber = serialNumber,
                HasLock = hasLock,
                AmountOfKeys = amountOfKeys,
                Type = assetType
            };
            BaseUrl = _url + $"api/Kensington";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, kensington);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<KensingtonDTO> GetByID(int id)
        {
            BaseUrl = _url + $"api/Kensington/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<KensingtonDTO>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Update(KensingtonDTO key)
        {
            BaseUrl = _url + $"api/Kensington";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, key);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Deactivate(KensingtonDTO key, string reason)
        {
            BaseUrl = _url + $"api/Kensington/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl,key);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Activate(KensingtonDTO key)
        {
            BaseUrl = _url + $"api/Kensington/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, key);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
    }
}
