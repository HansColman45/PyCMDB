using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Token;

namespace CMDB.UI.Specflow.Questions.Token
{
    public class OpenTheTokenAssignIdentityPage : Question<TokenAssignIdentityPage>
    {
        public override TokenAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TokenOverviewPage>();
            page.ClickElementByXpath(MainPage.AssignIdenityXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            TokenAssignIdentityPage tokenAssignIdentityPage = WebPageFactory.Create<TokenAssignIdentityPage>(page.WebDriver);
            return tokenAssignIdentityPage;
        }
    }
}
