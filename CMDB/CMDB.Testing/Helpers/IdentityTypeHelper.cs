using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers
{
    public class IdentityTypeHelper
    {
        public static async Task<IdentityType> CreateSimpleIdentityType(CMDBContext context, Admin admin,bool active = true)
        {
            IdentityType identityType = new IdentityTypeBuilder()
                .With(x => x.LastModifiedAdminId, admin.AdminId)
                .Build();
            context.Types.Add(identityType);

            identityType.Logs.Add(new LogBuilder()
                .With(x => x.Type, identityType)
                .With(x => x.LogText, $"The IdentityType with type: {identityType.Type} and description: {identityType.Description} is created by Automation in table identitytype")
                .Build()
            );

            await context.SaveChangesAsync();
            if (!active)
            {
                identityType.active = 0;
                await context.SaveChangesAsync();
            }
            return identityType;
        }
        public static async Task Delete(CMDBContext context, IdentityType identityType)
        {
            context.RemoveRange(identityType.Logs);
            context.Remove<IdentityType>(identityType);
            await context.SaveChangesAsync();
        }
    }
}
