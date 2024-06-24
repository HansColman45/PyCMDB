using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Subscription;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheSubscriptionOverviewPage : Question<SubscriptionOverviewPage>
    {
        public override SubscriptionOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Subscription25']");
            page.ClickElementByXpath("//a[@href='/Subscription']");
            page.WaitOnAddNew();
            return new();
        }
    }
}
