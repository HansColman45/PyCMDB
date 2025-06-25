using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using System;

namespace CMDB.Testing.Builders.EntityBuilders
{
    public class AdminBuilder : GenericBogusEntityBuilder<Admin>
    {
        public AdminBuilder()
        {
            SetDefaultRules((f, a) =>
            {
                a.DateSet = DateTime.UtcNow;
                a.Level = 9;
                a.Password = "1234";
            });
        }
        public override Admin Build()
        {
            var user = base.Build();
            user.Password = new PasswordHasher().EncryptPassword(user.Password);
            return user;
        }
    }
}
