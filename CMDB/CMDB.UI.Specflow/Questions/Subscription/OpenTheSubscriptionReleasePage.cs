using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Subscription;

namespace CMDB.UI.Specflow.Questions.Subscription
{
    public class OpenTheSubscriptionReleasePage : Question<SubscriptionReleasePage>
    {
        public override SubscriptionReleasePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionDetailPage>();
            page.ClickElementByXpath(MainPage.ReleaseIdenityXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            SubscriptionReleasePage subscriptionReleasePage = WebPageFactory.Create<SubscriptionReleasePage>(page.WebDriver);
            return subscriptionReleasePage;
        }
    }
    public class OpenTheMobileSubscriptionReleasePage : Question<SubscriptionReleasePage>
    {
        public override SubscriptionReleasePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionDetailPage>();
            page.ClickElementByXpath(MainPage.ReleaseMobileXPath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            SubscriptionReleasePage subscriptionReleasePage = WebPageFactory.Create<SubscriptionReleasePage>(page.WebDriver);
            return subscriptionReleasePage;
        }
    }
}
