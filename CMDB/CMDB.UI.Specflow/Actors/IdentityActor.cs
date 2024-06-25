using CMDB.UI.Specflow.Questions;
using CMDB.UI.Specflow.Questions.Identity;

namespace CMDB.UI.Specflow.Actors
{
    public class IdentityActor : CMDBActor
    {
        public IdentityActor(ScenarioContext scenarioContext, string name = "Identity") : base(scenarioContext, name)
        {
        }
        public string IdentityLastLogLine
        {
            get
            {
                var detail = Perform(new OpenTheIdentityDetailPage());
                detail.WebDriver = Driver;
                detail.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_detail");
                return Perform(new TheIdentityDertailLastLogLine());
            }
        }
    }
}
