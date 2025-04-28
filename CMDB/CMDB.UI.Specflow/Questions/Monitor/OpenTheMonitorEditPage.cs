using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Monitor;

namespace CMDB.UI.Specflow.Questions.Monitor
{
    public class OpenTheMonitorEditPage : Question<UpdateMonitorPage>
    {
        public override UpdateMonitorPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MonitorOverviewPage>();
            page.ClickElementByXpath(MainPage.EditXpath);
            UpdateMonitorPage updateMonitorPage = WebPageFactory.Create<UpdateMonitorPage>(page.WebDriver);
            return updateMonitorPage;
        }
    }
}
