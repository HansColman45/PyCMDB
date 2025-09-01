using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Permissions;
using CMDB.UI.Specflow.Questions.Permissions;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.PermissionActors
{
    public class PermissionCreator : PermissionActor
    {
        public PermissionCreator(ScenarioContext scenarioContext, string name = "PermissionCreator") : base(scenarioContext, name)
        {
        }

        public void DoCreatePermission(Helpers.Permission permission)
        {
            rndNr = rnd.Next();
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"permission {permission.Right+rndNr} with {permission.Description+rndNr}", admin.Account.UserID, Table);
            var createPage = Perform(new OpenTheCreatePermissionPage());
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_createpage");
            createPage.Right = permission.Right+rndNr;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Right");
            createPage.Description = permission.Description+rndNr;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Description");
            createPage.Create();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_aftercreate");
        }
        public void SearchPermission(Helpers.Permission permission)
        {
            var page = GetAbility<PermissionOverviewPage>();
            Search(permission.Right + rndNr);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_searched");
        }
    }
}
