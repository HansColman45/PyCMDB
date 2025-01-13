using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class AssetCategoryService : LogService
    {
        public AssetCategoryService() : base()
        {
        }
        public async Task<List<AssetCategoryDTO>> ListAll()
        {
            BaseUrl = _url + $"api/AssetCategory/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<AssetCategoryDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<AssetCategoryDTO>> ListAll(string searchString)
        {
            BaseUrl = _url + $"api/AssetCategory/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<AssetCategoryDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<AssetCategoryDTO> ListByID(int id)
        {
            BaseUrl = _url + $"api/AssetCategory/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<AssetCategoryDTO>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Create(AssetCategoryDTO category)
        {
            BaseUrl = _url + $"api/AssetCategory";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, category);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Update(AssetCategoryDTO category, string Category, string prefix)
        {
            category.Category = Category;
            category.Prefix = prefix;
            BaseUrl = _url + $"api/AssetCategory";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl,category);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Deactivate(AssetCategoryDTO category, string Reason)
        {
            BaseUrl = _url + $"api/AssetCategory/{Reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, category);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Activate(AssetCategoryDTO category, string Table)
        {
            BaseUrl = _url + $"api/AssetCategory/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, category);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<bool> IsExisting(AssetCategoryDTO category, string Category = "")
        {
            bool result;
            if(category is not null)
                category.Category = Category;
            BaseUrl = _url + $"api/AssetCategory/IsExisting";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, category);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
            else
                result = await response.Content.ReadAsJsonAsync<bool>();
            return result;
        }
    }
}
