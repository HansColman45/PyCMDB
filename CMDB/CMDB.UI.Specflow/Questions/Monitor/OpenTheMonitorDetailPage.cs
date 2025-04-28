using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Monitor;

namespace CMDB.UI.Specflow.Questions.Monitor
{
    public class OpenTheMonitorDetailPage : Question<MonitorDetailPage>
    {
        public override MonitorDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MonitorOverviewPage>();
            page.ClickElementByXpath(MainPage.InfoXpath);
            MonitorDetailPage monitorDetailPage = WebPageFactory.Create<MonitorDetailPage>(page.WebDriver);
            return monitorDetailPage;
        }
    }
}
