using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Testing.Helpers.Devices;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.Desktop
{
    /// <summary>
    /// This class is used to create a desktop
    /// </summary>
    public class CreateTheDesktop : Question<Task<Domain.Entities.Desktop>>
    {
        public override async Task<Domain.Entities.Desktop> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            var desktop = await DesktopHelper.CreateSimpleDesktop(context.context, context.Admin);
            return (Domain.Entities.Desktop)context.GetDevice(desktop.AssetTag);
        }
    }
    /// <summary>
    /// This class is used to create an inactive desktop
    /// </summary>
    public class CreateTheInactiveDesktop : Question<Task<Domain.Entities.Desktop>>
    {
        public override async Task<Domain.Entities.Desktop> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            var desktop = await DesktopHelper.CreateSimpleDesktop(context.context, context.Admin, false);
            return (Domain.Entities.Desktop)context.GetDevice(desktop.AssetTag);
        }
    }
}
