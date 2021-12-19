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
        public static Identity CreateSimpleIdentity(CMDBContext context)
        {
            var language = context.Languages.Where(x => x.Code == "NL").FirstOrDefault();
            var identype = context.IdentityTypes.Where(x => x.Type == "Werknemer").FirstOrDefault();

            var identity = new IdentityBuilder()
                .With(x => x.Language, language)
                .With(x => x.Type, identype)
                .Build();

            identity.Logs.Add(new LogBuilder()
                .With(x => x.Identity, identity)
                .With(x => x.LogText, $"The Identity width name: {identity.Name} is created by Automation in table identity")
                .Build()
            );

            context.Identities.Add(identity);
            context.SaveChanges();

            return identity;
        }
    }
}
