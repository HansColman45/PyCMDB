using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Docking;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Docking
{
    public class OpenTheDockingReleaseIdentityPage : Question<DockingReleaseIdentityPage>
    {
        public override DockingReleaseIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingDetailPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.ReleaseAccountXPath);
            return new();
        }
    }
}
