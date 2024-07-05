using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Domain.Entities;
using CMDB.Testing.Helpers.Devices;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.DataContextAnswers
{
    /// <summary>
    /// This class is used to create a monitor
    /// </summary>
    public class CreateTheMonitor : Question<Task<CMDB.Domain.Entities.Screen>>
    {
        public override async Task<Screen> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await ScreenHelper.CreateScreen(context.context, context.Admin);
        }
    }
    /// <summary>
    /// This class is used to create an inactive monitor
    /// </summary>
    public class CreateTheInactiveMonitor : Question<Task<CMDB.Domain.Entities.Screen>>
    {
        public override async Task<Screen> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await ScreenHelper.CreateScreen(context.context, context.Admin);
        }
    }
}
