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
    /// <summary>
    /// MobileService is used to manage the mobile devices
    /// </summary>
    public class MobileService : CMDBServices
    {
        /// <summary>
        /// Constructor for the MobileService
        /// </summary>
        public MobileService() : base()
        {
        }
        /// <summary>
        /// This will return a list of all mobiles
        /// </summary>
        /// <returns>List of <see cref="MobileDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<MobileDTO>> ListAll()
        {
            BaseUrl = Url + $"api/Mobile/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<MobileDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return a list of all mobiles matching the search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>list of <see cref="MobileDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<MobileDTO>> ListAll(string searchString)
        {
            BaseUrl = Url + $"api/Mobile/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<MobileDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will create a new mobile
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task CreateNew(MobileDTO mobile)
        {
            string value = $"{mobile.MobileType.AssetCategory.Category} with type {mobile.MobileType}";
            BaseUrl = Url + $"api/Mobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, mobile);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will update the mobile with the new IMEI and AssetType
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="newImei"></param>
        /// <param name="newAssetType"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Update(MobileDTO mobile, long newImei, AssetTypeDTO newAssetType)
        {
            mobile.IMEI = newImei;
            mobile.MobileType = newAssetType;
            BaseUrl = Url + $"api/Mobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, mobile);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will deactivate the mobile with the given reason
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Deactivate(MobileDTO mobile, string reason)
        {
            BaseUrl = Url + $"api/Mobile/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, mobile);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will activate the mobile with the given reason
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Activate(MobileDTO mobile)
        {
            BaseUrl = Url + $"api/Mobile/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, mobile);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will the AssetType with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="AssetTypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<AssetTypeDTO> ListAssetTypeById(int id)
        {
            BaseUrl = Url + $"api/AssetType/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<AssetTypeDTO>();
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will check if the mobile is existing in the database
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="imei"></param>
        /// <returns><see cref="bool"/></returns>
        public async Task<bool> IsMobileExisting(MobileDTO mobile, long? imei = null)
        {
            bool result = false;
            if (imei is not null)
                mobile.IMEI = (long)imei;

            BaseUrl = Url + $"api/Mobile/IsMobileExisting";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<bool>();
            }
            return result;
        }
        /// <summary>
        /// This will return the mobile with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<MobileDTO> GetMobileById(int id)
        {
            BaseUrl = Url + $"api/Mobile/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<MobileDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return a list of all free identities
        /// </summary>
        /// <returns>List of <see cref="SelectListItem"/></returns>
        public async Task<List<SelectListItem>> ListFreeIdentities()
        {
            List<SelectListItem> identites = new();
            BaseUrl = Url + $"api/Identity/ListAllFreeIdentities/mobile";
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
        /// <summary>
        /// This will return the identity with the given id
        /// </summary>
        /// <param name="IdenId"></param>
        /// <returns><see cref="IdentityDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<IdentityDTO> GetIdentity(int IdenId) 
        {
            BaseUrl = Url + $"api/Identity/{IdenId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<IdentityDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will check if the mobile is free
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="checkSub"></param>
        /// <returns></returns>
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
        /// <summary>
        /// This will assign the identity to the mobile
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task AssignIdentity2Mobile(MobileDTO mobile, IdentityDTO identity)
        {
            AssignMobileRequest assignRequest = new()
            {
                IdentityId = identity.IdenId,
                MobileIds = [mobile.MobileId]
            };
            BaseUrl = Url + $"api/Mobile/AssignIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl,assignRequest);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will release the identity from the mobile
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task ReleaseIdenity(MobileDTO mobile)
        {
            AssignMobileRequest assignRequest = new()
            {
                IdentityId = mobile.Identity.IdenId,
                MobileIds = [mobile.MobileId]
            };
            BaseUrl = Url + $"api/Mobile/ReleaseIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, assignRequest);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return a list of all free subscriptions
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListFreeMobileSubscriptions()
        {
            List<SelectListItem> identites = new();
            BaseUrl = Url + $"api/Subscription/ListAllFreeSubscriptions/Mobile";
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
        /// <summary>
        /// This will assign the subscription to the mobile
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="subscription"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task AssignSubscription(MobileDTO mobile, SubscriptionDTO subscription)
        {
            AssignMobileSubscriptionRequest request = new()
            {
                MobileId = mobile.MobileId,
                SubscriptionId = subscription.SubscriptionId
            };
            BaseUrl = Url + $"api/Mobile/AssignSubscription";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return the subscription with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="SubscriptionDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<SubscriptionDTO> GetSubribtion(int id)
        {
            BaseUrl = Url + $"api/Subscription/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<SubscriptionDTO>();
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will release the subscription from the mobile
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task ReleaseSubscription(MobileDTO mobile)
        {
            AssignMobileSubscriptionRequest request = new()
            {
                MobileId = mobile.MobileId,
                SubscriptionId = mobile.Subscription.SubscriptionId
            };
            BaseUrl = Url + $"api/Mobile/ReleaseSubscription";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode); ;
        }
    }
}
