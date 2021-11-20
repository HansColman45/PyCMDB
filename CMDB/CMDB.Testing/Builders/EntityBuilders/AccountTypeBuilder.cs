using CMDB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Builders.EntityBuilders
{
    public class AccountTypeBuilder : GenericBogusEntityBuilder<AccountType>
    {
        public AccountTypeBuilder()
        {
            SetDefaultRules((f, at) =>
            {
                at.Type = f.Commerce.Product();
                at.Description = f.Lorem.Sentence();
            });
        }
    }
}
