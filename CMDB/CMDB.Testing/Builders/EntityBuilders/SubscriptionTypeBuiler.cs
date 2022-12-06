using CMDB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Builders.EntityBuilders
{
    public class SubscriptionTypeBuiler: GenericBogusEntityBuilder<SubscriptionType>
    {
        public SubscriptionTypeBuiler()
        {
            SetDefaultRules((f, t) =>
            {
                t.Provider = f.Commerce.Product();
                t.Type = f.Address.StreetName();
                t.Description = f.Company.CompanyName();
            });
        }
    }
}
