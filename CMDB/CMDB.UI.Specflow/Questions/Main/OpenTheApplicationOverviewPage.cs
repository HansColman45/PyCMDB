using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Application;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheApplicationOverviewPage : Question<ApplicationOverviewPage>
    {
        public override ApplicationOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Application']");
            page.ClickElementByXpath("//a[@id='Application']");
            page.ClickElementByXpath("//a[@id='Application44']");
            page.ClickElementByXpath("//a[@href='/Application']");
            page.WaitOnAddNew();
            ApplicationOverviewPage applicationOverviewPage = WebPageFactory.Create<ApplicationOverviewPage>(page.WebDriver);
            return applicationOverviewPage;
        }
    }
}
