using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Subscription
{
    public class SubscriptionOverviewPage : MainPage
    {
        public SubscriptionOverviewPage(IWebDriver web) : base(web)
        {
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
