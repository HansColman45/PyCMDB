using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class IdentityService : LogService
    {
        public IdentityService() : base()
        {
        }
        public async Task<ICollection<IdentityDTO>> ListAll()
        {
            BaseUrl = _url + $"api/Identity/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<IdentityDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<ICollection<IdentityDTO>> ListAll(string searchString)
        {
            BaseUrl = _url + $"api/Identity/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<IdentityDTO>>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<IdentityDTO> GetByID(int id)
        {
            BaseUrl = _url + $"api/Identity/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<IdentityDTO>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<SelectListItem>> ListActiveIdentityTypes()
        {
            List<SelectListItem> identityTypes = new();
            BaseUrl = _url + $"api/IdentityType/GetAll";
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
                throw new NotAValidSuccessCode(_url, response.StatusCode);
            return identityTypes;
        }
        public async Task<List<SelectListItem>> ListAllActiveLanguages()
        {
            List<SelectListItem> langs = new();
            BaseUrl = _url + $"api/Language/GetAll";
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
                throw new NotAValidSuccessCode(_url, response.StatusCode);
            return langs;
        }
        public async Task<List<DeviceDTO>> ListAllFreeDevices()
        {
            BaseUrl = _url + $"api/Device/AllFreeDevices";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<DeviceDTO>>();
            }
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<MobileDTO>> ListAllFreeMobiles()
        {
            BaseUrl = _url + $"api/Mobile/ListAllFreeMobiles";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<MobileDTO>>();
            }
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
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
            BaseUrl = _url + "api/Identity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, identity);
            if(!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
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
            BaseUrl = _url + "api/Identity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, identity);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task Deactivate(IdentityDTO identity, string reason)
        {
            BaseUrl = _url + $"api/Identity/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, identity);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task Activate(IdentityDTO identity)
        {
            BaseUrl = _url + "api/Identity/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, identity);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<bool> IsExisting(IdentityDTO identity, string UserID = "")
        {
            bool result = false;
            BaseUrl = _url + "api/Identity/IsExisting";
            _Client.SetBearerToken(TokenStore.Token);
            if(!string.IsNullOrEmpty(UserID))
                identity.UserID = UserID;
            var response = await _Client.PostAsJsonAsync(BaseUrl, identity);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<bool>();
            return result;
        }
        public async Task ReleaseDevices(IdentityDTO identity, List<DeviceDTO> devices)
        {
            AssignDeviceRequest request = new()
            {
                IdentityId = identity.IdenId,
                AssetTags = devices.Select(x => x.AssetTag).ToList(),
            };
            BaseUrl = _url + "api/Identity/ReleaseDevices";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task ReleaseMobile(IdentityDTO identity, List<MobileDTO> mobiles)
        {
            AssignMobileRequest request = new()
            {
                IdentityId = identity.IdenId,
                MobileIds = mobiles.Select(x => x.MobileId).ToList(),
            };
            BaseUrl = _url + "api/Identity/ReleaseMobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<List<SelectListItem>> ListAllFreeAccounts()
        {
            List<SelectListItem> accounts = new();
            BaseUrl = _url + "api/Account/ListAllFreeAccounts";
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
        public bool IsPeriodOverlapping(int? IdenID, DateTime ValidFrom, DateTime ValidUntil)
        {
            bool result = false;
            /*if (IdenID == null)
                throw new Exception("Missing required id");
            else
            {
                var Identity = _context.IdenAccounts
                        .Include(x => x.Identity)
                        .Where(x => x.Identity.IdenId == IdenID && ValidFrom <= x.ValidFrom && x.ValidUntil >= ValidUntil)
                        .ToList();
                if (Identity.Count > 0)
                    result = true;
                else
                    result = false;
            }*/
            return result;
        }
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
            BaseUrl = _url + $"api/Identity/AssignAccount";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, idenAccount);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task AssignDevice(IdentityDTO identity, List<DeviceDTO> devicesToAdd)
        {
            AssignDeviceRequest request = new()
            {
                IdentityId = identity.IdenId,
                AssetTags = devicesToAdd.Select(x => x.AssetTag).ToList(),
            };
            BaseUrl = _url + "api/Identity/AssignDevices";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl,request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task AssignMobiles(IdentityDTO identity, List<MobileDTO> mobiles)
        {
            AssignMobileRequest request = new()
            {
                IdentityId = identity.IdenId,
                MobileIds = mobiles.Select(x => x.MobileId).ToList(),
            };
            BaseUrl = _url + "api/Identity/AssignMobile";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task ReleaseAccount4Identity(IdenAccountDTO idenAccount)
        {
            BaseUrl = _url + $"api/Identity/ReleaseAccount";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, idenAccount);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<AccountDTO> GetAccountByID(int ID)
        {
            BaseUrl = _url + $"api/Account/{ID}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<AccountDTO>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<IdenAccountDTO> GetIdenAccountByID(int id)
        {
            BaseUrl = _url + $"api/IdenAccount/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<IdenAccountDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<DeviceDTO> GetDevice(string assetTag)
        {
            BaseUrl = _url + $"api/Device/{assetTag}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<DeviceDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<MobileDTO> GetMobile(int mobileId)
        {
            BaseUrl = _url + $"api/Mobile/{mobileId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<MobileDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        private async Task<TypeDTO> GetTypeById(int typeId)
        {
            BaseUrl = _url + $"api/IdentityType/{typeId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode) 
                return await response.Content.ReadAsJsonAsync<TypeDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl,response.StatusCode);
        }
        private async Task<LanguageDTO> GetLanguageByCode(string code)
        {
            BaseUrl = _url + $"api/Language/{code}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<LanguageDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
    }
}
