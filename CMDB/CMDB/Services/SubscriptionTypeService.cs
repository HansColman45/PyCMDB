using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class SubscriptionTypeService : CMDBServices
    {
        public SubscriptionTypeService() : base()
        {
        }
        public async Task<ICollection<SubscriptionTypeDTO>> ListAll()
        {
            BaseUrl = _url + $"api/SubscriptionType/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<SubscriptionTypeDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<ICollection<SubscriptionTypeDTO>> ListAll(string searchString)
        {
            BaseUrl = _url + $"api/SubscriptionType/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<SubscriptionTypeDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<SubscriptionTypeDTO> GetById(int TypeId)
        {
            BaseUrl = _url + $"api/SubscriptionType/{TypeId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var account = await response.Content.ReadAsJsonAsync<SubscriptionTypeDTO>();
                return account;
            }
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<SelectListItem>> GetCategories()
        {
            List<SelectListItem> types = new();
            BaseUrl = _url + $"api/AssetCategory/GetAll";
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
        public async Task<AssetCategoryDTO> GetAssetCategory(int Id)
        {
            BaseUrl = _url + $"api/AssetCategory/{Id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<AssetCategoryDTO>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Create(SubscriptionTypeDTO subscriptionType)
        {
            BaseUrl = _url + $"api/SubscriptionType";
            _Client.SetBearerToken(TokenStore.Token);
            await _Client.PostAsJsonAsync(BaseUrl, subscriptionType);
        }
        public async Task Edit(SubscriptionTypeDTO subscriptionType, string provider, string Type, string description)
        {
            subscriptionType.Provider = provider;
            subscriptionType.Type = Type;
            subscriptionType.Description = description;

            BaseUrl = _url + $"api/SubscriptionType";
            _Client.SetBearerToken(TokenStore.Token);
            await _Client.PutAsJsonAsync(BaseUrl, subscriptionType);
        }
        public async Task Delete(SubscriptionTypeDTO subscriptionType, string reason)
        {
            BaseUrl = _url + $"api/SubscriptionType/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            await _Client.DeleteAsJsonAsync(BaseUrl, subscriptionType);
        }
        public async Task Activate(SubscriptionTypeDTO subscriptionType)
        {
            BaseUrl = _url + $"api/SubscriptionType/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            await _Client.PostAsJsonAsync(BaseUrl, subscriptionType);
        }
    }
}
