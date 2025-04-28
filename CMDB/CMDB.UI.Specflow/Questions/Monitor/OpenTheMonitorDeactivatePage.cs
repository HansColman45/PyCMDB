using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Monitor;

namespace CMDB.UI.Specflow.Questions.Monitor
{
    public class OpenTheMonitorDeactivatePage : Question<DeactivateMonitorPage>
    {
        public override DeactivateMonitorPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MonitorOverviewPage>();
            page.ClickElementByXpath(MainPage.DeactivateXpath);
            DeactivateMonitorPage deactivateMonitorPage = WebPageFactory.Create<DeactivateMonitorPage>(page.WebDriver);
            return deactivateMonitorPage;
        }
    }
}
