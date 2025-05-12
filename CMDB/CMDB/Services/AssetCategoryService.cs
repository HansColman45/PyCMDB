using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Infrastructure;
using CMDB.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDB.Services
{
    /// <summary>
    /// This is the AssetCategory service
    /// </summary>
    public class AssetCategoryService : CMDBServices
    {
        /// <summary>
        /// Constructor for the AssetCategory service
        /// </summary>
        public AssetCategoryService() : base()
        {
        }
        /// <summary>
        /// This method returns a list of all asset categories
        /// </summary>
        /// <returns>List of <see cref="AssetCategoryDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<AssetCategoryDTO>> ListAll()
        {
            BaseUrl = Url + $"api/AssetCategory/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<AssetCategoryDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method returns a list of all asset categories with a search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>List of <see cref="AssetCategoryDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<AssetCategoryDTO>> ListAll(string searchString)
        {
            BaseUrl = Url + $"api/AssetCategory/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<AssetCategoryDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will return a AssetCategoryDTO by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="AssetCategoryDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<AssetCategoryDTO> GetByID(int id)
        {
            BaseUrl = Url + $"api/AssetCategory/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<AssetCategoryDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will create a new asset category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Create(AssetCategoryDTO category)
        {
            BaseUrl = Url + $"api/AssetCategory";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, category);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will update an asset category
        /// </summary>
        /// <param name="category"><see cref="AssetCategoryDTO"/></param>
        /// <param name="Category"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Update(AssetCategoryDTO category, string Category, string prefix)
        {
            category.Category = Category;
            category.Prefix = prefix;
            BaseUrl = Url + $"api/AssetCategory";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl,category);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will deactivate an asset category
        /// </summary>
        /// <param name="category"><see cref="AssetCategoryDTO"/></param>
        /// <param name="Reason"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Deactivate(AssetCategoryDTO category, string Reason)
        {
            BaseUrl = Url + $"api/AssetCategory/{Reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, category);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will activate an asset category
        /// </summary>
        /// <param name="category"><see cref="AssetCategoryDTO"/></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Activate(AssetCategoryDTO category)
        {
            BaseUrl = Url + $"api/AssetCategory/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, category);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will check if an asset category exists
        /// </summary>
        /// <param name="category"><see cref="AssetCategoryDTO"/></param>
        /// <param name="Category"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<bool> IsExisting(AssetCategoryDTO category, string Category = "")
        {
            bool result;
            if(category is not null)
                category.Category = Category;
            BaseUrl = Url + $"api/AssetCategory/IsExisting";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, category);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
            else
                result = await response.Content.ReadAsJsonAsync<bool>();
            return result;
        }
    }
}
