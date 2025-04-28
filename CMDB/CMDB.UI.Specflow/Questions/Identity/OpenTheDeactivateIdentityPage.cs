using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Identity
{
    public class OpenTheDeactivateIdentityPage : Question<DeactivateIdentityPage>
    {
        public override DeactivateIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath(MainPage.DeactivateXpath);
            page.WaitUntilElmentVisableByXpath("//input[@id='reason']");
            DeactivateIdentityPage deactivateIdentityPage = WebPageFactory.Create<DeactivateIdentityPage>(page.WebDriver);
            return deactivateIdentityPage;
        }
    }
}
