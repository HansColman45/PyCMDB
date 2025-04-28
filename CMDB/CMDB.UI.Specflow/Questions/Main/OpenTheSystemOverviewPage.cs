using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.System;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheSystemOverviewPage : Question<SystemOverviewPage>
    {
        public override SystemOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='System']");
            page.ClickElementByXpath("//a[@id='System']");
            page.ClickElementByXpath("//a[@id='System41']");
            page.ClickElementByXpath("//a[@href='/System']");
            page.WaitOnAddNew();
            SystemOverviewPage systemOverviewPage = WebPageFactory.Create<SystemOverviewPage>(page.WebDriver);
            return systemOverviewPage;
        }
    }
}
