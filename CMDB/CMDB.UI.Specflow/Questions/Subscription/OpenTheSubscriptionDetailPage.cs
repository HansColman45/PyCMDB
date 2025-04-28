using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Subscription;

namespace CMDB.UI.Specflow.Questions.Subscription
{
    public class OpenTheSubscriptionDetailPage : Question<SubscriptionDetailPage>
    {
        public override SubscriptionDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionOverviewPage>();
            page.ClickElementByXpath(MainPage.InfoXpath);
            SubscriptionDetailPage subscriptionDetailPage = WebPageFactory.Create<SubscriptionDetailPage>(page.WebDriver);
            return subscriptionDetailPage;
        }
    }
}
