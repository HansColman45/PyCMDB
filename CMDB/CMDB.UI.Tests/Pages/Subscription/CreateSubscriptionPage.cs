using OpenQA.Selenium;

namespace CMDB.UI.Tests.Pages
{
    public class CreateSubscriptionPage : MainPage
    {
        public CreateSubscriptionPage(IWebDriver webDriver) : base(webDriver)
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