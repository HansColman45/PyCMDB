using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.SubscriptionType;

namespace CMDB.UI.Specflow.Questions.SubscriptionType
{
    public class OpenTheSubscriptionTypeDeactivatePage : Question<DeactivateSubscriptionTypePage>
    {
        public override DeactivateSubscriptionTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionTypeOverviewPage>();
            page.ClickElementByXpath(MainPage.DeactivateXpath);
            DeactivateSubscriptionTypePage deactivateSubscriptionTypePage = WebPageFactory.Create<DeactivateSubscriptionTypePage>(page.WebDriver);
            return deactivateSubscriptionTypePage;
        }
    }
}
