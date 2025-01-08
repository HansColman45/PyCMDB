using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Subscription
{
    public class CreateSubscriptionPage : MainPage
    {
        public CreateSubscriptionPage() : base()
        {
        }
        public string Type
        {
            set => SelectTektInDropDownByXpath("//select[@id='SubscriptionType']", value);
        }
        public string Phonenumber
        {
            set => EnterInTextboxByXPath("//input[@id='PhoneNumber']", value);
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}