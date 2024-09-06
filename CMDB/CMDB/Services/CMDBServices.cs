using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Domain.Responses;
using CMDB.Infrastructure;
using CMDB.Util;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class CMDBServices
    {
        protected string _url = "https://localhost:7055/";
        protected CMDBContext _context;
        protected HttpClient _Client;
        protected string BaseUrl { get; set; }
        public Admin Admin
        {
            get
            {
                BaseUrl = _url + $"api/Admin/{TokenStore.AdminId}";
                _Client.SetBearerToken(TokenStore.Token);
                var response = _Client.GetAsync(BaseUrl).Result;
                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsJsonAsync<Admin>().Result;
                else
                    return null;
            }
            /*set
            {
                if (_context.Admin != null && value != null)
                {
                    if (_context.Admin.AdminId != value.AdminId)
                        _context.Admin = _context.Admins.Where(x => x.AdminId == value.AdminId).SingleOrDefault();
                }
                else if (value is null)
                    _context.Admin = null;
            }*/
        }

        public CMDBServices(CMDBContext context)
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => {
                    //if (development) return true;
                    return sslPolicyErrors == System.Net.Security.SslPolicyErrors.None;
                }
            };
            _Client = new HttpClient(clientHandler);
            _context = context;
        }
        #region generic app things
        public string LogDateFormat
        {
            get
            {
                string format = "dd/MM/yyyy";
                Configuration config = _context.Configurations
                    .Where(x => x.Code == "General" && x.SubCode == "LogDateFormat").SingleOrDefault();
                format = config.CFN_Tekst;
                return format;
            }
        }
        public string DateFormat
        {
            get
            {
                string format = "dd/MM/yyyy";
                Configuration config = _context.Configurations
                    .Where(x => x.Code == "General" && x.SubCode == "DateFormat").SingleOrDefault();
                format = config.CFN_Tekst;
                return format;
            }
        }
        public string Company
        {
            get
            {
                string format = "";
                Configuration config = _context.Configurations
                    .Where(x => x.Code == "General" && x.SubCode == "Company").SingleOrDefault();
                format = config.CFN_Tekst;
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
            BaseUrl = _url + "api/Admin/login";
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
            _Client.SetBearerToken(token);
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
    }
}
