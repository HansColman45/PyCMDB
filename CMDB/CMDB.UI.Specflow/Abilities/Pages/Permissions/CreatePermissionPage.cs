using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Permissions
{
    public class CreatePermissionPage: MainPage
    {
        public CreatePermissionPage(IWebDriver webDriver): base(webDriver)
        {
        }

        public string Right
        {
            set
            {
                EnterInTextboxByXPath("//input[@id='Right']", value);
            }
        }
        public string Description
        {
            set
            {
                EnterInTextboxByXPath("//input[@id='Description']", value);
            }
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}
