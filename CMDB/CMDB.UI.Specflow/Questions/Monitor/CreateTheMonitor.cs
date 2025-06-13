using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Domain.Entities;
using CMDB.Testing.Helpers.Devices;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.Monitor
{
    /// <summary>
    /// This class is used to create a monitor
    /// </summary>
    public class CreateTheMonitor : Question<Task<Screen>>
    {
        public override async Task<Screen> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            var screen = await ScreenHelper.CreateScreen(context.context, context.Admin);
            return (Screen)context.GetDevice(screen.AssetTag);
        }
    }
    /// <summary>
    /// This class is used to create an inactive monitor
    /// </summary>
    public class CreateTheInactiveMonitor : Question<Task<Screen>>
    {
        public override async Task<Screen> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            var screen = await ScreenHelper.CreateScreen(context.context, context.Admin, false);
            return (Screen)context.GetDevice(screen.AssetTag);
        }
    }
}
