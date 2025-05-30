using CMDB.UI.Specflow.Questions;
using CMDB.UI.Specflow.Questions.Identity;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.IdentityActors
{
    public class IdentityActor : CMDBActor
    {
        protected string Table => "identity";
        public IdentityActor(ScenarioContext scenarioContext, string name = "Identity") : base(scenarioContext, name)
        {
        }
        public string IdentityLastLogLine
        {
            get
            {
                var detail = Perform(new OpenTheIdentityDetailPage());
                detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
                return Perform(new TheIdentityDetailLastLogLine());
            }
        }
    }
}
