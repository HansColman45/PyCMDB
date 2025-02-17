using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Subscription
{
    public class SubscriptionAssignMobilePage : MainPage
    {
        public void SelectMobile(Domain.Entities.Mobile mobile)
        {
            SelectValueInDropDownByXpath("//select[@id='Mobile']", $"{mobile.MobileId}");
        }
    }
}