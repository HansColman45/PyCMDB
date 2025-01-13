using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers
{
    public class IdentityHelper
    {
        public static async Task<Identity> CreateSimpleIdentity(CMDBContext context, Admin admin, bool active = true)
        {
            var language = context.Languages.Where(x => x.Code == "NL").AsNoTracking().FirstOrDefault();
            var identype = context.Types.OfType<IdentityType>().Where(x => x.Type == "Werknemer").AsNoTracking().FirstOrDefault();

            var identity = new IdentityBuilder()
                .With(x => x.LanguageCode, language.Code)
                .With(x => x.TypeId, identype.TypeId)
                .With(x => x.LastModifiedAdminId, admin.AdminId)
                .With(x => x.active, 1)
                .Build();

            identity.Logs.Add(new LogBuilder()
                .With(x => x.Identity, identity)
                .With(x => x.LogText, $"The Identity width name: {identity.Name} is created by Automation in table identity")
                .Build()
            );

            context.Identities.Add(identity);
            await context.SaveChangesAsync();
            if (!active)
            {
                //For some reason the savechanges above changes the state
                identity.active = 0;
                await context.SaveChangesAsync();
            }
            return identity;
        }
        public static async Task Delete(CMDBContext context, Identity identity)
        {
            context.RemoveRange(identity.Logs);
            context.Remove<Identity>(identity);
            await context.SaveChangesAsync();
        }
    }
}
