using CMDB.UI.Specflow.Questions.Admin;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.Admins
{
    public class AdminActor : CMDBActor
    {
        public AdminActor(ScenarioContext scenarioContext, string name = "AdminActor") : base(scenarioContext, name)
        {
        }
        public string LastLogLine
        {
            get
            {
                var detail = Perform(new OpenTheAdminDetailPage());
                detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
                return detail.GetLastLog();
            }
        }
    }
}
