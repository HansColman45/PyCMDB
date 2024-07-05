using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Domain.Entities;
using CMDB.Testing.Helpers;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.DataContextAnswers
{
    /// <summary>
    /// This class is used to create an identity type
    /// </summary>
    public class CreateTheIdentityType : Question<Task<IdentityType>>
    {
        public override async Task<IdentityType> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await IdentityTypeHelper.CreateSimpleIdentityType(context.context, context.Admin);
        }
    }
    /// <summary>
    /// This class is used to create an inactive identity type
    /// </summary>
    public class CreateTheInactiveIdentityType : Question<Task<IdentityType>>
    {
        public override async Task<IdentityType> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await IdentityTypeHelper.CreateSimpleIdentityType(context.context, context.Admin,false);
        }
    }
}
