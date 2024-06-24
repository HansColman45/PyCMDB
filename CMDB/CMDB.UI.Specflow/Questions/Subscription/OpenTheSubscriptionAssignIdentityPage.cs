using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Subscription;

namespace CMDB.UI.Specflow.Questions.Subscription
{
    public class OpenTheSubscriptionAssignIdentityPage : Question<SubscriptionAssignIdentityPage>
    {
        public override SubscriptionAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.AssignIdenityXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new();
        }
    }
}
