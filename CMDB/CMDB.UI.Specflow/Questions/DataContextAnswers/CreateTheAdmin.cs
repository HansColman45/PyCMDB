using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.DataContextAnswers
{
    public class CreateTheAdmin : Question<Task<Domain.Entities.Admin>>
    {
        public override async Task<Domain.Entities.Admin> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await context.CreateNewCMDBAdmin();
        }
    }
}
