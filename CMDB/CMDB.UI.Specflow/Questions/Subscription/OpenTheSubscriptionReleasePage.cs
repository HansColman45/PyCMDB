using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Subscription;

namespace CMDB.UI.Specflow.Questions.Subscription
{
    public class OpenTheSubscriptionReleasePage : Question<SubscriptionReleasePage>
    {
        public override SubscriptionReleasePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionDetailPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.ReleaseIdenityXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new();
        }
    }
    public class OpenTheMobileSubscriptionReleasePage : Question<SubscriptionReleasePage>
    {
        public override SubscriptionReleasePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionDetailPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.ReleaseMobileXPath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new();
        }
    }
}
