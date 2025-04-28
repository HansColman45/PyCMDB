using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Subscription;

namespace CMDB.UI.Specflow.Questions.Subscription
{
    public class OpenTheSubscriptionEditPage : Question<UpdateSubscriptionPage>
    {
        public override UpdateSubscriptionPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionOverviewPage>();
            page.ClickElementByXpath(MainPage.EditXpath);
            UpdateSubscriptionPage updateSubscriptionPage = WebPageFactory.Create<UpdateSubscriptionPage>(page.WebDriver);
            return updateSubscriptionPage;
        }
    }
}
