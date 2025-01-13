using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Identity;

namespace CMDB.UI.Specflow.Questions.Identity
{
    public class OpenTheReleaseSubscriptionPage : Question<ReleaseSubscriptionPage>
    {
        public override ReleaseSubscriptionPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityDetailPage>();
            page.ClickElementByXpath(IdentityDetailPage.ReleaseSubscriptionXPath);
            return new();
        }
    }
}
