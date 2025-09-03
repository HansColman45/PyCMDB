using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Domain.Entities;
using CMDB.Testing.Helpers;
using CMDB.UI.Specflow.Abilities.Data;

namespace CMDB.UI.Specflow.Questions.Permissions
{
    public class CreateThePermission : Question<Task<Permission>>
    {
        public override async Task<Permission> PerformAs(IPerformer actor)
        {
            var dataContext= actor.GetAbility<DataContext>();
            return await PermissionHelper.CreateSimplePermission(dataContext.DBcontext, dataContext.Admin);
        }
    }
}
