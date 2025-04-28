using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Subscription;

namespace CMDB.UI.Specflow.Questions.Subscription
{
    public class OpenTheSubscriptionCreatePage : Question<CreateSubscriptionPage>
    {
        public override CreateSubscriptionPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionOverviewPage>();
            page.ClickElementByXpath(MainPage.NewXpath);
            CreateSubscriptionPage createSubscriptionPage = WebPageFactory.Create<CreateSubscriptionPage>(page.WebDriver);
            return createSubscriptionPage;
        }
    }
}
