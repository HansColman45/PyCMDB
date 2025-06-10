using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Domain.Entities;
using CMDB.Testing.Helpers;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.DataContextAnswers
{
    public class CreateTheMenu : Question<Task<Menu>>
    {
        public override Task<Menu> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return MenuHelper.CreateSimpleMenu(context.context,context.Admin);
        }
    }
}
