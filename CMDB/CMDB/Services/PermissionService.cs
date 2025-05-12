using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System;
using System.Text;

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

    }
}
