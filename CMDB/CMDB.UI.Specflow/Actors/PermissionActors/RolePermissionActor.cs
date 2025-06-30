using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.RolePermissions;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.PermissionActors
{
    public class RolePermissionActor : CMDBActor
    {
        public RolePermissionActor(ScenarioContext scenarioContext, string name = "RolePermissionActor") : base(scenarioContext, name)
        {
        }
        protected string Table => "permission";
        public string LastLogLine
        {
            get
            {
                var detailPage = Perform(new OpenTheRolePermissionDetailPage());
                detailPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
                return detailPage.LastLogLine;
            }
        }
        public async Task<Menu> GetOrCreateMenu(string label)
        {
            var context = GetAbility<DataContext>();
            var menu = await context.GetOrCreateMenu(label, admin);
            return menu;
        }
        public Permission GetOrCreatePermission(string wright)
        {
            var context = GetAbility<DataContext>();
            var permission = context.GetPermission(wright);
            return permission;
        }
    }
}
