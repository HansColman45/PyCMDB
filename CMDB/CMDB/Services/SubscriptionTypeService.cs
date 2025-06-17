using CMDB.Domain.CustomExeptions;
using CMDB.Domain.DTOs;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Services
{
    /// <summary>
    /// The SubscriptionTypeService class
    /// </summary>
    public class SubscriptionTypeService : CMDBServices
    {
        /// <summary>
        /// Constructor for the SubscriptionTypeService class
        /// </summary>
        public SubscriptionTypeService() : base()
        {
        }
        /// <summary>
        /// This will return a list of all the subscription types
        /// </summary>
        /// <returns>List of <see cref="SubscriptionTypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<ICollection<SubscriptionTypeDTO>> ListAll()
        {
            BaseUrl = Url + $"api/SubscriptionType/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<SubscriptionTypeDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return a list of all the subscription types based on the search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>List of <see cref="SubscriptionTypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<ICollection<SubscriptionTypeDTO>> ListAll(string searchString)
        {
            BaseUrl = Url + $"api/SubscriptionType/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<SubscriptionTypeDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This wull reurn the subscriptionType by the id
        /// </summary>
        /// <param name="TypeId"></param>
        /// <returns><see cref="SubscriptionTypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<SubscriptionTypeDTO> GetById(int TypeId)
        {
            BaseUrl = Url + $"api/SubscriptionType/{TypeId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var account = await response.Content.ReadAsJsonAsync<SubscriptionTypeDTO>();
                return account;
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return all Asset Categories
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetCategories()
        {
            List<SelectListItem> types = new();
            BaseUrl = Url + $"api/AssetCategory/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var categories = await response.Content.ReadAsJsonAsync<List<AssetCategoryDTO>>();
                foreach (var category in categories.Where(x => x.Category.Contains("Subscription")))
                {
                    types.Add(new SelectListItem($"{category.Category}", $"{category.Id}"));
                }
            }
            return types;
        }
        /// <summary>
        /// This will return the Asset Category by the id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<AssetCategoryDTO> GetAssetCategory(int Id)
        {
            BaseUrl = Url + $"api/AssetCategory/{Id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<AssetCategoryDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will create a new subscription type
        /// </summary>
        /// <param name="subscriptionType"><see cref="SubscriptionTypeDTO"/></param>
        /// <returns></returns>
        public async Task Create(SubscriptionTypeDTO subscriptionType)
        {
            BaseUrl = Url + $"api/SubscriptionType";
            _Client.SetBearerToken(TokenStore.Token);
            await _Client.PostAsJsonAsync(BaseUrl, subscriptionType);
        }
        /// <summary>
        /// This will edit the subscription type
        /// </summary>
        /// <param name="subscriptionType"><see cref="SubscriptionTypeDTO"/></param>
        /// <param name="provider"></param>
        /// <param name="Type"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task Edit(SubscriptionTypeDTO subscriptionType, string provider, string Type, string description)
        {
            subscriptionType.Provider = provider;
            subscriptionType.Type = Type;
            subscriptionType.Description = description;

            BaseUrl = Url + $"api/SubscriptionType";
            _Client.SetBearerToken(TokenStore.Token);
            await _Client.PutAsJsonAsync(BaseUrl, subscriptionType);
        }
        /// <summary>
        /// This will deactivate the subscription type
        /// </summary>
        /// <param name="subscriptionType"><see cref="SubscriptionTypeDTO"/></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async Task Delete(SubscriptionTypeDTO subscriptionType, string reason)
        {
            BaseUrl = Url + $"api/SubscriptionType/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            await _Client.DeleteAsJsonAsync(BaseUrl, subscriptionType);
        }
        /// <summary>
        /// This will activate the subscription type
        /// </summary>
        /// <param name="subscriptionType"><see cref="SubscriptionTypeDTO"/></param>
        /// <returns></returns>
        public async Task Activate(SubscriptionTypeDTO subscriptionType)
        {
            BaseUrl = Url + $"api/SubscriptionType/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            await _Client.PostAsJsonAsync(BaseUrl, subscriptionType);
        }
    }
}
