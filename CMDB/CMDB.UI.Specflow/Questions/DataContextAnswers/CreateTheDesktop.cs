using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Testing.Helpers.Devices;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.DataContextAnswers
{
    /// <summary>
    /// This class is used to create a desktop
    /// </summary>
    public class CreateTheDesktop : Question<Task<CMDB.Domain.Entities.Desktop>>
    {
        public override async Task<Domain.Entities.Desktop> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await DesktopHelper.CreateSimpleDesktop(context.context, context.Admin);
        }
    }
    /// <summary>
    /// This class is used to create an inactive desktop
    /// </summary>
    public class CreateTheInactiveDesktop : Question<Task<CMDB.Domain.Entities.Desktop>>
    {
        public override async Task<Domain.Entities.Desktop> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await DesktopHelper.CreateSimpleDesktop(context.context, context.Admin, false);
        }
    }
}
