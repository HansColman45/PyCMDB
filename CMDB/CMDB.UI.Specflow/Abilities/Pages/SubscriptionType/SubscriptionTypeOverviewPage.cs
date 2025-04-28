using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.SubscriptionType
{
    public class SubscriptionTypeOverviewPage : MainPage
    {
        public SubscriptionTypeOverviewPage(IWebDriver web) : base(web)
        {
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
