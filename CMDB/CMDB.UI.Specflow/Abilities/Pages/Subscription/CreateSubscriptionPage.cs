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
            set => SelectTektInDropDownByXpath("//", value);
        }
        public string Phonenumber
        {
            set => EnterInTextboxByXPath("//", value);
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}