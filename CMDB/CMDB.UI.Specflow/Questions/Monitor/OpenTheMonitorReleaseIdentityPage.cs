using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Monitor;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Monitor
{
    public class OpenTheMonitorReleaseIdentityPage : Question<MonitorReleaseIdentityPage>
    {
        public override MonitorReleaseIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MonitorDetailPage>();
            page.ClickElementByXpath(MainPage.ReleaseIdenityXpath);
            MonitorReleaseIdentityPage monitorReleaseIdentityPage = WebPageFactory.Create<MonitorReleaseIdentityPage>(page.WebDriver);
            return monitorReleaseIdentityPage;
        }
    }
}
