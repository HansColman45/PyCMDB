using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheIdentityOverviewPage : Question<IdentityOverviewPage>
    {
        public override IdentityOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Identity']");
            page.ClickElementByXpath("//a[@id='Identity']");
            page.ClickElementByXpath("//a[@id='Identity2']");
            page.ClickElementByXpath("//a[@href='/Identity']");
            page.WaitOnAddNew();
            IdentityOverviewPage identityOverviewPage = WebPageFactory.Create<IdentityOverviewPage>(page.WebDriver);
            return identityOverviewPage;
        }
    }
}
