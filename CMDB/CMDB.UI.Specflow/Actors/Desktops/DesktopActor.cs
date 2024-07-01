using CMDB.UI.Specflow.Questions.Desktop;

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
                detailPage.WebDriver = Driver;
                return detailPage.GetLastLog();
            }
        }
    }
}
