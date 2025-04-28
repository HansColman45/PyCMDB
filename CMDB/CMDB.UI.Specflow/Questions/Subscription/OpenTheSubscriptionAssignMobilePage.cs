using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Subscription;

namespace CMDB.UI.Specflow.Questions.Subscription
{
    public class OpenTheSubscriptionAssignMobilePage : Question<SubscriptionAssignMobilePage>
    {
        public override SubscriptionAssignMobilePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionOverviewPage>();
            page.ClickElementByXpath("//a[@title='AssignMobile']");
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            SubscriptionAssignMobilePage subscriptionAssignMobilePage = WebPageFactory.Create<SubscriptionAssignMobilePage>(page.WebDriver);
            return subscriptionAssignMobilePage;
        }
    }
}
