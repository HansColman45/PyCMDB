using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class IdentityTypeService : LogService
    {
        public IdentityTypeService() : base()
        {
        }
        public async Task<ICollection<TypeDTO>> ListAll()
        {
            BaseUrl = _url + $"api/IdentityType/GetAll";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<TypeDTO>>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<ICollection<TypeDTO>> ListAll(string searchString)
        {
            BaseUrl = _url + $"api/IdentityType/GetAll/{searchString}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<List<TypeDTO>>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<TypeDTO> GetByID(int id)
        {
            BaseUrl = _url + $"api/IdentityType/{id}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<TypeDTO>();
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task Create(TypeDTO identityType)
        {
            BaseUrl = _url + $"api/IdentityType";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, identityType);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsJsonAsync<TypeDTO>();
            }
            else
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task Update(TypeDTO identityType, string Type, string Description)
        {
            BaseUrl = _url + $"api/IdentityType";
            _Client.SetBearerToken(TokenStore.Token);
            identityType.Type = Type;
            identityType.Description = Description;
            var response = await _Client.PutAsJsonAsync(BaseUrl, identityType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task Deactivate(TypeDTO identityType, string reason)
        {
            BaseUrl = _url + $"api/IdentityType/{reason}";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.DeleteAsJsonAsync(BaseUrl, identityType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task Activate(TypeDTO identityType)
        {
            BaseUrl = _url + $"api/IdentityType/Activate";
            _Client.SetBearerToken(TokenStore.Token);
            var response = await _Client.PostAsJsonAsync(BaseUrl, identityType);
            if (!response.IsSuccessStatusCode)
                throw new NotAValidSuccessCode(BaseUrl, response.StatusCode);
        }
        public async Task<bool> IsExisting(TypeDTO identityType, string Type = "", string Description = "")
        {
            bool result = false;
            identityType.Type = Type == "" ? identityType.Type : Type;
            identityType.Description = Description == "" ? identityType.Description : Description;
            BaseUrl = _url + $"api/IdentityType/IsTypeExisting";
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
