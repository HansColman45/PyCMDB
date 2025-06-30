using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Helpers;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.RolePermissions;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.PermissionActors
{
    public class RolePermissionUpdator : RolePermissionActor
    {
        public RolePermissionUpdator(ScenarioContext scenarioContext, string name = "PermissionUpdator") : base(scenarioContext, name)
        {
        }

        public async Task<RolePerm> CreateRolePerm(Menu menu)
        {
            var context = GetAbility<DataContext>();
            var permission = GetOrCreatePermission("Read");
            return await RolePermissionHelper.CreateDefaultRolePermission(context.context, menu,permission, admin);
        }

        public void DoUpdate(RolePerm rolePerm, string level)
        {
            ExpectedLog = GenericLogLineCreator.UpdateLogLine("Level",$"{rolePerm.Level}", level, admin.Account.UserID,Table);
            var editPage = Perform(new OpenTheEditRolePermissionPage());
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_edit");
            editPage.SelectLevel(level);
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_level_selected");
            editPage.Edit();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_edited");
        }
    }
}
