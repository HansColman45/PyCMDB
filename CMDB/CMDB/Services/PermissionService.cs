using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Infrastructure;
using CMDB.Util;
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
    }
}
