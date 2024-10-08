using Azure;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Domain.Responses;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class CMDBServices
    {
        protected string _url => "https://localhost:7055/";
        protected HttpClient _Client;
        protected string BaseUrl { get; set; }
        public async Task<Admin> Admin()
        {
            var admin = new Admin();
            BaseUrl = _url + $"api/Admin/{TokenStore.AdminId}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                admin = await response.Content.ReadAsJsonAsync<Admin>();
            }
            return admin;
        }

        public CMDBServices()
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => {
                    //if (development) return true;
                    return sslPolicyErrors == System.Net.Security.SslPolicyErrors.None;
                }
            };
            _Client = new HttpClient(clientHandler);
        }
        #region generic app things
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
                BaseUrl = _url + $"api/Configuration";
                var response = _Client.PostAsJsonAsync(BaseUrl,request).Result;
                if (response.IsSuccessStatusCode)
                {
                    var config = response.Content.ReadAsJsonAsync<Configuration>().Result;
                    format = config.CFN_Tekst;
                }
                else
                    format = "dd/MM/yyyy";
                return format;
            }
        }
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
                BaseUrl = _url + $"api/Configuration";
                var response = _Client.PostAsJsonAsync(BaseUrl, request).Result;
                if (response.IsSuccessStatusCode)
                {
                    var config = response.Content.ReadAsJsonAsync<Configuration>().Result;
                    format = config.CFN_Tekst;
                }
                else
                    format = "dd/MM/yyyy";
                return format;
            }
        }
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
                BaseUrl = _url + $"api/Configuration";
                var response = _Client.PostAsJsonAsync(BaseUrl, request).Result;
                if (response.IsSuccessStatusCode)
                {
                    var config = response.Content.ReadAsJsonAsync<Configuration>().Result;
                    format = config.CFN_Tekst;
                }
                else
                    format = "";
                return format;
            }
        }
        #endregion

        public async Task<string> Login(string userID, string pwd)
        {
            AuthenticateRequest request = new()
            {
                Username = userID,
                Password = pwd
            };
            BaseUrl = _url + "api/Admin/Login";
            var response = await _Client.PostAsJsonAsync(BaseUrl, request);
            if (response.IsSuccessStatusCode)
            {
                AuthenticateResponse authenticateResponse = await response.Content.ReadAsJsonAsync<AuthenticateResponse>();
                TokenStore.AdminId = authenticateResponse.Id;
                TokenStore.Token = authenticateResponse.Token;
                return TokenStore.Token;
            }
            else
                return null;
        }
        #region generic menu
        public async Task<ICollection<Menu>> ListFirstMenuLevel()
        {
            BaseUrl = _url + "api/Menu/FirstLevel";
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<Menu>>();
            }
            else
                return new List<Menu>();
        }
        public async Task<ICollection<Menu>> ListSecondMenuLevel(int menuID)
        {
            BaseUrl = _url + $"api/Menu/SecondLevel/{menuID}";
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var menus = await response.Content.ReadAsJsonAsync<List<Menu>>();
                return menus;
            }
            else
                return new List<Menu>();
        }
        public async Task<ICollection<Menu>> ListPersonalMenu(string token, int menuID)
        {
            BaseUrl = _url + $"api/Menu/PersonalMenu/{menuID}";
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
        #endregion
        public List<SelectListItem> ListAssetTypes(string category)
        {
            List<SelectListItem> assettypes = new();
            /*var types = _context.AssetTypes.Include(x => x.Category).Where(x => x.Category.Category == category).ToList();
            foreach (var type in types)
            {
                assettypes.Add(new(type.Vendor + " " + type.Type, type.TypeID.ToString()));
            }*/
            return assettypes;
        }
    }
}
