using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers
{
    public class SubscriptionHelper
    {
        public static async Task<Subscription> CreateSimpleSubscription(CMDBContext context, SubscriptionType subscriptionType, Admin admin, bool active = true)
        {

            Subscription subscription = new SubscriptionBuilder()
                .With(x => x.SubsctiptionTypeId, subscriptionType.Id)
                .With(x => x.active,1)
                .With(x => x.AssetCategoryId,subscriptionType.AssetCategoryId)
                .With(x => x.LastModifiedAdminId, admin.AccountId)
                .Build();
            context.Subscriptions.Add(subscription);

            subscription.Logs.Add(new LogBuilder()
                .With(x => x.SubsriptionId, subscription.SubscriptionId)
                .With(x => x.LogText, $"The subscription with: {subscriptionType.Category.Category} and type {subscriptionType} on {subscription.PhoneNumber} is created by {admin.Account.UserID} in table subscription")
                .Build()
                );

            await context.SaveChangesAsync();
            if (!active) 
            {
                subscription.active = 0;
                await context.SaveChangesAsync();
            }
            return subscription;
        }
        public static async Task Delete(CMDBContext context, Subscription subscription)
        {
            context.RemoveRange(subscription.Logs);
            context.Remove(subscription);
            await context.SaveChangesAsync();
        }
    }
}
