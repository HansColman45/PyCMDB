using CMDB.UI.Specflow.Questions.Types;

namespace CMDB.UI.Specflow.Actors.AccountTypes
{
    public class AccountTypeActor : CMDBActor
    {
        public AccountTypeActor(ScenarioContext scenarioContext, string name = "AccountAcctor") : base(scenarioContext, name)
        {
        }
        public string AccountTypeLastLogLine
        {
            get
            {
                var detail = Perform(new OpenTheTypeDetailsPage());
                detail.WebDriver = Driver;
                detail.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_detail");
                return detail.GetLastLog("accounttype");
            }
        }
    }
}
