using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Testing.Helpers.Devices;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.DataContextAnswers
{
    /// <summary>
    /// This class is used to create a token
    /// </summary>
    public class CreateTheToken : Question<Task<CMDB.Domain.Entities.Token>>
    {
        public override async Task<Domain.Entities.Token> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await TokenHelper.CreateNewToken(context.context, context.Admin);
        }
    }
    /// <summary>
    /// This class is used to create an inactive token
    /// </summary>
    public class CreateTheInactiveToken : Question<Task<CMDB.Domain.Entities.Token>>
    {
        public override async Task<Domain.Entities.Token> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await TokenHelper.CreateNewToken(context.context, context.Admin,false);
        }
    }
}
