using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System;
using System.Text;

namespace CMDB.Services
{
    public class PermissionService : LogService
    {
        public PermissionService(CMDBContext context) : base(context)
        {
        }

    }
}
