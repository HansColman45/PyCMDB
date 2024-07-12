
using CMDB.UI.Specflow.Abilities.Pages.Monitor;
using CMDB.UI.Specflow.Questions.Mobile;
using CMDB.UI.Specflow.Questions.Monitor;

namespace CMDB.UI.Specflow.Actors.Monitors
{
    public class MonitorActor : CMDBActor
    {
        public MonitorActor(ScenarioContext scenarioContext, string name = "MonitorActor") : base(scenarioContext, name)
        {
        }
        protected static string Table => "screen";

        public string GetLastMonitorLogLine
        {
            get
            {
                var page = Perform(new OpenTheMonitorDetailPage());
                page.WebDriver = Driver;
                return page.GetLastLog();
            }
        }
    }
}
