using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Admin
{
    public class DeactivateAdminPage : MainPage
    {
        public DeactivateAdminPage(IWebDriver driver) : base(driver)
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
