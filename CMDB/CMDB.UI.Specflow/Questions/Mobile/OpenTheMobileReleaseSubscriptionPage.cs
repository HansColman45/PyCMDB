using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;

namespace CMDB.UI.Specflow.Questions.Mobile
{
    public class OpenTheMobileReleaseSubscriptionPage : Question<MobileReleaseSubscriptionPage>
    {
        public override MobileReleaseSubscriptionPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MobileDetailPage>();
            page.ClickElementByXpath(MainPage.ReleaseSubscriptionXpath);
            return new();
        }
    }
}
