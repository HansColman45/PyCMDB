using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Domain.Entities;
using CMDB.Testing.Helpers;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.SubscriptionType
{
    /// <summary>
    /// This class is used to create a subscription type
    /// </summary>
    public class CreateTheSubscriptionType: Question<Task<Domain.Entities.SubscriptionType>>
    {
        public override async Task<Domain.Entities.SubscriptionType> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            AssetCategory assetCategory = context.context.AssetCategories.Where(x => x.Category == "Internet Subscription").First();
            return await SubscriptionTypeHelper.CreateSimpleSubscriptionType(context.context, assetCategory, context.Admin);
        }
    }
    /// <summary>
    /// The class is used to create an inactive subscription type
    /// </summary>
    public class CreateTheInactiveSubscriptionType : Question<Task<Domain.Entities.SubscriptionType>>
    {
        public override async Task<Domain.Entities.SubscriptionType> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            AssetCategory assetCategory = context.context.AssetCategories.Where(x => x.Category == "Internet Subscription").First();
            return await SubscriptionTypeHelper.CreateSimpleSubscriptionType(context.context, assetCategory, context.Admin, false);
        }
    }
}
