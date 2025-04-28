using CMDB.UI.Specflow.Questions.Main;
using CMDB.UI.Specflow.Questions.Types;

namespace CMDB.UI.Specflow.Actors.IdentityTypes
{
    public class IdentityTypeActor : CMDBActor
    {
        protected string Table = "identitytype";
        public IdentityTypeActor(ScenarioContext scenarioContext, string name = "IdentityType") : base(scenarioContext, name)
        {
        }
        public string IdentityTypeLastLogLine
        {
            get
            {
                var detail = Perform(new OpenTheTypeDetailsPage());
                detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
                return detail.GetLastLog(Table);
            }
        }
    }
}
