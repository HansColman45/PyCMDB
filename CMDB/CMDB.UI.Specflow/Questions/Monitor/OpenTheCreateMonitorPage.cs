using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Monitor;

namespace CMDB.UI.Specflow.Questions.Monitor
{
    public class OpenTheCreateMonitorPage : Question<CreateMonitorPage>
    {
        public override CreateMonitorPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MonitorOverviewPage>();
            page.ClickElementByXpath(MainPage.NewXpath);
            CreateMonitorPage createMonitorPage = WebPageFactory.Create<CreateMonitorPage>(page.WebDriver);
            return createMonitorPage;
        }
    }
}
