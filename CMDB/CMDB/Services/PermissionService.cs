using CMDB.Domain.CustomExeptions;
using CMDB.Domain.DTOs;
using CMDB.Infrastructure;
using CMDB.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDB.Services
{
    /// <summary>
    /// This is the Permission service
    /// </summary>
    public class PermissionService : CMDBServices
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PermissionService() : base()
        {
        }
        /// <summary>
        /// This method returns a list of all identities
        /// </summary>
        /// <returns>List of <see cref="IdentityDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<PermissionDTO>> ListAll()
        {
            BaseUrl = Url + $"api/Permission/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<PermissionDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method returns a list of all identities
        /// </summary>
        /// <returns>List of <see cref="IdentityDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<PermissionDTO>> ListAll(string search)
        {
            BaseUrl = Url + $"api/Permission/GetAll/{search}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<PermissionDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This method returns a permission by id
        /// </summary>
        /// <param name="id">The Id of the perssion</param>
        /// <returns>The <see cref="PermissionDTO"/> or null</returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<PermissionDTO> GetById(int id)
        {
            BaseUrl = Url + $"api/Permission/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<PermissionDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This funstion will get the RolePermiossion info of the given perrsmission id
        /// </summary>
        /// <param name="id">The Id of the permission</param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<RolePermissionDTO>> GetRolePermissionInfo(int id)
        {
            BaseUrl = Url + $"api/Permission/RorePermOverview/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<RolePermissionDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will create a new permission
        /// </summary>
        /// <param name="permission">The Permssion to create</param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task CreatePermission(PermissionDTO permission)
        {
            BaseUrl = Url + $"api/Permission";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, permission);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will update a permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task UpdatePermission(PermissionDTO permission)
        {
            BaseUrl = Url + $"api/Permission";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, permission);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
    }
}
