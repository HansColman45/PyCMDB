using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Domain.Entities;
using CMDB.Testing.Helpers;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.Keys
{
    class CreateTheKensington : Question<Task<Kensington>>
    {
        public override async Task<Kensington> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            try
            {
                return await KensingtonHelper.CreateKensington(context.context, context.Admin);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    class CreateTheInactiveKensington : Question<Task<Kensington>>
    {
        public override async Task<Kensington> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await KensingtonHelper.CreateKensington(context.context, context.Admin, false);
        }
    }
}
