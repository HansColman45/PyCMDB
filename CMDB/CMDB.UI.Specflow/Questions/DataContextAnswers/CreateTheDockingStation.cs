using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Testing.Helpers.Devices;
using CMDB.UI.Specflow.Abilities.Data;
using System;

namespace CMDB.UI.Specflow.Questions.DataContextAnswers
{
    /// <summary>
    /// This class is used to create a docking station
    /// </summary>
    public class CreateTheDockingStation : Question<Task<CMDB.Domain.Entities.Docking>>
    {
        public override async Task<Domain.Entities.Docking> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await DockingHelpers.CreateSimpleDocking(context.context, context.Admin);
        }
    }
    /// <summary>
    /// This class is used to create an inactive docking station
    /// </summary>
    public class CreateTheIncativeDockingStation : Question<Task<CMDB.Domain.Entities.Docking>>
    {
        public override async Task<Domain.Entities.Docking> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await DockingHelpers.CreateSimpleDocking(context.context, context.Admin, false);
        }
    }
}
