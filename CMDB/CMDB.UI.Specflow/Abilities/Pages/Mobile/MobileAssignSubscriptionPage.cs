using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Mobile
{
    public class MobileAssignSubscriptionPage : MainPage
    {
        public MobileAssignSubscriptionPage() : base()
        {
        }
        public void SelectSubscription(Domain.Entities.Subscription subscription)
        {
            SelectValueInDropDownByXpath($"//*[@id='Subscription']",subscription.SubscriptionId.ToString());
        }
    }
}
