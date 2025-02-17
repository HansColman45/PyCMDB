using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class DevicesService : LogService
    {
        public DevicesService() : base()
        {
        }
        public async Task<List<DeviceDTO>> ListAll(string category)
        {
            BaseUrl = _url + $"api/Device/{category}/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<DeviceDTO>>();
            }
            else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ReAuthenticate();
                response = await _Client.GetAsync(BaseUrl);
                return await response.Content.ReadAsJsonAsync<List<DeviceDTO>>();
            }
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<DeviceDTO>> ListAll(string category, string searchString)
        {
            BaseUrl = _url + $"api/Device/{category}/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<DeviceDTO>>();
            }
            else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ReAuthenticate();
                response = await _Client.GetAsync(BaseUrl);
                return await response.Content.ReadAsJsonAsync<List<DeviceDTO>>();
            }
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<List<SelectListItem>> ListRams()
        {
            List<SelectListItem> assettypes = new();
            BaseUrl = _url + $"api/Device/GetAllRams";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var rams = await response.Content.ReadAsJsonAsync<List<RAM>>();
                foreach (var ram in rams)
                {
                    assettypes.Add(new(ram.Display, ram.Value.ToString()));
                }
            }
            return assettypes;
        }
        public async Task CreateNewDevice(DeviceDTO device)
        {
            BaseUrl = _url + $"api/Device";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task UpdateDevice(DeviceDTO desktop, string newRam, string newMAC, AssetTypeDTO newAssetType, string newSerialNumber)
        {
            var device = ConvertLaptopOrDesktop(desktop,newRam,newMAC,newAssetType,newSerialNumber);
            BaseUrl = _url + $"api/Device";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task UpdateDevice(DeviceDTO screen, string newSerialNumber, AssetTypeDTO newAssetType)
        {
            var device = ConvertDevice(screen, newAssetType, newSerialNumber);
            BaseUrl = _url + $"api/Device";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Deactivate(DeviceDTO device, string Reason)
        {
            BaseUrl = _url + $"api/Device/{Reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task Activate(DeviceDTO device)
        {
            BaseUrl = _url + $"api/Device/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<DeviceDTO> GetDeviceById(string category, string assetTag)
        {
            BaseUrl = _url + $"api/Device/{category}/{assetTag}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<DeviceDTO>();
            }
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<bool> IsDeviceExisting(DeviceDTO device) 
        {
            bool result = false;
            BaseUrl = _url + $"api/Device/IsExisting";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<bool>();
            return result;
        }
        public bool IsDeviceFree(DeviceDTO device)
        {
            bool result = false;
            if (device.Identity is null || device.Identity.IdenId ==1)
                return true;
            return result;
        }
        public async Task<List<SelectListItem>> ListFreeIdentities(string table)
        {
            List<SelectListItem> identites = new();
            BaseUrl = _url + $"api/Identity/ListAllFreeIdentities/{table}";
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
        public async Task<AssetTypeDTO> GetAssetTypeById(int id)
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
        public async Task<AssetCategoryDTO> GetAsstCategoryByCategory(string category)
        {
            BaseUrl = _url + $"api/AssetCategory/{category}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<AssetCategoryDTO>();
            }
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<IdentityDTO> GetAssignedIdentity(int idenId)
        {
            BaseUrl = _url + $"api/Identity/{idenId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<IdentityDTO>();
            else
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task AssignIdentity2Device(IdentityDTO identity, DeviceDTO device)
        {
            device.Identity = identity;
            BaseUrl = _url + $"api/Device/AssignIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task ReleaseIdenity(DeviceDTO device)
        {
            BaseUrl = _url + $"api/Device/ReleaseIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        private DeviceDTO ConvertDevice(DeviceDTO device, AssetTypeDTO newAssetType = null, string newSerialNumber = null)
        {
            DeviceDTO dto = new()
            {
                AssetTag = device.AssetTag,
                Active = device.Active,
                SerialNumber = newSerialNumber ?? device.SerialNumber,
                DeactivateReason = device.DeactivateReason,
                LastModifiedAdminId = device.LastModifiedAdminId,
                AssetType = new()
                {
                    Type = newAssetType == null ? device.AssetType.Type : newAssetType.Type,
                    Active = newAssetType == null ? device.AssetType.Active : newAssetType.Active,
                    DeactivateReason = newAssetType == null ? device.AssetType.DeactivateReason : newAssetType.DeactivateReason,
                    LastModifiedAdminId = newAssetType == null ? device.AssetType.LastModifiedAdminId : newAssetType.LastModifiedAdminId,
                    TypeID = newAssetType == null ? device.AssetType.TypeID : newAssetType.TypeID,
                    Vendor = newAssetType == null ? device.AssetType.Vendor : newAssetType.Vendor,
                    CategoryId = device.AssetType.CategoryId,
                    AssetCategory = new()
                    {
                        Category = device.Category.Category,
                        Active = device.Category.Active,
                        DeactivateReason = device.Category.DeactivateReason,
                        Prefix = device.Category.Prefix,
                        Id = device.Category.Id,
                        LastModifiedAdminId = device.Category.LastModifiedAdminId,
                    }
                },
                Category = new()
                {
                    Category = device.Category.Category,
                    Active = device.Category.Active,
                    DeactivateReason = device.Category.DeactivateReason,
                    Prefix = device.Category.Prefix,
                    Id = device.Category.Id,
                    LastModifiedAdminId = device.Category.LastModifiedAdminId,
                }
            };
            return dto;
        }
        private DeviceDTO ConvertLaptopOrDesktop(DeviceDTO desktop, string newRam = null, string newMAC = null, AssetTypeDTO newAssetType = null, string newSerialNumber = null)
        {
            DeviceDTO device = new()
            {
                AssetTag = desktop.AssetTag,
                Active = desktop.Active,
                RAM = newRam ?? desktop.RAM,
                MAC = newMAC ?? desktop.MAC,
                SerialNumber = newSerialNumber ?? desktop.SerialNumber,
                DeactivateReason = desktop.DeactivateReason,
                LastModifiedAdminId = desktop.LastModifiedAdminId,
                AssetType = new()
                {
                    Type = newAssetType == null ? desktop.AssetType.Type :newAssetType.Type ,
                    Active = newAssetType == null ? desktop.AssetType.Active : newAssetType.Active,
                    DeactivateReason = newAssetType == null ? desktop.AssetType.DeactivateReason : newAssetType.DeactivateReason,
                    LastModifiedAdminId = newAssetType == null ? desktop.AssetType.LastModifiedAdminId :newAssetType.LastModifiedAdminId,
                    TypeID = newAssetType == null ? desktop.AssetType.TypeID :newAssetType.TypeID,
                    Vendor = newAssetType == null ? desktop.AssetType.Vendor :newAssetType.Vendor,
                    CategoryId = desktop.AssetType.CategoryId,
                    AssetCategory = new()
                    {
                        Category = desktop.Category.Category,
                        Active = desktop.Category.Active,
                        DeactivateReason = desktop.Category.DeactivateReason,
                        Prefix = desktop.Category.Prefix,
                        Id = desktop.Category.Id,
                        LastModifiedAdminId = desktop.Category.LastModifiedAdminId,
                    }
                },
                Category = new()
                {
                    Category = desktop.Category.Category,
                    Active = desktop.Category.Active,
                    DeactivateReason = desktop.Category.DeactivateReason,
                    Prefix = desktop.Category.Prefix,
                    Id = desktop.Category.Id,
                    LastModifiedAdminId = desktop.Category.LastModifiedAdminId,
                }
            };
            return device;
        }
    }
}
