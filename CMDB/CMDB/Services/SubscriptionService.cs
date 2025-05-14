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
    /// This class is used to manage subscriptions
    /// </summary>
    public class SubscriptionService : CMDBServices
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SubscriptionService() : base()
        {
        }
        /// <summary>
        /// This function will list all subscriptions
        /// </summary>
        /// <returns>List of <see cref="SubscriptionDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<ICollection<SubscriptionDTO>> ListAll()
        {
            BaseUrl = Url + $"api/Subscription/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<SubscriptionDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will list all subscriptions based on the search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>List of <see cref="SubscriptionDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<ICollection<SubscriptionDTO>> ListAll(string searchString)
        {
            BaseUrl = Url + $"api/Subscription/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<SubscriptionDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will get a subscriptiontype by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="SubscriptionTypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<SubscriptionDTO> GetByID(int id)
        {
            BaseUrl = Url + $"api/Subscription/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<SubscriptionDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will create a subscription
        /// </summary>
        /// <param name="type"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Create(SubscriptionTypeDTO type, string phoneNumber)
        {
            BaseUrl = Url + $"api/Subscription";
            _Client.SetBearerToken(TokenStore.Token);
            SubscriptionDTO dto = new()
            {
                SubscriptionType = type,
                PhoneNumber = phoneNumber
            };
            var response = await _Client.PostAsJsonAsync(BaseUrl, dto);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will edit a subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Edit(SubscriptionDTO subscription, string phoneNumber)
        {
            subscription.PhoneNumber = phoneNumber;
            BaseUrl = Url + $"api/Subscription";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl,subscription);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will activate a subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Activate(SubscriptionDTO subscription)
        {
            BaseUrl = Url + $"api/Subscription/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, subscription);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will deactivate a subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Deactivate(SubscriptionDTO subscription, string reason)
        {
            BaseUrl = Url + $"api/Subscription/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, subscription);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will get a subscriptiontype by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<SubscriptionTypeDTO> GetSubscriptionTypeById(int id)
        {
            BaseUrl = Url + $"api/SubscriptionType/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<SubscriptionTypeDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will get all subscriptiontypes
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetSubscriptionTypes()
        {
            List<SelectListItem> types = new();
            BaseUrl = Url + $"api/SubscriptionType/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode) { 
                var subtypes = await response.Content.ReadAsJsonAsync<List<SubscriptionTypeDTO>>();
                foreach (var subtype in subtypes.Where(x => x.Active == 1)) 
                {
                    types.Add(new SelectListItem($"{subtype}", $"{subtype.Id}"));
                }
            }
            return types;
        }
        /// <summary>
        /// This function will check if a subscription is existing
        /// </summary>
        /// <param name="subscriptionType"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<bool> IsSubscritionExisting(SubscriptionTypeDTO subscriptionType, string phoneNumber, int id =0)
        {
            if (id != 0)
            {
                var sub = await GetByID(id);
                sub.PhoneNumber = phoneNumber;
                BaseUrl = Url + $"api/Subscription/IsExisting";
                _Client.SetBearerToken(TokenStore.Token);
                var response = await _Client.PostAsJsonAsync(BaseUrl, sub);
                if (response.IsSuccessStatusCode) 
                {
                    return await response.Content.ReadAsJsonAsync<bool>();
                }
                else
                    throw new NotAValidSuccessCode(Url, response.StatusCode);
            }
            else
            {
                SubscriptionDTO sub = new()
                {
                    SubscriptionType = subscriptionType,
                    PhoneNumber = phoneNumber
                };
                BaseUrl = Url + $"api/Subscription/IsExisting";
                _Client.SetBearerToken(TokenStore.Token);
                var response = await _Client.PostAsJsonAsync(BaseUrl, sub);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsJsonAsync<bool>();
                }
                else
                    throw new NotAValidSuccessCode(Url, response.StatusCode);
            }
        }
        /// <summary>
        /// This function will list all free identities
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListFreeIdentities()
        {
            List<SelectListItem> identites = new();
            BaseUrl = Url + $"api/Identity/ListAllFreeIdentities/subscription";
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
        /// This function will list all free mobiles
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListFreeMobiles()
        {
            List<SelectListItem> identites = new();
            BaseUrl = Url + $"api/Mobile/ListAllFreeMobiles/subscription";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var mobiles = await response.Content.ReadAsJsonAsync<List<MobileDTO>>();
                foreach( var mobile in mobiles)
                {
                    identites.Add(new($"Type: {mobile.MobileType.Vendor} {mobile.MobileType.Type} IMEI:{mobile.IMEI}", $"{mobile.MobileId}"));
                }
            }
            return identites ;
        }
        /// <summary>
        /// This function will release the identity from the subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task ReleaseIdenity(SubscriptionDTO subscription, IdentityDTO identity)
        {
            AssignInternetSubscriptionRequest request = new() {
                IdentityId = identity.IdenId,
                SubscriptionIds = [subscription.SubscriptionId]
            };
            BaseUrl = Url + "api/Subscription/ReleaseIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This function will assign the identity to the subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task AssignIdentity(SubscriptionDTO subscription, IdentityDTO identity)
        {
            AssignInternetSubscriptionRequest request = new()
            {
                IdentityId = identity.IdenId,
                SubscriptionIds = [subscription.SubscriptionId]
            };
            BaseUrl = Url + "api/Subscription/AssignIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This function will get the identity by id
        /// </summary>
        /// <param name="IdenId"></param>
        /// <returns></returns>
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
        /// This function will get the mobile by id
        /// </summary>
        /// <param name="mobileId"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<MobileDTO> GetMobile(int mobileId)
        {
            BaseUrl = Url + $"api/Mobile/{mobileId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<MobileDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will assign the mobile to the subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task AssignMobile(SubscriptionDTO subscription, MobileDTO mobile)
        {
            AssignMobileSubscriptionRequest request = new()
            {
                MobileId = mobile.MobileId,
                SubscriptionId = subscription.SubscriptionId
            };
            BaseUrl = Url + "api/Subscription/AssignMobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This function will release the mobile from the subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task ReleaseMobile(SubscriptionDTO subscription, MobileDTO mobile)
        {
            AssignMobileSubscriptionRequest request = new()
            {
                MobileId = mobile.MobileId,
                SubscriptionId = subscription.SubscriptionId
            };
            BaseUrl = Url + "api/Subscription/ReleaseMobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
    }
}
