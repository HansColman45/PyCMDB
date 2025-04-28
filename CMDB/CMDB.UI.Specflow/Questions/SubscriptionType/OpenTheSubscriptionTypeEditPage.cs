using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.SubscriptionType;

namespace CMDB.UI.Specflow.Questions.SubscriptionType
{
    public class OpenTheSubscriptionTypeEditPage : Question<UpdateSubscriptionTypePage>
    {
        public override UpdateSubscriptionTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionTypeOverviewPage>();
            page.ClickElementByXpath(MainPage.EditXpath);
            UpdateSubscriptionTypePage updateSubscriptionTypePage = WebPageFactory.Create<UpdateSubscriptionTypePage>(page.WebDriver);
            return updateSubscriptionTypePage;
        }
    }
}
