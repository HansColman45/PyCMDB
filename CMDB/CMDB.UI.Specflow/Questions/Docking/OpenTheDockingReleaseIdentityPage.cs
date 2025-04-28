using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Docking;

namespace CMDB.UI.Specflow.Questions.Docking
{
    public class OpenTheDockingReleaseIdentityPage : Question<DockingReleaseIdentityPage>
    {
        public override DockingReleaseIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingDetailPage>();
            page.ClickElementByXpath(MainPage.ReleaseIdenityXpath);
            DockingReleaseIdentityPage dockingReleaseIdentityPage = WebPageFactory.Create<DockingReleaseIdentityPage>(page.WebDriver);
            return dockingReleaseIdentityPage;
        }
    }
}
