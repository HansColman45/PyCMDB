using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Testing.Helpers;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.DataContextAnswers
{
    /// <summary>
    /// This class is used to create an identity
    /// </summary>
    public class CreateTheIdentity : Question<Task<CMDB.Domain.Entities.Identity>>
    {
        public override async Task<CMDB.Domain.Entities.Identity> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await IdentityHelper.CreateSimpleIdentity(context.context, context.Admin);
        }
    }
    /// <summary>
    /// This class is used to create an inactive identity
    /// </summary>
    public class CreateTheInactiveIdentity : Question<Task<CMDB.Domain.Entities.Identity>>
    {
        public override async Task<CMDB.Domain.Entities.Identity> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await IdentityHelper.CreateSimpleIdentity(context.context, context.Admin,false);
        }
    }
}
