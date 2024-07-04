using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.DataContextAnswers
{
    public class GetAllRAMInfo : Question<List<RAM>>
    {
        public override List<RAM> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return context.context.RAMs.ToList();
        }
    }
}
