using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Testing.Helpers;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.Identity
{
    /// <summary>
    /// This class is used to create an identity
    /// </summary>
    public class CreateTheIdentity : Question<Task<Domain.Entities.Identity>>
    {
        public override async Task<Domain.Entities.Identity> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await IdentityHelper.CreateSimpleIdentity(context.DBcontext, context.Admin);
        }
    }
    /// <summary>
    /// This class is used to create an inactive identity
    /// </summary>
    public class CreateTheInactiveIdentity : Question<Task<Domain.Entities.Identity>>
    {
        public override async Task<Domain.Entities.Identity> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await IdentityHelper.CreateSimpleIdentity(context.DBcontext, context.Admin,false);
        }
    }
}
