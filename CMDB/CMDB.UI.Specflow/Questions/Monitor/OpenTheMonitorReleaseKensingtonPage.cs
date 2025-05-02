using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Monitor;

namespace CMDB.UI.Specflow.Questions.Monitor
{
    public class OpenTheMonitorReleaseKensingtonPage : Question<MonitorReleaseKensingtonPage>
    {
        public override MonitorReleaseKensingtonPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MonitorOverviewPage>();
            page.ClickElementByXpath(MainPage.ReleaseKensingtonXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            MonitorReleaseKensingtonPage monitorReleaseKensingtonPage = WebPageFactory.Create<MonitorReleaseKensingtonPage>(page.WebDriver);
            return monitorReleaseKensingtonPage;
        }
    }
}
