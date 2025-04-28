using CMDB.UI.Specflow.Questions.Types;

namespace CMDB.UI.Specflow.Actors.AccountTypes
{
    public class AccountTypeActor : CMDBActor
    {
        protected string Table => "accounttype";
        public AccountTypeActor(ScenarioContext scenarioContext, string name = "AccountAcctor") : base(scenarioContext, name)
        {
        }
        public string AccountTypeLastLogLine
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
