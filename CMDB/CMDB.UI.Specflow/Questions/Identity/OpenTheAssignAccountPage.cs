using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Identity
{
    public class OpenTheAssignAccountPage : Question<AssignAccountPage>
    {
        public override AssignAccountPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath("//a[@title='Assign Account']");
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            AssignAccountPage assignAccountPage = WebPageFactory.Create<AssignAccountPage>(page.WebDriver);
            return assignAccountPage;
        }
    }
}
