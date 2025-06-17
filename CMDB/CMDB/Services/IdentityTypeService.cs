using CMDB.Domain.CustomExeptions;
using CMDB.Domain.DTOs;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDB.Services
{
    /// <summary>
    /// IdentityTypeService is used to manage the identity types
    /// </summary>
    public class IdentityTypeService : CMDBServices
    {
        /// <summary>
        /// Constructor for the IdentityTypeService
        /// </summary>
        public IdentityTypeService() : base()
        {
        }
        /// <summary>
        /// This willr eturn a list of all IdentityTypes
        /// </summary>
        /// <returns>List of <see cref="TypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<ICollection<TypeDTO>> ListAll()
        {
            BaseUrl = Url + $"api/IdentityType/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<TypeDTO>>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This will return a list of all IdentityTypes with a search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>List of <see cref="TypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<ICollection<TypeDTO>> ListAll(string searchString)
        {
            BaseUrl = Url + $"api/IdentityType/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<TypeDTO>>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This will return the IdentityType with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="TypeDTO"/></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task<TypeDTO> GetByID(int id)
        {
            BaseUrl = Url + $"api/IdentityType/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<TypeDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This will create a new Type
        /// </summary>
        /// <param name="identityType"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Create(TypeDTO identityType)
        {
            BaseUrl = Url + $"api/IdentityType";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, identityType);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsJsonAsync<TypeDTO>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This will update the Type with the given ID
        /// </summary>
        /// <param name="identityType"></param>
        /// <param name="Type"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Update(TypeDTO identityType, string Type, string Description)
        {
            BaseUrl = Url + $"api/IdentityType";
            _Client.SetBearerToken(TokenStore.Token);
            identityType.Type = Type;
            identityType.Description = Description;
            var response = await _Client.PutAsJsonAsync(BaseUrl, identityType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This will deactivate the Type with the given ID
        /// </summary>
        /// <param name="identityType"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Deactivate(TypeDTO identityType, string reason)
        {
            BaseUrl = Url + $"api/IdentityType/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, identityType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This will activate the Type with the given ID
        /// </summary>
        /// <param name="identityType"></param>
        /// <returns></returns>
        /// <exception cref="NotAValidSuccessCode"></exception>
        public async Task Activate(TypeDTO identityType)
        {
            BaseUrl = Url + $"api/IdentityType/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, identityType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        /// <summary>
        /// This will check if the Type with the given ID exists
        /// </summary>
        /// <param name="identityType"></param>
        /// <param name="Type"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public async Task<bool> IsExisting(TypeDTO identityType, string Type = "", string Description = "")
        {
            bool result = false;
            identityType.Type = Type == "" ? identityType.Type : Type;
            identityType.Description = Description == "" ? identityType.Description : Description;
            BaseUrl = Url + $"api/IdentityType/IsExisting";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, identityType);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsJsonAsync<bool>();
            }
            return result;
        }
    }
}
