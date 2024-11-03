using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Util;

namespace CMDB.Services
{
    public class MobileService : LogService
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
            string value = $"{mobile.Category.Category} with type {mobile.MobileType}";
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
        public bool IsMobileExisting(MobileDTO mobile, long? imei = null)
        {
            bool result = false;
            /*if (imei != null && mobile.IMEI != imei)
            {
                var mobiles = _context.Mobiles.Where(x => x.IMEI == imei).ToList();
                if (mobiles.Count > 0)
                    result = true;
            }
            else if (imei != null && mobile.IMEI == imei)
                result = false;
            else
            {
                var mobiles = _context.Mobiles.Where(x => x.IMEI == mobile.IMEI).ToList();
                if (mobiles.Count > 0)
                    result = true;
            }*/
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
            BaseUrl = _url + $"api/Identity/ListAllFreeIdentities";
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
            bool result = false;
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
            mobile.Identity = identity;
            /*identity.LastModfiedAdmin = Admin;
            mobile.LastModfiedAdmin = Admin;
            identity.Mobiles.Add(mobile);
            await _context.SaveChangesAsync();
            await LogAssignMobile2Identity("identity",mobile,identity);
            await LogAssignIdentity2Mobile(table, identity, mobile);*/
        }
        public async Task ReleaseIdenity(MobileDTO mobile)
        {
            /*identity.LastModfiedAdmin = Admin;
            mobile.LastModfiedAdmin = Admin;
            identity.Mobiles.Remove(mobile);
            mobile.Identity = null;
            await _context.SaveChangesAsync();
            await LogReleaseMobileFromIdenity(table,mobile,identity);
            await LogReleaseIdentityFromMobile("identity", identity, mobile);*/
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
            subscription.Mobile = mobile;
            /*mobile.LastModfiedAdmin = Admin;
            subscription.LastModfiedAdmin = Admin;
            mobile.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            await LogAssignSubscription2Mobile(table, mobile, subscription);
            await LogAssignMobile2Subscription("mobile",subscription,mobile);*/
        }

        public async Task<SubscriptionDTO?> GetSubribtion(int id)
        {
            BaseUrl = _url + $"api/Subscription/ListAllFreeSubscriptions/Mobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<SubscriptionDTO>();
            }
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
    }
}
