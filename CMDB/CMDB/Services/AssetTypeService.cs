using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class AssetTypeService : CMDBServices
    {
        public AssetTypeService() : base()
        {
        }
        public async Task<List<AssetTypeDTO>> ListAllAssetTypes()
        {
            BaseUrl = _url + $"api/AssetType/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<AssetTypeDTO>>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<List<AssetTypeDTO>> ListAllAssetTypes(string searchString)
        {
            BaseUrl = _url + $"api/AssetType/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<AssetTypeDTO>>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<AssetTypeDTO> GetById(int id)
        {
            BaseUrl = _url + $"api/AssetType/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<AssetTypeDTO>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task CreateNewAssetType(AssetTypeDTO assetType)
        {
            BaseUrl = _url + $"api/AssetType";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl,assetType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task UpdateAssetType(AssetTypeDTO assetType, string Vendor, string Type)
        {
            assetType.Type = Type;
            assetType.Vendor = Vendor;
            BaseUrl = _url + $"api/AssetType";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, assetType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl,response.StatusCode);
        }
        public async Task DeactivateAssetType(AssetTypeDTO assetType, string reason)
        {
            BaseUrl = _url + $"api/AssetType/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, assetType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task ActivateAssetType(AssetTypeDTO assetType)
        {
            BaseUrl = _url + $"api/AssetType/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, assetType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<bool> IsAssetTypeExisting(AssetTypeDTO assetType, string Vendor = "", string type = "")
        {
            bool result = false;
            if(!string.IsNullOrEmpty(Vendor))
                assetType.Vendor = Vendor;
            if(!string.IsNullOrEmpty(type))
                assetType.Type = type;
            BaseUrl = _url + $"api/AssetType/IsExisting";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, assetType);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<bool>();
            return result;
        }
        public async Task<List<SelectListItem>> ListActiveCategories()
        {
            List<SelectListItem> assettypes = new();
            BaseUrl = _url + $"api/AssetCategory/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var categories = await response.Content.ReadAsJsonAsync<List<AssetCategoryDTO>>();
                foreach (var category in categories.Where(x => x.Active == 1 && x.Id != 3 && x.Id != 4))
                {
                    assettypes.Add(new(category.Category, category.Id.ToString()));
                }
                return assettypes;
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<AssetCategoryDTO> ListAssetCategoryByID(int id)
        {
            BaseUrl = _url + $"api/AssetCategory/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<AssetCategoryDTO>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
    }
}
