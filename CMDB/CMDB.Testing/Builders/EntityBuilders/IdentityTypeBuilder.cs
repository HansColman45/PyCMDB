using CMDB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Builders.EntityBuilders
{
    public class IdentityTypeBuilder : GenericBogusEntityBuilder<IdentityType>
    {
        public IdentityTypeBuilder()
        {
            SetDefaultRules((f, it) =>
            {
                it.Type = f.Company.CompanyName();
                it.Description = f.Lorem.Sentence();
            });
        }
    }
}
