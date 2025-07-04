using CMDB.Domain.CustomExeptions;
using CMDB.Domain.DTOs;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CMDB.Services
{
    /// <summary>
    /// This is the Identity service
    /// </summary>
    public class IdentityService : CMDBServices
    {
        /// <summary>
        /// Constructor for the Identity service
        /// </summary>
        public IdentityService() : base()
        {
        }
        /// <summary>
        /// This method returns a list of all identities
        /// </summary>
        /// <returns>List of <see cref="IdentityDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<IdentityDTO>> ListAll()
        {
            BaseUrl = Url + $"api/Identity/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<IdentityDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method returns a list of all identities with a search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>List of <see cref="IdentityDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<IdentityDTO>> ListAll(string searchString)
        {
            BaseUrl = Url + $"api/Identity/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<IdentityDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will return a IdentityDTO by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="IdentityDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<IdentityDTO> GetByID(int id)
        {
            BaseUrl = Url + $"api/Identity/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<IdentityDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method returns a list of all identity types
        /// </summary>
        /// <returns>list of <see cref="SelectListItem"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<SelectListItem>> ListActiveIdentityTypes()
        {
            List<SelectListItem> identityTypes = new();
            BaseUrl = Url + $"api/IdentityType/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode) {
                var types = await response.Content.ReadAsJsonAsync<List<TypeDTO>>();
                foreach (var type in types.Where(x => x.Active == 1))
                {
                    identityTypes.Add(new SelectListItem(type.Type + " " + type.Description, type.TypeId.ToString()));
                }
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
            return identityTypes;
        }
        /// <summary>
        /// This method returns a list of all languages
        /// </summary>
        /// <returns><see cref="LanguageDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<SelectListItem>> ListAllActiveLanguages()
        {
            List<SelectListItem> langs = new();
            BaseUrl = Url + $"api/Language/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var languages = await response.Content.ReadAsJsonAsync<List<LanguageDTO>>();
                foreach (var language in languages)
                {
                    langs.Add(new SelectListItem(language.Description, language.Code));
                }
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
            return langs;
        }
        /// <summary>
        /// This method returns a list of all free devices
        /// </summary>
        /// <returns>List of <see cref="DeviceDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<DeviceDTO>> ListAllFreeDevices()
        {
            BaseUrl = Url + $"api/Device/AllFreeDevices";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<DeviceDTO>>();
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method returns a list of all free mobiles
        /// </summary>
        /// <returns>List of <see cref="MobileDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<MobileDTO>> ListAllFreeMobiles()
        {
            BaseUrl = Url + $"api/Mobile/ListAllFreeMobiles/identitiy";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<MobileDTO>>();
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method creates a new identity
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="LastName"></param>
        /// <param name="type"></param>
        /// <param name="UserID"></param>
        /// <param name="Company"></param>
        /// <param name="EMail"></param>
        /// <param name="Language"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Create(string firstName, string LastName, int type, string UserID, string Company, string EMail, string Language)
        {
            var typeDTO = await GetTypeById(type);
            var langDTO = await GetLanguageByCode(Language);
            IdentityDTO identity = new()
            {
                Company = Company,
                EMail = EMail,
                UserID = UserID,
                Active = 1,
                DeactivateReason = "",
                LastModifiedAdminId = TokenStore.AdminId,
                FirstName = firstName,
                LastName = LastName,
                Type = typeDTO,
                Language = langDTO
            };
            BaseUrl = Url + "api/Identity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, identity);
            if(!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method updates an identity
        /// </summary>
        /// <param name="identity"><see cref="IdentityDTO"/></param>
        /// <param name="firstName"></param>
        /// <param name="LastName"></param>
        /// <param name="type"></param>
        /// <param name="UserID"></param>
        /// <param name="Company"></param>
        /// <param name="EMail"></param>
        /// <param name="Language"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Edit(IdentityDTO identity, string firstName, string LastName, int type, string UserID, string Company, string EMail, string Language)
        {
            var typeDTO = await GetTypeById(type);
            var langDTO = await GetLanguageByCode(Language);
            identity.Type = typeDTO;
            identity.Language = langDTO;
            identity.FirstName = firstName;
            identity.LastName = LastName;
            identity.UserID = UserID;
            identity.Company = Company;
            identity.EMail = EMail;
            BaseUrl = Url + "api/Identity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, identity);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method deactivates an identity
        /// </summary>
        /// <param name="identity"><see cref="IdentityDTO"/></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Deactivate(IdentityDTO identity, string reason)
        {
            BaseUrl = Url + $"api/Identity/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, identity);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method activates an identity
        /// </summary>
        /// <param name="identity"><see cref="IdentityDTO"/></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Activate(IdentityDTO identity)
        {
            BaseUrl = Url + "api/Identity/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, identity);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method checks if an identity already exists
        /// </summary>
        /// <param name="identity"><see cref="IdentityDTO"/></param>
        /// <param name="Language"></param>
        /// <param name="type"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<bool> IsExisting(IdentityDTO identity, int type, string Language, string UserID = "")
        {
            var typeDTO = await GetTypeById(type);
            var langDTO = await GetLanguageByCode(Language);
            identity.Type = typeDTO;
            identity.Language = langDTO;
            bool result = false;
            BaseUrl = Url + "api/Identity/IsExisting";
            _Client.SetBearerToken(TokenStore.Token);
            if(!string.IsNullOrEmpty(UserID))
                identity.UserID = UserID;
            var response = await _Client.PostAsJsonAsync(BaseUrl, identity);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<bool>();
            return result;
        }
        /// <summary>
        /// This method releases devices from an identity
        /// </summary>
        /// <param name="identity"><see cref="IdentityDTO"/></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task ReleaseDevices(IdentityDTO identity, List<DeviceDTO> devices)
        {
            AssignDeviceRequest request = new()
            {
                IdentityId = identity.IdenId,
                AssetTags = devices.Select(x => x.AssetTag).ToList(),
            };
            BaseUrl = Url + "api/Identity/ReleaseDevices";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method releases the key from a device
        /// </summary>
        /// <param name="device"><see cref="DeviceDTO"/></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task ReleaseKensington(DeviceDTO device)
        {
            BaseUrl = Url + $"api/Device/ReleaseKensington";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method releases the key from a mobile
        /// </summary>
        /// <param name="identity"><see cref="IdentityDTO"/></param>
        /// <param name="mobiles"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task ReleaseMobile(IdentityDTO identity, List<MobileDTO> mobiles)
        {
            AssignMobileRequest request = new()
            {
                IdentityId = identity.IdenId,
                MobileIds = mobiles.Select(x => x.MobileId).ToList(),
            };
            BaseUrl = Url + "api/Identity/ReleaseMobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method returns a list of all free accounts
        /// </summary>
        /// <returns>List of <see cref="SelectListItem"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<SelectListItem>> ListAllFreeAccounts()
        {
            List<SelectListItem> accounts = new();
            BaseUrl = Url + "api/Account/ListAllFreeAccounts";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var infos = await response.Content.ReadAsJsonAsync<List<IdentityAccountInfo>>();
                foreach (var info in infos) 
                {
                    accounts.Add(new(info.UserId + " " + info.Name, info.AccId.ToString()));
                }
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
            return accounts;
        }
        /// <summary>
        /// This method returns a list of all free internet subscriptions
        /// </summary>
        /// <returns>List of <see cref="SubscriptionDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<SubscriptionDTO>> ListFreeInternetSubscriptions()
        {
            BaseUrl = Url + $"api/Subscription/ListAllFreeSubscriptions/Internet";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var list = await response.Content.ReadAsJsonAsync<List<SubscriptionDTO>>();
                return list;
            }
            else 
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method checks if a period is overlapping with an existing identity
        /// </summary>
        /// <param name="IdenID">The Id of the Identity</param>
        /// <param name="ValidFrom"></param>
        /// <param name="ValidUntil"></param>
        /// <returns></returns>
        public async Task<bool> IsPeriodOverlapping(int IdenID, DateTime ValidFrom, DateTime ValidUntil)
        {
            bool result = false;
            IsPeriodOverlappingRequest request = new()
            {
                IdentityId = IdenID,
                StartDate = ValidFrom,
                EndDate = ValidUntil,
            };
            BaseUrl = Url + $"api/IdenAccount/IsPeriodOverlapping";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl,request);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsJsonAsync<bool>();
            }
            return result;
        }
        /// <summary>
        /// This method assigns an account to an identity
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="AccID"></param>
        /// <param name="ValidFrom"></param>
        /// <param name="ValidUntil"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task AssignAccount2Idenity(IdentityDTO identity, int AccID, DateTime ValidFrom, DateTime ValidUntil)
        {
            var Account = await GetAccountByID(AccID);
            IdenAccountDTO idenAccount = new()
            {
                Identity = identity,
                Account = Account,
                ValidFrom = ValidFrom,
                ValidUntil = ValidUntil
            };
            BaseUrl = Url + $"api/Identity/AssignAccount";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, idenAccount);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method assigns a device to an identity
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="devicesToAdd"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task AssignDevice(IdentityDTO identity, List<DeviceDTO> devicesToAdd)
        {
            AssignDeviceRequest request = new()
            {
                IdentityId = identity.IdenId,
                AssetTags = devicesToAdd.Select(x => x.AssetTag).ToList(),
            };
            BaseUrl = Url + "api/Identity/AssignDevices";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl,request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method assigns a mobile to an identity
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="mobiles"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task AssignMobiles(IdentityDTO identity, List<MobileDTO> mobiles)
        {
            AssignMobileRequest request = new()
            {
                IdentityId = identity.IdenId,
                MobileIds = mobiles.Select(x => x.MobileId).ToList(),
            };
            BaseUrl = Url + "api/Identity/AssignMobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method assigns a subscription to an identity
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="subscriptions"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task AssignSubscription(IdentityDTO identity, List<SubscriptionDTO> subscriptions)
        {
            AssignInternetSubscriptionRequest request = new()
            {
                IdentityId = identity.IdenId,
                SubscriptionIds = subscriptions.Select(x => x.SubscriptionId).ToList()
            };
            BaseUrl = Url + "api/Identity/AssignSubscription";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method releases a subscription from an identity
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="subscriptions"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task ReleaseSubscription(IdentityDTO identity, List<SubscriptionDTO> subscriptions)
        {
            AssignInternetSubscriptionRequest request = new()
            {
                IdentityId = identity.IdenId,
                SubscriptionIds = subscriptions.Select(x => x.SubscriptionId).ToList()
            };
            BaseUrl = Url + "api/Identity/ReleaseSubscription";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method releases an account from an identity
        /// </summary>
        /// <param name="idenAccount"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task ReleaseAccount4Identity(IdenAccountDTO idenAccount)
        {
            BaseUrl = Url + $"api/Identity/ReleaseAccount";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, idenAccount);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method returns an account by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<AccountDTO> GetAccountByID(int ID)
        {
            BaseUrl = Url + $"api/Account/{ID}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<AccountDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method returns an IdenAccount by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<IdenAccountDTO> GetIdenAccountByID(int id)
        {
            BaseUrl = Url + $"api/IdenAccount/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<IdenAccountDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method returns a device by asset tag
        /// </summary>
        /// <param name="assetTag"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<DeviceDTO> GetDevice(string assetTag)
        {
            BaseUrl = Url + $"api/Device/{assetTag}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<DeviceDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method returns a mobile by ID
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
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This method returns a subscription by ID
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<SubscriptionDTO> GetSubscriptionById(int subscriptionId)
        {
            BaseUrl = Url + $"api/Subscription/{subscriptionId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<SubscriptionDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }

        private async Task<TypeDTO> GetTypeById(int typeId)
        {
            BaseUrl = Url + $"api/IdentityType/{typeId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode) 
                return await response.Content.ReadAsJsonAsync<TypeDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl,response.StatusCode);
        }
        private async Task<LanguageDTO> GetLanguageByCode(string code)
        {
            BaseUrl = Url + $"api/Language/{code}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<LanguageDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
    }
}
