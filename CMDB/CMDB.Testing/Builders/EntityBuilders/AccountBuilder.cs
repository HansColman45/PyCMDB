using CMDB.Domain.Entities;

namespace CMDB.Testing.Builders.EntityBuilders
{
    public class AccountBuilder : GenericBogusEntityBuilder<Account>
    {
        public AccountBuilder()
        {
            SetDefaultRules((f, a) =>
            {
                a.UserID = f.Person.UserName;
                a.Type = new AccountTypeBuilder().Build();
                a.Application = new ApplicationBuilder().Build();
            });
        }
    }
}
