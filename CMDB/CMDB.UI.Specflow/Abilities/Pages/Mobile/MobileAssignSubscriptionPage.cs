using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Mobile
{
    public class MobileAssignSubscriptionPage : MainPage
    {
        public MobileAssignSubscriptionPage(IWebDriver web) : base(web)
        {
        }
        public void SelectSubscription(Domain.Entities.Subscription subscription)
        {
            SelectValueInDropDownByXpath($"//*[@id='Subscription']",subscription.SubscriptionId.ToString());
        }
    }
}
