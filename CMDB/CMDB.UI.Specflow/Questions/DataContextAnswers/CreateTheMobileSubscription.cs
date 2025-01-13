using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Testing.Helpers;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.DataContextAnswers
{
    public class CreateTheMobileSubscription : Question<Task<Domain.Entities.Subscription>>
    {
        public override async Task<Domain.Entities.Subscription> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            var assetCategory = context.GetAssetCategory("Mobile Subscription");
            var type = await SubscriptionTypeHelper.CreateSimpleSubscriptionType(context.context, assetCategory,context.Admin);
            return await SubscriptionHelper.CreateSimpleSubscription(context.context,type,context.Admin);
        }
    }
    public class CreateTheInactiveMobileSubscription : Question<Task<Domain.Entities.Subscription>>
    {
        public override async Task<Domain.Entities.Subscription> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            var assetCategory = context.GetAssetCategory("Mobile Subscription");
            var type = await SubscriptionTypeHelper.CreateSimpleSubscriptionType(context.context, assetCategory, context.Admin);
            return await SubscriptionHelper.CreateSimpleSubscription(context.context, type, context.Admin, false);
        }
    }
    public class CreateTheInternetSubscription : Question<Task<Domain.Entities.Subscription>>
    {
        public override async Task<Domain.Entities.Subscription> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            var assetCategory = context.GetAssetCategory("Internet Subscription");
            var type = await SubscriptionTypeHelper.CreateSimpleSubscriptionType(context.context, assetCategory, context.Admin);
            return await SubscriptionHelper.CreateSimpleSubscription(context.context, type, context.Admin);
        }
    }
    public class CreateTheInactiveInternetSubscription : Question<Task<Domain.Entities.Subscription>>
    {
        public override async Task<Domain.Entities.Subscription> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            var assetCategory = context.GetAssetCategory("Internet Subscription");
            var type = await SubscriptionTypeHelper.CreateSimpleSubscriptionType(context.context, assetCategory, context.Admin);
            return await SubscriptionHelper.CreateSimpleSubscription(context.context, type, context.Admin, false);
        }
    }
}
