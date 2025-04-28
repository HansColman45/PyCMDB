using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Subscription
{
    public class DeactivateSubscriptionPage : MainPage
    {
        public DeactivateSubscriptionPage(IWebDriver web) : base(web)
        {
        }
        public string Reason
        {
            set => EnterInTextboxByXPath("//input[@id='reason']", value);
        }
        public void Delete()
        {
            ClickElementByXpath("//button[.='Delete']");
        }
    }
}