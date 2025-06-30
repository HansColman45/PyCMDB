using CMDB.Infrastructure;
using CMDB.UI.Specflow.Helpers;
using CMDB.UI.Specflow.Questions.RolePermissions;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.PermissionActors
{
    public class RolePermissionCreator : RolePermissionActor
    {
        public RolePermissionCreator(ScenarioContext scenarioContext, string name = "RolePermissionCreator") : base(scenarioContext, name)
        {
        }

        public async Task CreateNewRolePermission(RolePerm rolePerm)
        {
            var menu = await GetOrCreateMenu(rolePerm.Menu);
            var permission = GetOrCreatePermission(rolePerm.Permission);
            string value = $"permission {rolePerm.Permission} that has been granted for level {rolePerm.Level} and menu {menu.Label}";
            ExpectedLog = GenericLogLineCreator.CreateLogLine(value, admin.Account.UserID,Table);
            var editPage = Perform(new OpenTheCreateRolePermissionPage());
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_edit");
            editPage.SelectMenu(menu);
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_MenuSelected");
            editPage.SelectPermission(permission);
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_PermissionSelected");
            editPage.SelectLevel(rolePerm.Level);
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LevelSelected");
            editPage.Create();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Saved");
        }
    }
}
