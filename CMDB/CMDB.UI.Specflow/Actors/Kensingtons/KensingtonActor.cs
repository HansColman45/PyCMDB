using CMDB.UI.Specflow.Questions.Keys;

namespace CMDB.UI.Specflow.Actors.Kensingtons
{
    public class KensingtonActor : CMDBActor
    {
        protected string Table = "kensington";
        public KensingtonActor(ScenarioContext scenarioContext, string name = "Kensington") : base(scenarioContext, name)
        {
        }
        public string LastLogLine
        {
            get
            {
                var detail = Perform(new OpenTheKensingtonDetailPage());
                detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
                return detail.GetLastLog();
            }
        }
    }
}
