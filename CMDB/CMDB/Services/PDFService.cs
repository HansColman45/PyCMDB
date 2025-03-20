using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Infrastructure;
using CMDB.Util;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class PDFService: CMDBServices
    {
        private readonly string path = $"api/PDFGenerator";
        public PDFService()
        {
        }
        /// <summary>
        /// This funstion will set the info of the User
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <param name="ITEmployee">Name of the IT Employee</param>
        /// <param name="Singer">Name of the signer</param>
        /// <param name="FirstName">Firstname</param>
        /// <param name="LastName">Lastname</param>
        /// <param name="Receiver">The name of the reciever</param>
        /// <param name="Language">The language of the reciever</param>
        /// <param name="type">The type of the PDF can be NULL</param>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task SetUserinfo(string UserId, 
            string ITEmployee, 
            string Singer, 
            string FirstName,
            string LastName,
            string Receiver, 
            string Language, 
            string type = null)
        {
            BaseUrl = _url + path + "/AddUserInfo";
            PDFInformation info = new()
            {
                ITEmployee = ITEmployee,
                Language = Language,
                FirstName = FirstName, 
                LastName = LastName,
                Type = type,
                Singer = Singer,
                Receiver = Receiver,
                UserID = UserId,
            };
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, info);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task SetAccontInfo(IdenAccountDTO account)
        {
            BaseUrl = _url + "api/PDFGenerator/AddAccountInfo";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl,account);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task SetDeviceInfo(DeviceDTO device)
        {
            BaseUrl = _url + path + "/AddAssetInfo";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task SetMobileInfo(MobileDTO mobile)
        {
            BaseUrl = _url + path + "/AddMobileInfo";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, mobile);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task SetSubscriptionInfo(SubscriptionDTO subscription)
        {
            BaseUrl = _url + path + "/AddSubscriptionInfo";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, subscription);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task SetKeyInfo(KensingtonDTO kensington)
        {
            BaseUrl = _url + path + "/AddKeyInfo";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, kensington);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
        }
        public async Task<string> GenratePDFFile(string entity, int id)
        {
            BaseUrl = _url + path + $"/{entity}/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url,response.StatusCode);
            else
                return await response.Content.ReadAsJsonAsync<string>();
        }
        public async Task<string> GenratePDFFile(string entity, string assetTag)
        {
            BaseUrl = _url + path + $"/{entity}/{assetTag}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(_url, response.StatusCode);
            else
                return await response.Content.ReadAsJsonAsync<string>();
        }
    }
}
