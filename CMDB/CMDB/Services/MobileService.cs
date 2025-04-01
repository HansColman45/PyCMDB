using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Requests;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class MobileService : CMDBServices
    {
        public MobileService() : base()
        {
        }
        public async Task<List<MobileDTO>> ListAll()
        {
            BaseUrl = _url + $"api/Mobile/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<MobileDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<MobileDTO>> ListAll(string searchString)
        {
            BaseUrl = _url + $"api/Mobile/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<MobileDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task CreateNew(MobileDTO mobile)
        {
            string value = $"{mobile.MobileType.AssetCategory.Category} with type {mobile.MobileType}";
            BaseUrl = _url + $"api/Mobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, mobile);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Update(MobileDTO mobile, long newImei, AssetTypeDTO newAssetType)
        {
            mobile.IMEI = newImei;
            mobile.MobileType = newAssetType;
            BaseUrl = _url + $"api/Mobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, mobile);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Deactivate(MobileDTO mobile, string reason)
        {
            BaseUrl = _url + $"api/Mobile/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, mobile);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Activate(MobileDTO mobile)
        {
            BaseUrl = _url + $"api/Mobile/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, mobile);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<AssetTypeDTO> ListAssetTypeById(int id)
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
        public async Task<bool> IsMobileExisting(MobileDTO mobile, long? imei = null)
        {
            bool result = false;
            if (imei is not null)
                mobile.IMEI = (long)imei;

            BaseUrl = _url + $"api/Mobile/IsMobileExisting";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<bool>();
            }
            return result;
        }
        public async Task<MobileDTO> GetMobileById(int id)
        {
            BaseUrl = _url + $"api/Mobile/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<MobileDTO>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<SelectListItem>> ListFreeIdentities()
        {
            List<SelectListItem> identites = new();
            BaseUrl = _url + $"api/Identity/ListAllFreeIdentities/mobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var idens = await response.Content.ReadAsJsonAsync<List<IdentityDTO>>();
                foreach (var identity in idens)
                {
                    identites.Add(new(identity.Name + " " + identity.UserID, identity.IdenId.ToString()));
                }
            }
            return identites;
        }
        public async Task<IdentityDTO> GetIdentity(int IdenId) 
        {
            BaseUrl = _url + $"api/Identity/{IdenId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<IdentityDTO>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public bool IsDeviceFree(MobileDTO mobile, bool checkSub = false)
        {
            bool result = true;
            //TODO: Fix this
           /* if (!checkSub) { 
                var mobiles = _context.Mobiles.Where(x => x.MobileId == mobile.MobileId).First();
                if (mobiles.Identity is null || mobiles.MobileId == 1)
                    result = true;
            }
            if (checkSub)
            {
                var mobiles = _context.Mobiles
                    .Include(x => x.Subscriptions)
                    .Where(x => x.MobileId == mobile.MobileId).First();
                if(mobiles.Subscriptions.Count == 0)
                    result= true;
            }*/
            return result;
        }
        public async Task AssignIdentity2Mobile(MobileDTO mobile, IdentityDTO identity)
        {
            AssignMobileRequest assignRequest = new()
            {
                IdentityId = identity.IdenId,
                MobileIds = [mobile.MobileId]
            };
            BaseUrl = _url + $"api/Mobile/AssignIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl,assignRequest);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task ReleaseIdenity(MobileDTO mobile)
        {
            AssignMobileRequest assignRequest = new()
            {
                IdentityId = mobile.Identity.IdenId,
                MobileIds = [mobile.MobileId]
            };
            BaseUrl = _url + $"api/Mobile/ReleaseIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, assignRequest);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<SelectListItem>> ListFreeMobileSubscriptions()
        {
            List<SelectListItem> identites = new();
            BaseUrl = _url + $"api/Subscription/ListAllFreeSubscriptions/Mobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var subscriptions = await response.Content.ReadAsJsonAsync<List<SubscriptionDTO>>();
                foreach (var subscription in subscriptions)
                {
                    identites.Add(new($"Type: {subscription.SubscriptionType} on phonenumber: {subscription.PhoneNumber}", $"{subscription.SubscriptionId}"));
                }
            }
            return identites;
        }
        public async Task AssignSubscription(MobileDTO mobile, SubscriptionDTO subscription)
        {
            AssignMobileSubscriptionRequest request = new()
            {
                MobileId = mobile.MobileId,
                SubscriptionId = subscription.SubscriptionId
            };
            BaseUrl = _url + $"api/Mobile/AssignSubscription";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }

        public async Task<SubscriptionDTO> GetSubribtion(int id)
        {
            BaseUrl = _url + $"api/Subscription/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<SubscriptionDTO>();
            }
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }

        public async Task ReleaseSubscription(MobileDTO mobile)
        {
            AssignMobileSubscriptionRequest request = new()
            {
                MobileId = mobile.MobileId,
                SubscriptionId = mobile.Subscription.SubscriptionId
            };
            BaseUrl = _url + $"api/Mobile/ReleaseSubscription";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode); ;
        }
    }
}
