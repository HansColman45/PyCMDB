using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Subscription
{
    public class SubscriptionAssignMobilePage : MainPage
    {
        public SubscriptionAssignMobilePage(IWebDriver web) : base(web)
        {
        }
        public void SelectMobile(Domain.Entities.Mobile mobile)
        {
            SelectValueInDropDownByXpath("//select[@id='Mobile']", $"{mobile.MobileId}");
        }
    }
}