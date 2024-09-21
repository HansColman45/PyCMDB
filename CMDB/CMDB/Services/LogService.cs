using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Infrastructure;
using CMDB.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMDB.Services
{
    public class LogService : CMDBServices
    {
        public LogService() : base()
        {
        }
        #region Admin stuff
        public async Task<bool> HasAdminAccess(int adminId, string site, string action)
        {
            BaseUrl = _url + $"api/Admin/HasAdminAccess";
            _Client.SetBearerToken(TokenStore.Token);
            HasAdminAccessRequest request = new()
            {
                AdminId = adminId,
                Site = site,
                Action = action
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
