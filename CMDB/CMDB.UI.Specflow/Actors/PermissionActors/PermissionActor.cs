using CMDB.UI.Specflow.Abilities.Pages.Permissions;
using CMDB.UI.Specflow.Questions.Permissions;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.PermissionActors
{
    public class PermissionActor : CMDBActor
    {
        public PermissionActor(ScenarioContext scenarioContext, string name = "Permission") : base(scenarioContext, name)
        {
        }
        protected static string Table => "permission";

        public string LastLogLine()
        {
            var page = Perform(new OpenThePermissionDetailPage());
            return page.LastLogLine;
        }
    }
}
