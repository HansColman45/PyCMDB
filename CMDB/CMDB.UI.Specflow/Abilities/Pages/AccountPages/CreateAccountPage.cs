using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.AccountPages
{
    public class CreateAccountPage : MainPage
    {
        public CreateAccountPage() : base()
        {
        }
        public string UserId
        {
            set => EnterInTextboxByXPath("//input[@id='UserID']", value);
        }
        public string Type
        {
            set => SelectTektInDropDownByXpath("//select[@id='Type']", value);
        }
        public string Application
        {
            set => SelectTektInDropDownByXpath("//select[@id='Application']", value);
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}
