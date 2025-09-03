using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Permissions;
using CMDB.UI.Specflow.Questions.Permissions;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.PermissionActors
{
    public class PermissionUpdator : PermissionActor
    {
        public PermissionUpdator(ScenarioContext scenarioContext, string name = "PermissionUpdator") : base(scenarioContext, name)
        {
        }

        public Permission DoUpdatePermission(Permission permission, string field, string newValue)
        {
            var editPage = Perform(new OpenTheEditPermissionPage());
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_editpage");
            switch (field)
            {
                case "Right":
                    var oldValue = permission.Rights;
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, oldValue, newValue, admin.Account.UserID, Table);
                    permission.Rights = newValue;
                    editPage.Right = newValue;
                    editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Right");
                    break;
                case "Description":
                    var oldDesc = permission.Description;
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, oldDesc, newValue, admin.Account.UserID, Table);
                    permission.Description = newValue;
                    editPage.Description = newValue;
                    editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Description");
                    break;
                default:
                    throw new ArgumentException($"Field '{field}' is not recognized for update.");
            }
            editPage.Edit();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_afteredit");
            return permission;
        }

        public void DoDelete()
        {
            var deletePage = Perform(new OpenTheDeletePermissionPage());
            deletePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_deletepage");
            deletePage.Delete();
            deletePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_deleted");
        }

        public int CountSearchedElements()
        {
            var detailsPage = GetAbility<PermissionOverviewPage>();
            return detailsPage.CountElementsByXPath("//div[@class='alert alert-danger']");
        }
    }
}
