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
    public class SubscriptionService : LogService
    {
        public SubscriptionService() : base()
        {
        }
        public async Task<ICollection<SubscriptionDTO>> ListAll()
        {
            BaseUrl = _url + $"api/Subscription/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<SubscriptionDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<ICollection<SubscriptionDTO>> ListAll(string searchString)
        {
            BaseUrl = _url + $"api/Subscription/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<SubscriptionDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<SubscriptionDTO> GetByID(int id)
        {
            BaseUrl = _url + $"api/Subscription/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<SubscriptionDTO>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Create(SubscriptionTypeDTO type, string phoneNumber)
        {
            BaseUrl = _url + $"api/Subscription";
            _Client.SetBearerToken(TokenStore.Token);
            SubscriptionDTO dto = new()
            {
                SubscriptionType = type,
                PhoneNumber = phoneNumber
            };
            var response = await _Client.PostAsJsonAsync(BaseUrl, dto);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Edit(SubscriptionDTO subscription, string phoneNumber)
        {
            subscription.PhoneNumber = phoneNumber;
            BaseUrl = _url + $"api/Subscription";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl,subscription);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Activate(SubscriptionDTO subscription)
        {
            BaseUrl = _url + $"api/Subscription/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, subscription);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Deactivate(SubscriptionDTO subscription, string reason)
        {
            BaseUrl = _url + $"api/Subscription/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, subscription);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<SubscriptionTypeDTO> GetSubscriptionTypeById(int id)
        {
            BaseUrl = _url + $"api/SubscriptionType/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<SubscriptionTypeDTO>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<SelectListItem>> GetSubscriptionTypes()
        {
            List<SelectListItem> types = new();
            BaseUrl = _url + $"api/SubscriptionType/GetAll";
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
        public async Task<bool> IsSubscritionExisting(SubscriptionTypeDTO subscriptionType, string phoneNumber, int id =0)
        {
            if (id != 0)
            {
                var sub = await GetByID(id);
                sub.PhoneNumber = phoneNumber;
                BaseUrl = _url + $"api/Subscription/IsExisting";
                _Client.SetBearerToken(TokenStore.Token);
                var response = await _Client.PostAsJsonAsync(BaseUrl, sub);
                if (response.IsSuccessStatusCode) 
                {
                    return await response.Content.ReadAsJsonAsync<bool>();
                }
                else
                    throw new NotAValidSuccessCode(_url, response.StatusCode);
            }
            else
            {
                SubscriptionDTO sub = new()
                {
                    SubscriptionType = subscriptionType,
                    PhoneNumber = phoneNumber
                };
                BaseUrl = _url + $"api/Subscription/IsExisting";
                _Client.SetBearerToken(TokenStore.Token);
                var response = await _Client.PostAsJsonAsync(BaseUrl, sub);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsJsonAsync<bool>();
                }
                else
                    throw new NotAValidSuccessCode(_url, response.StatusCode);
            }
        }
        public async Task<List<SelectListItem>> ListFreeIdentities()
        {
            List<SelectListItem> identites = new();
            BaseUrl = _url + $"api/Identity/ListAllFreeIdentities/subscription";
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
        public async Task<List<SelectListItem>> ListFreeMobiles()
        {
            List<SelectListItem> identites = new();
            BaseUrl = _url + $"api/Mobile/ListAllFreeMobiles/subscription";
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
        public async Task ReleaseIdenity(SubscriptionDTO subscription, IdentityDTO identity)
        {
            AssignInternetSubscriptionRequest request = new() {
                IdentityId = identity.IdenId,
                SubscriptionIds = [subscription.SubscriptionId]
            };
            BaseUrl = _url + "api/Subscription/ReleaseIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task AssignIdentity(SubscriptionDTO subscription, IdentityDTO identity)
        {
            AssignInternetSubscriptionRequest request = new()
            {
                IdentityId = identity.IdenId,
                SubscriptionIds = [subscription.SubscriptionId]
            };
            BaseUrl = _url + "api/Subscription/AssignIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
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
        public async Task<MobileDTO> GetMobile(int mobileId)
        {
            BaseUrl = _url + $"api/Mobile/{mobileId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<MobileDTO>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task AssignMobile(SubscriptionDTO subscription, MobileDTO mobile)
        {
            AssignMobileSubscriptionRequest request = new()
            {
                MobileId = mobile.MobileId,
                SubscriptionId = subscription.SubscriptionId
            };
            BaseUrl = _url + "api/Subscription/AssignMobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task ReleaseMobile(SubscriptionDTO subscription, MobileDTO mobile)
        {
            AssignMobileSubscriptionRequest request = new()
            {
                MobileId = mobile.MobileId,
                SubscriptionId = subscription.SubscriptionId
            };
            BaseUrl = _url + "api/Subscription/ReleaseMobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
    }
}
