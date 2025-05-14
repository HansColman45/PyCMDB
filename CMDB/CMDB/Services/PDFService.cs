using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Infrastructure;
using CMDB.Util;
using System.Threading.Tasks;

namespace CMDB.Services
{
    /// <summary>
    /// This class is used to generate PDF files
    /// </summary>
    public class PDFService: CMDBServices
    {
        private readonly string path = $"api/PDFGenerator";

        /// <summary>
        /// This constructor will set the base url of the PDF service
        /// </summary>
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
            BaseUrl = Url + path + "/AddUserInfo";
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
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will set the info of the account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task SetAccontInfo(IdenAccountDTO account)
        {
            BaseUrl = Url + "api/PDFGenerator/AddAccountInfo";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl,account);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will set the info of the device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task SetDeviceInfo(DeviceDTO device)
        {
            BaseUrl = Url + path + "/AddAssetInfo";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, device);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will set the info of the mobile
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task SetMobileInfo(MobileDTO mobile)
        {
            BaseUrl = Url + path + "/AddMobileInfo";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, mobile);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will set the info of the subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task SetSubscriptionInfo(SubscriptionDTO subscription)
        {
            BaseUrl = Url + path + "/AddSubscriptionInfo";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, subscription);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will set the info of the kensington
        /// </summary>
        /// <param name="kensington"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task SetKeyInfo(KensingtonDTO kensington)
        {
            BaseUrl = Url + path + "/AddKeyInfo";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, kensington);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
        }
        /// <summary>
        /// This function will generate teh PDF file
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<string> GenratePDFFile(string entity, int id)
        {
            BaseUrl = Url + path + $"/{entity}/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url,response.StatusCode);
            else
                return await response.Content.ReadAsJsonAsync<string>();
        }
        /// <summary>
        /// This function will generate the PDF File
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="assetTag"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<string> GenratePDFFile(string entity, string assetTag)
        {
            BaseUrl = Url + path + $"/{entity}/{assetTag}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(Url, response.StatusCode);
            else
                return await response.Content.ReadAsJsonAsync<string>();
        }
    }
}
