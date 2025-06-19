using CMDB.Domain.CustomExeptions;
using CMDB.Domain.DTOs;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Domain.Responses;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CMDB.Services
{
    /// <summary>
    /// General service
    /// </summary>
    public class CMDBServices
    {
        /// <summary>
        /// The URL of the API
        /// </summary>
        protected string Url { get; set; }
        /// <summary>
        /// The <see cref="HttpClient"/>
        /// </summary>
        protected HttpClient _Client;
        /// <summary>
        /// The BaseUrl
        /// </summary>
        protected string BaseUrl { get; set; }
        /// <summary>
        /// The logger for this service
        /// </summary>
        protected readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// The Admin we are working with
        /// </summary>
        /// <returns></returns>
        public async Task<Admin> Admin()
        {
            var admin = new Admin();
            BaseUrl = Url + $"api/Admin/{TokenStore.AdminId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                admin = await response.Content.ReadAsJsonAsync<Admin>();
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ReAuthenticate();
                response = await _Client.GetAsync(BaseUrl);
                admin = await response.Content.ReadAsJsonAsync<Admin>();
            }
            return admin;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public CMDBServices()
        {
            string baseUrl = Appsettings.BaseUrl;
            if(string.IsNullOrEmpty(baseUrl))
            {
                Url = "https://localhost:7055/";
            }
            else
                Url = baseUrl;
            log.Info($"Using base URL: {Url}");
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                    //return sslPolicyErrors == System.Net.Security.SslPolicyErrors.None;
                }
            };
            _Client = new HttpClient(clientHandler);
        }
        #region generic app things
        /// <summary>
        /// The format for the dates in the logs
        /// </summary>
        public string LogDateFormat
        {
            get
            {
                string format;
                ConfigurationRequest request = new()
                {
                    Code = "General",
                    SubCode = "LogDateFormat"
                }; 
                BaseUrl = Url + $"api/Configuration";
                var response = _Client.PostAsJsonAsync(BaseUrl,request).Result;
                _Client.SetBearerToken(TokenStore.Token);
                if (response.IsSuccessStatusCode)
                {
                    var config = response.Content.ReadAsJsonAsync<Domain.Entities.Configuration>().Result;
                    format = config.CFN_Tekst;
                }
                else
                    format = "dd/MM/yyyy";
                return format;
            }
        }
        /// <summary>
        /// The format for all the other dates in the app
        /// </summary>
        public string DateFormat
        {
            get
            {
                string format;
                ConfigurationRequest request = new()
                {
                    Code = "General",
                    SubCode = "DateFormat"
                };
                BaseUrl = Url + $"api/Configuration";
                var response = _Client.PostAsJsonAsync(BaseUrl, request).Result;
                if (response.IsSuccessStatusCode)
                {
                    var config = response.Content.ReadAsJsonAsync<Domain.Entities.Configuration>().Result;
                    format = config.CFN_Tekst;
                }
                else
                    format = "dd/MM/yyyy";
                return format;
            }
        }
        /// <summary>
        /// The company name we have build the app for
        /// </summary>
        public string Company
        {
            get
            {
                string format;
                ConfigurationRequest request = new()
                {
                    Code = "General",
                    SubCode = "Company"
                };
                BaseUrl = Url + $"api/Configuration";
                _Client.SetBearerToken(TokenStore.Token);
                var response = _Client.PostAsJsonAsync(BaseUrl, request).Result;
                if (response.IsSuccessStatusCode)
                {
                    var config = response.Content.ReadAsJsonAsync<Domain.Entities.Configuration>().Result;
                    format = config.CFN_Tekst;
                }
                else
                    format = "";
                return format;
            }
        }
        #endregion
        /// <summary>
        /// This method will reauthenticate the user
        /// </summary>
        /// <returns></returns>
        public async Task ReAuthenticate()
        {
            var token = await Login(TokenStore.UserName, TokenStore.Password);
            if(token is not null)
            {
                _Client.SetBearerToken(token);
            }
        }
        /// <summary>
        /// This will log the Admin in
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public async Task<string> Login(string userID, string pwd)
        {
            AuthenticateRequest request = new()
            {
                Username = userID,
                Password = pwd
            };
            BaseUrl = Url + "api/Admin/Login";
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (response.IsSuccessStatusCode)
            {
                AuthenticateResponse authenticateResponse = await response.Content.ReadAsJsonAsync<AuthenticateResponse>();
                TokenStore.AdminId = authenticateResponse.Id;
                TokenStore.Token = authenticateResponse.Token;
                TokenStore.UserName = userID;
                TokenStore.Password = pwd;
                return TokenStore.Token;
            }
            else if(response.StatusCode == HttpStatusCode.BadRequest)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
            else
                return null;
        }
        #region generic menu
        /// <summary>
        /// This method will get the first level of the menu
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<Menu>> ListFirstMenuLevel()
        {
            BaseUrl = Url + "api/Menu/FirstLevel";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<Menu>>();
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized) 
            { 
                await ReAuthenticate();
                response = await _Client.GetAsync(BaseUrl);
                return await response.Content.ReadAsJsonAsync<List<Menu>>();
            }
            else
                return new List<Menu>();
        }
        /// <summary>
        /// This method will get the second level of the menu
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public async Task<ICollection<Menu>> ListSecondMenuLevel(int menuID)
        {
            BaseUrl = Url + $"api/Menu/SecondLevel/{menuID}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var menus = await response.Content.ReadAsJsonAsync<List<Menu>>();
                return menus;
            }
            else
                return new List<Menu>();
        }
        /// <summary>
        /// This method will get the personal menu for the admin that is logged in
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public async Task<ICollection<Menu>> ListPersonalMenu(int menuID)
        {
            BaseUrl = Url + $"api/Menu/PersonalMenu/{menuID}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var menus = await response.Content.ReadAsJsonAsync<List<Menu>>();
                return menus;
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ReAuthenticate();
                response = await _Client.GetAsync(BaseUrl);
                var menus = await response.Content.ReadAsJsonAsync<List<Menu>>();
                return menus;
            }
            else
                return new List<Menu>();
        }
        #endregion
        /// <summary>
        /// This method will get the asset types for a specific category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListAssetTypes(string category)
        {
            List<SelectListItem> assettypes = new();
            BaseUrl = Url + $"api/AssetType/GetByCategory/{category}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode) 
            {
                var types = await response.Content.ReadAsJsonAsync<List<AssetTypeDTO>>();
                foreach (var type in types)
                {
                    assettypes.Add(new(type.ToString(), type.TypeID.ToString()));
                }
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ReAuthenticate();
                response = await _Client.GetAsync(BaseUrl);
                var types = await response.Content.ReadAsJsonAsync<List<AssetTypeDTO>>();
                foreach (var type in types)
                {
                    assettypes.Add(new(type.ToString(), type.TypeID.ToString()));
                }
            }
            else
                throw new NotAValidSuccessCode(Url, response.StatusCode);
            return assettypes;
        }
        #region Admin stuff
        /// <summary>
        /// This function will check if the admin has access to a specific site and action
        /// </summary>
        /// <param name="adminId">The adminId</param>
        /// <param name="site">The part of the site requestion action to</param>
        /// <param name="action">The action we are performing</param>
        /// <returns>bool</returns>
        public async Task<bool> HasAdminAccess(int adminId, string site, string action)
        {
            BaseUrl = Url + $"api/Admin/HasAdminAccess";
            _Client.SetBearerToken(TokenStore.Token);
            var perm = EnumExtensions.ParseEnum<CMDB.Domain.Requests.Permission>(action);
            HasAdminAccessRequest request = new()
            {
                AdminId = adminId,
                Site = site,
                Permission = perm
            };
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<bool>();
            else
                return false;
        }
        #endregion
    }
}
