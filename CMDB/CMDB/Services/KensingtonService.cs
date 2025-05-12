using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using Microsoft.Graph.Models.Security;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class KensingtonService : CMDBServices
    {
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
        public async Task Update(KensingtonDTO key)
        {
            BaseUrl = Url + $"api/Kensington";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, key);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        public async Task Deactivate(KensingtonDTO key, string reason)
        {
            BaseUrl = Url + $"api/Kensington/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl,key);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        public async Task Activate(KensingtonDTO key)
        {
            BaseUrl = Url + $"api/Kensington/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, key);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
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
