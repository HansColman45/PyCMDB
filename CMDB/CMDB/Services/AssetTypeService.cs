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
    /// <summary>
    /// This is the AssetType service
    /// </summary>
    public class AssetTypeService : CMDBServices
    {
        /// <summary>
        /// Constructor for the AssetType service
        /// </summary>
        public AssetTypeService() : base()
        {
        }
        /// <summary>
        /// This method returns a list of all asset types
        /// </summary>
        /// <returns><see cref="AssetTypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<AssetTypeDTO>> ListAllAssetTypes()
        {
            BaseUrl = Url + $"api/AssetType/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<AssetTypeDTO>>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method returns a list of all asset types with a search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns><see cref="AssetTypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<AssetTypeDTO>> ListAllAssetTypes(string searchString)
        {
            BaseUrl = Url + $"api/AssetType/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<AssetTypeDTO>>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method will return a AssetTypeDTO by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="AssetTypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<AssetTypeDTO> GetById(int id)
        {
            BaseUrl = Url + $"api/AssetType/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<AssetTypeDTO>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method creates a new asset type
        /// </summary>
        /// <param name="assetType"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task CreateNewAssetType(AssetTypeDTO assetType)
        {
            BaseUrl = Url + $"api/AssetType";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl,assetType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method updates an asset type
        /// </summary>
        /// <param name="assetType"><see cref="AssetTypeDTO"/></param>
        /// <param name="Vendor"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task UpdateAssetType(AssetTypeDTO assetType, string Vendor, string Type)
        {
            assetType.Type = Type;
            assetType.Vendor = Vendor;
            BaseUrl = Url + $"api/AssetType";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, assetType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl,response.StatusCode);
        }
        /// <summary>
        /// This method deactivates an asset type
        /// </summary>
        /// <param name="assetType"><see cref="AssetTypeDTO"/></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task DeactivateAssetType(AssetTypeDTO assetType, string reason)
        {
            BaseUrl = Url + $"api/AssetType/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, assetType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method activates an asset type
        /// </summary>
        /// <param name="assetType"><see cref="AssetTypeDTO"/></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task ActivateAssetType(AssetTypeDTO assetType)
        {
            BaseUrl = Url + $"api/AssetType/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, assetType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method checks if an asset type already exists
        /// </summary>
        /// <param name="assetType"><see cref="AssetTypeDTO"/></param>
        /// <param name="Vendor"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<bool> IsAssetTypeExisting(AssetTypeDTO assetType, string Vendor = "", string type = "")
        {
            bool result = false;
            if(!string.IsNullOrEmpty(Vendor))
                assetType.Vendor = Vendor;
            if(!string.IsNullOrEmpty(type))
                assetType.Type = type;
            BaseUrl = Url + $"api/AssetType/IsExisting";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, assetType);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<bool>();
            return result;
        }
        /// <summary>
        /// This method returns a list of all active asset categories
        /// </summary>
        /// <returns>List of <see cref="SelectListItem"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<SelectListItem>> ListActiveCategories()
        {
            List<SelectListItem> assettypes = new();
            BaseUrl = Url + $"api/AssetCategory/GetAll";
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
        /// <summary>
        /// This method returns a AssetCategoryDTO by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="AssetCategoryDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<AssetCategoryDTO> ListAssetCategoryByID(int id)
        {
            BaseUrl = Url + $"api/AssetCategory/{id}";
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
