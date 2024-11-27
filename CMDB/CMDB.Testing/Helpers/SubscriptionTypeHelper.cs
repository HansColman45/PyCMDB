using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using System;
using System.Threading.Tasks;
using Admin = CMDB.Domain.Entities.Admin;

namespace CMDB.Testing.Helpers
{
    public class SubscriptionTypeHelper
    {
        public static async Task<SubscriptionType> CreateSimpleSubscriptionType(CMDBContext context, AssetCategory category, Admin admin,bool active = true)
        {
            try
            {
                SubscriptionType subscriptionType = new SubscriptionTypeBuiler()
                        .With(x => x.AssetCategoryId, category.Id)
                        .With(x => x.LastModifiedAdminId, admin.AdminId)
                        .Build();
                context.SubscriptionTypes.Add(subscriptionType);

                subscriptionType.Logs.Add(new LogBuilder()
                    .With(x => x.SubscriptionType, subscriptionType)
                    .With(x => x.LogText, $"The {category.Category} with {subscriptionType.Provider} and {subscriptionType.Type} is created by Automation in table subscriptiontype")
                    .Build());
                await context.SaveChangesAsync();

                if (!active)
                {
                    subscriptionType.active = 0;
                    await context.SaveChangesAsync();
                }
                return subscriptionType;
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
