using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Identity
{
    public class OpenTheReleaseAccountPage : Question<ReleaseAccountPage>
    {
        public override ReleaseAccountPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityDetailPage>();
            page.ClickElementByXpath(IdentityOverviewPage.ReleaseAccountXPath);
            return new();
        }
    }
}
