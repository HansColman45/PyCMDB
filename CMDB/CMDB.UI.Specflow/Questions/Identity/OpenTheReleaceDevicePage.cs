using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Identity
{
    public class OpenTheReleaceDevicePage : Question<ReleaseDevicePage>
    {
        public override ReleaseDevicePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityDetailPage>();
            page.ClickElementByXpath(IdentityOverviewPage.ReleaseDeviceXPath);
            return new();
        }
    }
}
