using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Domain.Entities;
using CMDB.Testing.Helpers;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.Types
{
    /// <summary>
    /// This class is used to create an account type
    /// </summary>
    public class CreateTheAccountType : Question<Task<AccountType>>
    {
        public override async Task<AccountType> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await AccountTypeHelper.CreateSimpleAccountType(context.DBcontext, context.Admin);
        }
    }
    /// <summary>
    /// This class is used to create an inactive account type
    /// </summary>
    public class CreateTheInactiveAccountType : Question<Task<AccountType>>
    {
        public override async Task<AccountType> PerformAs(IPerformer actor)
        {
            var context = actor.GetAbility<DataContext>();
            return await AccountTypeHelper.CreateSimpleAccountType(context.DBcontext, context.Admin,false);
        }
    }
}
