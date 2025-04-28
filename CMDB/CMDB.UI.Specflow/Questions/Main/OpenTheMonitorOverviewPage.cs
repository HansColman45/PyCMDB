using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Monitor;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheMonitorOverviewPage : Question<MonitorOverviewPage>
    {
        public override MonitorOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Monitor15']");
            page.ClickElementByXpath("//a[@href='/Monitor']");
            page.WaitOnAddNew();
            MonitorOverviewPage monitorOverviewPage = WebPageFactory.Create<MonitorOverviewPage>(page.WebDriver);
            return monitorOverviewPage;
        }
    }
}
