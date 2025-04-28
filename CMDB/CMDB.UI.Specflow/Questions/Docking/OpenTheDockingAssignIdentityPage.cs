using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Docking;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Docking
{
    public class OpenTheDockingAssignIdentityPage : Question<DockingAssignIdentityPage>
    {
        public override DockingAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(DockingOverviewPage.AssignIdenityXpath);
            DockingAssignIdentityPage assignIdentityPage = WebPageFactory.Create<DockingAssignIdentityPage>(page.WebDriver);
            return assignIdentityPage;
        }
    }
}
