using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers
{
    public class IdentityHelper
    {
        public static async Task<Identity> CreateSimpleIdentity(CMDBContext context, Admin admin, bool active = true)
        {
            var language = context.Languages.Where(x => x.Code == "NL").FirstOrDefault();
            var identype = context.IdentityTypes.Where(x => x.Type == "Werknemer").FirstOrDefault();

            var identity = new IdentityBuilder()
                .With(x => x.Language, language)
                .With(x => x.Type, identype)
                .With(x => x.LastModifiedAdminId, admin.AdminId)
                .With(x => x.active, active ? 1 : 0)
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
                context.SaveChanges();
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
