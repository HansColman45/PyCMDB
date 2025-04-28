using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Subscription
{
    public class UpdateSubscriptionPage : MainPage
    {
        public UpdateSubscriptionPage(IWebDriver web) : base(web)
        {
        }
        public string Phonenumber
        {
            set => EnterInTextboxByXPath("//input[@id='PhoneNumber']", value);
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
        }
    }
}