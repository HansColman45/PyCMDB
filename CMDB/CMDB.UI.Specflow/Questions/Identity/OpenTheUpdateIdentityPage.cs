using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Identity
{
    public class OpenTheUpdateIdentityPage : Question<UpdateIdentityPage>
    {
        public override UpdateIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath(MainPage.EditXpath);
            page.WaitUntilElmentVisableByXpath("//input[@name='FirstName']");
            UpdateIdentityPage updateIdentityPage = WebPageFactory.Create<UpdateIdentityPage>(page.WebDriver);
            return updateIdentityPage;
        }
    }
}
