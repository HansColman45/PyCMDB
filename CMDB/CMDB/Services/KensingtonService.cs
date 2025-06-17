using CMDB.Domain.CustomExeptions;
using CMDB.Domain.DTOs;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDB.Services
{
    /// <summary>
    /// KensingtonService is used to manage the Kensington keys
    /// </summary>
    public class KensingtonService : CMDBServices
    {
        /// <summary>
        /// This will return a list of all Kensington keys
        /// </summary>
        /// <returns>List of <see cref="KensingtonDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<KensingtonDTO>> ListAll()
        {
            BaseUrl = Url + $"api/Kensington/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<KensingtonDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return a list of all Kensington keys with a search string
        /// </summary>
        /// <param name="search"></param>
        /// <returns>List of <see cref="KensingtonDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<KensingtonDTO>> Search(string search)
        {
            BaseUrl = Url + $"api/Kensington/GetAll/{search}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<KensingtonDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return the AssetType with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="AssetTypeDTO"/></returns>
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
        /// This will create a new Kensington key
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="hasLock"></param>
        /// <param name="amountOfKeys"></param>
        /// <param name="assetType"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Create(string serialNumber, bool hasLock, int amountOfKeys, AssetTypeDTO assetType)
        {
            KensingtonDTO kensington = new()
            {
                SerialNumber = serialNumber,
                HasLock = hasLock,
                AmountOfKeys = amountOfKeys,
                Type = assetType
            };
            BaseUrl = Url + $"api/Kensington";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, kensington);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return a Kensington key with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="KensingtonDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<KensingtonDTO> GetByID(int id)
        {
            BaseUrl = Url + $"api/Kensington/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<KensingtonDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will update a Kensington key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Update(KensingtonDTO key)
        {
            BaseUrl = Url + $"api/Kensington";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, key);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will deactivate a Kensington key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Deactivate(KensingtonDTO key, string reason)
        {
            BaseUrl = Url + $"api/Kensington/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl,key);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will activate a Kensington key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Activate(KensingtonDTO key)
        {
            BaseUrl = Url + $"api/Kensington/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, key);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will return a list of all free devices
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListFreeDevices()
        {
            List<SelectListItem> devices = new();
            BaseUrl = Url + $"api/Device/AllFreeDevices/Kensington";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var devicesDTO = await response.Content.ReadAsJsonAsync<List<DeviceDTO>>();
                foreach (var device in devicesDTO)
                {
                    devices.Add(new SelectListItem($"{device.Category.Category} SerialNumber: {device.SerialNumber} type: {device.AssetType} AssetTag: {device.AssetTag}", device.AssetTag));
                }
            }
            return devices;
        }
        /// <summary>
        /// This will retunrn a device with the given asset tag
        /// </summary>
        /// <param name="assetTag"></param>
        /// <returns><see cref="DeviceDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<DeviceDTO> GetDeviceByAssetTag(string assetTag)
        {
            BaseUrl = Url + $"api/Device/{assetTag}";
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
        /// This will assign a Kensington key to a device
        /// </summary>
        /// <param name="key"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task AssignKey2Device(KensingtonDTO key, DeviceDTO device)
        {
            key.Device = device;
            key.AssetTag = device.AssetTag;
            BaseUrl = Url + $"api/Kensington/AssignDevice";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, key);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This will release a Kensington key from a device
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task ReleaseDevice(KensingtonDTO key)
        {
            BaseUrl = Url + $"api/Kensington/ReleaseDevice";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, key);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
    }
}
