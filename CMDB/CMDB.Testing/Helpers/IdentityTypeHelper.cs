using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers
{
    public class IdentityTypeHelper
    {
        public static async Task Delete(CMDBContext context, IdentityType identityType)
        {
            context.RemoveRange(identityType.Logs);
            context.Remove<IdentityType>(identityType);
            await context.SaveChangesAsync();
        }
    }
}
