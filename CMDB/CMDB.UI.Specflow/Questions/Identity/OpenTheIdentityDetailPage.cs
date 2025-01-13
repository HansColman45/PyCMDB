using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Identity
{
    public class OpenTheIdentityDetailPage : Question<IdentityDetailPage>
    {
        public override IdentityDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath(IdentityOverviewPage.InfoXpath);
            return new();
        }
    }
}
