using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Testing.Helpers.Devices;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.Mobile
{
    /// <summary>
    /// This class is used to create a mobile
    /// </summary>
    public class CreateTheMobile : Question<Task<Domain.Entities.Mobile>>
    {
        public override async Task<Domain.Entities.Mobile> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await MobileHelper.CreateSimpleMobile(context.context, context.Admin);
        }
    }
    /// <summary>
    /// This class is used to create an inactive mobile
    /// </summary>
    public class CreateTheInactiveMobile : Question<Task<Domain.Entities.Mobile>>
    {
        public override async Task<Domain.Entities.Mobile> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await MobileHelper.CreateSimpleMobile(context.context, context.Admin,false);
        }
    }
}
