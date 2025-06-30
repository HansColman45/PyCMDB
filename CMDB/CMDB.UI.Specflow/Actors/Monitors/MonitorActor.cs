using CMDB.UI.Specflow.Questions.Monitor;
using Reqnroll;

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
                return page.LastLogLine;
            }
        }
    }
}
