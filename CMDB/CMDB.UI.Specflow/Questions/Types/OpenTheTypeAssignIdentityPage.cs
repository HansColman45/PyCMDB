using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Types;

namespace CMDB.UI.Specflow.Questions.Types
{
    public class OpenTheTypeAssignIdentityPage : Question<TypeAssignIdentityPage>
    {
        public override TypeAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TypeOverviewPage>();
            page.ClickElementByXpath(MainPage.AssignIdenityXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            TypeAssignIdentityPage typeAssignIdentityPage = WebPageFactory.Create<TypeAssignIdentityPage>(page.WebDriver);
            return typeAssignIdentityPage;
        }
    }
}
