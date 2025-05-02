using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Monitor;

namespace CMDB.UI.Specflow.Questions.Monitor
{
    public class OpenTheMonitorAssignKensingtonPage : Question<MonitorAssignKensingtonPage>
    {
        public override MonitorAssignKensingtonPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MonitorOverviewPage>();
            page.ClickElementByXpath(MainPage.AssignKensingtonXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            MonitorAssignKensingtonPage monitorAssignKensingtonPage = WebPageFactory.Create<MonitorAssignKensingtonPage>(page.WebDriver);
            return monitorAssignKensingtonPage;
        }
    }
}
