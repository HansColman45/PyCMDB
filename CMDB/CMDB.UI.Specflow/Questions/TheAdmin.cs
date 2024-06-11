using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions
{
    internal class TheAdmin : Question<Admin>
    {
        public override Admin PerformAs(IPerformer actor)
        {
            return new();
        }
        public static async Task<Admin> CreateNewAdminAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await context.CreateNewAdmin();
        }
    }
}
