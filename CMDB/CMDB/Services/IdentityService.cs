using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            List<DeviceDTO> devices = new();
            /**/
            return devices;
        }
        public async Task<List<Mobile>> ListAllFreeMobiles()
        {
            /* var mobiles = await _context.Mobiles
                 .Include(x => x.MobileType)
                 .Include(x => x.Category)
                 .Where(x => x.IdentityId == 1)
                 .ToListAsync();
             return mobiles;*/
            return [];
        }
        public void GetAssingedDevices(IdentityDTO identity)
        {
            /*identity.Devices = _context.Identities
                .Include(x => x.Devices)
                .ThenInclude(x => x.Category)
                .Include(x => x.Devices)
                .ThenInclude(x => x.Type)
                .Where(x => x.IdenId == identity.IdenId)
                .SelectMany(x => x.Devices)
                .ToList();
            identity.Mobiles = _context.Identities
                .Include(x => x.Mobiles)
                .ThenInclude(x => x.Subscriptions)
                .Include(x => x.Mobiles)
                .ThenInclude(x => x.MobileType)
                .Where(x => x.IdenId == identity.IdenId)
                .SelectMany(x => x.Mobiles)
                .ToList();
            identity.Subscriptions = _context.Subscriptions
                .Include(x => x.SubscriptionType)
                .Include(x => x.Category)
                .Where(x => x.IdentityId == identity.IdenId)
                .ToList();*/
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
        public bool IsExisting(IdentityDTO identity, string UserID = "")
        {
            bool result = false;
            /*if (String.IsNullOrEmpty(UserID) && String.Compare(identity.UserID, UserID) != 0)
            {
                var idenities = _context.Identities
                .Where(x => x.UserID == UserID)
                .ToList();
                if (idenities.Count > 0)
                    result = true;
                else
                    result = false;
            }*/
            return result;
        }
        public ICollection<IdentityType> GetIdenityTypeByID(int id)
        {
            /*var types = _context.Types
                .OfType<IdentityType>()
                .Where(x => x.TypeId == id)
                .ToList();
            return types;*/
            return [];
        }
        public async Task ReleaseDevices(IdentityDTO identity, List<DeviceDTO> devices)
        {
            /*foreach (Device device in devices)
            {
                device.IdentityId = 1;
                device.LastModfiedAdmin = Admin;
                identity.Devices.Remove(device);
                await _context.SaveChangesAsync();
                switch (device.Category.Category)
                {
                    case "Laptop":
                        await LogReleaseIdentityFromDevice("laptop", identity, device);
                        break;
                    case "Desktop":
                        await LogReleaseIdentityFromDevice("desktop", identity, device);
                        break;
                    case "Token":
                        await LogReleaseIdentityFromDevice("token", identity, device);
                        break;
                    case "Docking station":
                        await LogReleaseIdentityFromDevice("docking", identity, device);
                        break;
                    case "Monitor":
                        await LogReleaseIdentityFromDevice("screen", identity, device);
                        break;
                    default:
                        throw new Exception("Category not know");
                }
                await LogReleaseDeviceFromIdenity(table, device,identity);
            }*/
        }
        public async Task ReleaseMobile(IdentityDTO identity, Mobile mobile, string table)
        {
            /*identity.LastModfiedAdmin = Admin;
            mobile.LastModfiedAdmin = Admin;
            identity.Mobiles.Remove(mobile);
            mobile.IdentityId = 1;
            await _context.SaveChangesAsync();
            await LogReleaseMobileFromIdenity("mobile", mobile, identity);
            await LogReleaseIdentityFromMobile(table, identity, mobile);*/
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
            /*var Accounts = await GetAccountByID(AccID);
            var Account = Accounts.First<Account>();
            identity.Accounts.Add(new()
            {
                Account = Account,
                ValidFrom = ValidFrom,
                ValidUntil = ValidUntil,
                LastModifiedAdmin = Admin
            });
            identity.LastModfiedAdmin = Admin;
            Account.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
            await LogAssignIden2Account(Table, identity.IdenId, identity, Account);
            await LogAssignAccount2Identity("account", AccID, Account, identity);*/
        }
        public async Task AssignDevice(IdentityDTO identity, List<DeviceDTO> devicesToAdd)
        {
            /*foreach (var device in devicesToAdd)
            {
                device.LastModfiedAdmin = Admin;
                switch (device.Category.Category)
                {
                    case "Laptop":
                        await LogAssignIdentity2Device("laptop", identity, device);
                        break;
                    case "Desktop":
                        await LogAssignIdentity2Device("desktop", identity, device);
                        break;
                    case "Token":
                        await LogAssignIdentity2Device("token", identity, device);
                        break;
                    case "Docking station":
                        await LogAssignIdentity2Device("docking", identity, device);
                        break;
                    case "Monitor":
                        await LogAssignIdentity2Device("screen", identity, device);
                        break;
                    default:
                        throw new Exception("Category not know");
                }
                await LogAssignDevice2Identity(table, device, identity);
            }*/
            //await _context.SaveChangesAsync();
        }
        public void AssignMobiles(IdentityDTO identity, List<Mobile> mobiles)
        {
            /*identity.LastModfiedAdmin = Admin;
            identity.Mobiles = mobiles;
            foreach (var mobile in mobiles)
            {
                mobile.LastModfiedAdmin = Admin;
                await LogAssignIdentity2Mobile("idenity", identity, mobile);
                await LogAssignMobile2Identity(Table,mobile, identity);
            }*/
            //await _context.SaveChangesAsync();
        }
        public async Task ReleaseAccount4Identity(IdentityDTO identity, AccountDTO account, int idenAccountID)
        {
            /*var idenAccount = _context.IdenAccounts.
                Include(x => x.Identity)
                .Where(x => x.ID == idenAccountID)
                .Single<IdenAccount>();
            idenAccount.ValidUntil = DateTime.Now;
            idenAccount.LastModifiedAdmin = Admin;
            account.LastModfiedAdmin = Admin;
            identity.LastModfiedAdmin = Admin;
            await _context.SaveChangesAsync();
            await LogReleaseAccountFromIdentity(Table, identity.IdenId, identity, account);
            await LogReleaseIdentity4Account("account", account.AccID, identity, account);*/
        }
        public async Task<List<Account>> GetAccountByID(int ID)
        {
            /*List<Account> accounts = await _context.Accounts
                .Include(x => x.Application)
                .Include(x => x.Type)
                .Where(x => x.AccID == ID)
                .ToListAsync();
            return accounts;*/
            return [];
        }
        public async Task<IdenAccountDTO> GetIdenAccountByID(int id)
        {
            BaseUrl = _url + $"api/IdenAccount/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<IdenAccountDTO>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<Device> GetDevice(string assetTag)
        {
            /*Device device = await _context.Devices
                .Include(x => x.Category)
                .Include(x => x.Type)
                .Where(x => x.AssetTag == assetTag)
                .FirstOrDefaultAsync();
            return device;*/
            return new();
        }
        public async Task<Mobile> GetMobile(int mobileId)
        {
            /*Mobile mobile = await _context.Mobiles
                .Include(x => x.MobileType)
                .Include(x => x.Category)
                .Where(x => x.MobileId == mobileId)
                .FirstOrDefaultAsync();
            return mobile;*/
            return new();
        }
        private async Task<TypeDTO> GetTypeById(int typeId)
        {
            BaseUrl = _url + $"api/IdentityType/{typeId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsJsonAsync<TypeDTO>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl,response.StatusCode);
        }
        private async Task<LanguageDTO> GetLanguageByCode(string code)
        {
            BaseUrl = _url + $"api/Language/{code}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<LanguageDTO>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
    }
}
