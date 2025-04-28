using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.SubscriptionType;

namespace CMDB.UI.Specflow.Questions.SubscriptionType
{
    public class OpenTheSubscriptionTypeDetailPage : Question<SubscriptionTypeDetailPage>
    {
        public override SubscriptionTypeDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionTypeOverviewPage>();
            page.ClickElementByXpath(MainPage.InfoXpath);
            SubscriptionTypeDetailPage subscriptionTypeDetailPage = WebPageFactory.Create<SubscriptionTypeDetailPage>(page.WebDriver);
            return subscriptionTypeDetailPage;
        }
    }
}
