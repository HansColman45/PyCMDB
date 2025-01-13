using CMDB.Domain.Entities;

namespace CMDB.Testing.Builders.EntityBuilders
{
    public class SubscriptionBuilder : GenericBogusEntityBuilder<Subscription> 
    {
        public SubscriptionBuilder()
        {
            SetDefaultRules((f, s) =>
            {
                s.PhoneNumber = f.Person.Phone;
            });
        }
    }
}
