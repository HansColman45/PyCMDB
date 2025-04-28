using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;

namespace CMDB.UI.Specflow.Questions.Mobile
{
    public class OpenTheMobileAssignIdentityPage : Question<MobileAssignIdentityPage>
    {
        public override MobileAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MobileOverviewPage>();
            page.ClickElementByXpath(MainPage.AssignIdenityXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            MobileAssignIdentityPage mobileAssignIdentityPage = WebPageFactory.Create<MobileAssignIdentityPage>(page.WebDriver);
            return mobileAssignIdentityPage;
        }
    }
}
