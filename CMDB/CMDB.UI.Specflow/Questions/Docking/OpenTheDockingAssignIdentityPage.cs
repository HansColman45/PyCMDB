using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Docking;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Docking
{
    public class OpenTheDockingAssignIdentityPage : Question<DockingAssignIdentityPage>
    {
        public override DockingAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(DockingOverviewPage.AssignIdenityXpath);
            return new();
        }
    }
}
