using CMDB.API.Models;
using CMDB.Domain.CustomExeptions;
using CMDB.Infrastructure;
using CMDB.Util;
using System.Collections.Generic;
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
    }
}
