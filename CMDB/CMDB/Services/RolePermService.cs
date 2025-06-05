using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Services
{
    /// <summary>
    /// The service for managing role permissions.
    /// </summary>
    public class RolePermService : CMDBServices
    {
        /// <summary>
        /// This function returns a list of all role permissions.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<RolePermissionDTO>> ListAll()
        {
            BaseUrl = Url + $"api/RolePermission/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<RolePermissionDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function returns a list of all role permissions.
        /// </summary>
        /// <param name="searchStr">The search string to filter the role permissions.</param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<List<RolePermissionDTO>> ListAll(string searchStr)
        {
            BaseUrl = Url + $"api/RolePermission/GetAll/{searchStr}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<List<RolePermissionDTO>>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// Retrieves a role permission by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the role permission to retrieve. Must be a positive integer.</param>
        /// <returns>A <see cref="RolePermissionDTO"/> object representing the role permission associated with the specified
        /// identifier.</returns>
        /// <exception cref="NotAValidSuccessCode">Thrown if the HTTP response does not indicate a successful status code.</exception>
        public async Task<RolePermissionDTO> GetById(int id)
        {
            BaseUrl = Url + $"api/RolePermission/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<RolePermissionDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// Retrieves a list of all available menus and converts them into selectable items.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="SelectListItem"/> objects representing the available menus. Each item
        /// contains the menu label as the text and the menu ID as the value.</returns>
        /// <exception cref="NotAValidSuccessCode">Thrown if the HTTP response does not indicate a successful status code.</exception>
        public async Task<List<SelectListItem>> GetAllMenus()
        {
            List<SelectListItem> types = new();
            BaseUrl = Url + $"api/Menu/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var menus = await response.Content.ReadAsJsonAsync<List<Menu>>();
                foreach (var menu in menus.Where(x => x.URL =="#"))
                {
                    types.Add(new SelectListItem(menu.Label, menu.MenuId.ToString()));
                }
                return types;
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// Retrieves a list of all available permissions and formats them as selectable items.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see
        /// cref="SelectListItem"/> objects, where the <see cref="SelectListItem.Text"/> property  represents the
        /// permission name and the <see cref="SelectListItem.Value"/> property represents  the permission ID.</returns>
        /// <exception cref="NotAValidSuccessCode">Thrown if the HTTP response does not indicate a successful status code.</exception>
        public async Task<List<SelectListItem>> GetAllPermissions()
        {
            List<SelectListItem> types = new();
            BaseUrl = Url + $"api/Permission/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var permissions = await response.Content.ReadAsJsonAsync<List<PermissionDTO>>();
                foreach (var permission in permissions)
                {
                    types.Add(new SelectListItem($"{permission.Right}", permission.Id.ToString()));
                }
                return types;
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// List all levels
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListAllLevels()
        {
            List<SelectListItem> Levels = new();
            for (int i = 0; i <= 9; i++)
            {
                Levels.Add(new SelectListItem(i.ToString(), i.ToString()));
            }
            return Levels;
        }
        /// <summary>
        /// Retrieves a menu by its unique identifier.
        /// </summary>
        /// <remarks>This method sends an HTTP GET request to retrieve all menus from the API endpoint 
        /// and filters the result to find the menu matching the specified identifier. Ensure that a valid bearer token
        /// is set in the client before calling this method.</remarks>
        /// <param name="menuid">The unique identifier of the menu to retrieve.</param>
        /// <returns>A <see cref="Menu"/> object representing the menu with the specified identifier,  or <see langword="null"/>
        /// if no menu with the given identifier exists.</returns>
        /// <exception cref="NotAValidSuccessCode">Thrown if the HTTP response does not indicate a successful status code.</exception>
        public async Task<Menu> GetMenuById(int menuid)
        {
            BaseUrl = Url + $"api/Menu/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var menus = await response.Content.ReadAsJsonAsync<List<Menu>>();
                return menus.FirstOrDefault(x => x.MenuId == menuid);
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// Retrieves the permission details for the specified permission ID.
        /// </summary>
        /// <remarks>This method sends an HTTP GET request to the API endpoint to fetch the permission
        /// details. Ensure that a valid bearer token is set in the client before calling this method.</remarks>
        /// <param name="permissionId">The unique identifier of the permission to retrieve. Must be a positive integer.</param>
        /// <returns>A <see cref="PermissionDTO"/> object containing the details of the requested permission.</returns>
        /// <exception cref="NotAValidSuccessCode">Thrown if the server response does not indicate a successful status code.</exception>
        public async Task<PermissionDTO> GetPermissionById(int permissionId)
        {
            BaseUrl = Url + $"api/Permission/{permissionId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<PermissionDTO>();
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// Creates a new role permission by sending the specified data to the server.
        /// </summary>
        /// <remarks>This method sends a POST request to the server's RolePermission API endpoint. Ensure
        /// that the  <see cref="TokenStore.Token"/> is valid and has the necessary permissions to create role
        /// permissions.</remarks>
        /// <param name="rolePermission">The data representing the role permission to be created. This must include all required fields as defined by
        /// the server's API.</param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode">Thrown if the server responds with a non-success HTTP status code.</exception>
        public async Task Create(RolePermissionDTO rolePermission)
        {
            BaseUrl = Url + $"api/RolePermission";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, rolePermission);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// Updates an existing role permission with the specified data.
        /// </summary>
        /// <param name="roleper">The updated <see cref="RolePermissionDTO"/></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Edit(RolePermissionDTO roleper)
        {
            BaseUrl = Url + $"api/RolePermission";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PutAsJsonAsync(BaseUrl, roleper);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
    }
}
