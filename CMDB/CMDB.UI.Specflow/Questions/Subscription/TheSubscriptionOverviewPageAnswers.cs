using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Subscription;

namespace CMDB.UI.Specflow.Questions.Subscription
{
    public class OpenTheSubscriptionCreatePage : Question<CreateSubscriptionPage>
    {
        public override CreateSubscriptionPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheSubscriptionDetailPage : Question<SubscriptionDetailPage>
    {
        public override SubscriptionDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheSubscriptionEditPage : Question<UpdateSubscriptionPage>
    {
        public override UpdateSubscriptionPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheSubscriptionDeactivatePage : Question<DeactivateSubscriptionPage>
    {
        public override DeactivateSubscriptionPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheSubscriptionAssignIdentityPage : Question<SubscriptionAssignIdentityPage>
    {
        public override SubscriptionAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.AssignIdenityXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(page.WebDriver);
        }
    }
}
