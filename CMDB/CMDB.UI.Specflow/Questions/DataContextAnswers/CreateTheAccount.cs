using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Testing.Helpers;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.DataContextAnswers
{
    /// <summary>
    /// This class is used to create an active account
    /// </summary>
    public class CreateTheAccount : Question<Task<Domain.Entities.Account>>
    {
        public override async Task<Domain.Entities.Account> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            var account = await AccountHelper.CreateSimpleAccountAsync(context.context, context.Admin);
            return context.GetAccount(account.AccID);
        }
    }
    /// <summary>
    /// This class is used to create an inactive account
    /// </summary>
    public class CreateTheIncativeAccount : Question<Task<CMDB.Domain.Entities.Account>>
    {
        public override async Task<Domain.Entities.Account> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            var account =  await AccountHelper.CreateSimpleAccountAsync(context.context, context.Admin, false);
            return context.GetAccount(account.AccID);
        }
    }
}
