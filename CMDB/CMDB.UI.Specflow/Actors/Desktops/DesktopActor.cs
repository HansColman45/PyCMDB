using CMDB.UI.Specflow.Questions.Desktop;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.Desktops
{
    public class DesktopActor : CMDBActor
    {
        protected string Table => "desktop";
        public DesktopActor(ScenarioContext scenarioContext, string name = "DesktopActor") : base(scenarioContext, name)
        {
        }
        public string DesktopLastLogLine
        {
            get
            {
                var detailPage = Perform(new OpenTheDesktopDetailPage());
                return detailPage.GetLastLog();
            }
        }
    }
}
