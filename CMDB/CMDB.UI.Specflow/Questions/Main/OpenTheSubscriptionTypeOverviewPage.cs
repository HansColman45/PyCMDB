using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.SubscriptionType;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheSubscriptionTypeOverviewPage : Question<SubscriptionTypeOverviewPage>
    {
        public override SubscriptionTypeOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.WaitUntilElmentVisableByXpath("//a[@id='Subscription Type38']");
            page.ClickElementByXpath("//a[@id='Subscription Type38']");
            page.ClickElementByXpath("//a[@href='/SubscriptionType']");
            page.WaitOnAddNew();
            return new();
        }
    }
}
