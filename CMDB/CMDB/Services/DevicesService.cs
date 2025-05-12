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
    /// <summary>
    /// This is the Devices service
    /// </summary>
    public class DevicesService : CMDBServices
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DevicesService() : base()
        {
        }
        /// <summary>
        /// This method gets all devices by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<DeviceDTO>> ListAll(string category)
        {
            BaseUrl = Url + $"api/Device/{category}/GetAll";
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
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method gets all devices by category and searchstring
        /// </summary>
        /// <param name="category"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<DeviceDTO>> ListAll(string category, string searchString)
        {
            BaseUrl = Url + $"api/Device/{category}/GetAll/{searchString}";
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
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method gets all listed RAM
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListRams()
        {
            List<SelectListItem> assettypes = new();
            BaseUrl = Url + $"api/Device/GetAllRams";
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
        /// <summary>
        /// This method will create a new device
        /// </summary>
        /// <param name="device"><see cref="DeviceDTO"/></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task CreateNewDevice(DeviceDTO device)
        {
            BaseUrl = Url + $"api/Device";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will update a device
        /// </summary>
        /// <param name="desktop">The <see cref="DeviceDTO"/></param>
        /// <param name="newRam"></param>
        /// <param name="newMAC"></param>
        /// <param name="newAssetType"></param>
        /// <param name="newSerialNumber"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task UpdateDevice(DeviceDTO desktop, string newRam, string newMAC, AssetTypeDTO newAssetType, string newSerialNumber)
        {
            var device = ConvertLaptopOrDesktop(desktop,newRam,newMAC,newAssetType,newSerialNumber);
            BaseUrl = Url + $"api/Device";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will update a device
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="newSerialNumber"></param>
        /// <param name="newAssetType"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task UpdateDevice(DeviceDTO screen, string newSerialNumber, AssetTypeDTO newAssetType)
        {
            var device = ConvertDevice(screen, newAssetType, newSerialNumber);
            BaseUrl = Url + $"api/Device";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will deactivate a device
        /// </summary>
        /// <param name="device"><see cref="DeviceDTO"/></param>
        /// <param name="Reason"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Deactivate(DeviceDTO device, string Reason)
        {
            BaseUrl = Url + $"api/Device/{Reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will activate a device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Activate(DeviceDTO device)
        {
            BaseUrl = Url + $"api/Device/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will get a device by category and assettag
        /// </summary>
        /// <param name="category"></param>
        /// <param name="assetTag"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<DeviceDTO> GetDeviceById(string category, string assetTag)
        {
            BaseUrl = Url + $"api/Device/{category}/{assetTag}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<DeviceDTO>();
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will check if a device is existing
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public async Task<bool> IsDeviceExisting(DeviceDTO device) 
        {
            bool result = false;
            BaseUrl = Url + $"api/Device/IsExisting";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<bool>();
            return result;
        }
        /// <summary>
        /// This method will check if the device is not linked to an identity
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public bool IsDeviceFree(DeviceDTO device)
        {
            bool result = false;
            if (device.Identity is null || device.Identity.IdenId ==1)
                return true;
            return result;
        }
        /// <summary>
        /// This method will get all free identities
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListFreeIdentities(string table)
        {
            List<SelectListItem> identites = new();
            BaseUrl = Url + $"api/Identity/ListAllFreeIdentities/{table}";
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
        /// This method will get the assetType by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<AssetTypeDTO> GetAssetTypeById(int id)
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
        /// This method will get the assetType by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<AssetCategoryDTO> GetAsstCategoryByCategory(string category)
        {
            BaseUrl = Url + $"api/AssetCategory/{category}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<AssetCategoryDTO>();
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will get the identity by id
        /// </summary>
        /// <param name="idenId"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<IdentityDTO> GetAssignedIdentity(int idenId)
        {
            BaseUrl = Url + $"api/Identity/{idenId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<IdentityDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will assign an identity to a device
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task AssignIdentity2Device(IdentityDTO identity, DeviceDTO device)
        {
            device.Identity = identity;
            BaseUrl = Url + $"api/Device/AssignIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will release the identity from a device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task ReleaseIdenity(DeviceDTO device)
        {
            BaseUrl = Url + $"api/Device/ReleaseIdentity";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will get the kensington by id
        /// </summary>
        /// <param name="keyId"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<KensingtonDTO> GetKensingtonById(int keyId)
        {
            BaseUrl = Url + $"api/Kensington/{keyId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<KensingtonDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will get all free keys
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListFreeKeys()
        {
            List<SelectListItem> keys = new();
            BaseUrl = Url + $"api/Kensington/GetAllFreeKeys";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var kensingtons = await response.Content.ReadAsJsonAsync<List<KensingtonDTO>>();
                foreach (var kensington in kensingtons)
                {
                    keys.Add(new($"Kensington {kensington.Type.Vendor} {kensington.Type.Type} and serialnumber: {kensington.SerialNumber}", kensington.KeyID.ToString()));
                }
            }
            return keys;
        }
        /// <summary>
        /// This method will assign a kensington to a device
        /// </summary>
        /// <param name="kensington"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task AssignKensington2Device(KensingtonDTO kensington, DeviceDTO device)
        {
            device.Kensington = kensington;
            BaseUrl = Url + $"api/Device/AssignKensington";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method will release the kensington from a device
        /// </summary>
        /// <param name="device"></param>
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
        private static DeviceDTO ConvertDevice(DeviceDTO device, AssetTypeDTO newAssetType = null, string newSerialNumber = null)
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
        private static DeviceDTO ConvertLaptopOrDesktop(DeviceDTO desktop, string newRam = null, string newMAC = null, AssetTypeDTO newAssetType = null, string newSerialNumber = null)
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
