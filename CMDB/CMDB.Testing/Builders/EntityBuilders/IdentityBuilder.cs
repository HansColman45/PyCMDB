using CMDB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Builders.EntityBuilders
{
    public class IdentityBuilder : GenericBogusEntityBuilder<Identity>
    {
        public IdentityBuilder()
        {
            SetDefaultRules((f, i) =>
            {
                i.FirstName = f.Person.FirstName;
                i.LastName = f.Person.LastName;
                i.UserID = f.Person.UserName;
                i.Company = f.Company.CompanyName();
                i.EMail = $"{i.FirstName}.{i.LastName}@CMDB.be";
                i.Type = new IdentityTypeBuilder().Build();
            });
        }
    }
}
