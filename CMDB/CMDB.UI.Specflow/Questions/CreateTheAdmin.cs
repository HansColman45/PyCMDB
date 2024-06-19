using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions
{
    public class CreateTheAdmin : Question<Task<Admin>>
    {
        public override async Task<Admin> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await context.CreateNewAdmin();
        }
    }
}
