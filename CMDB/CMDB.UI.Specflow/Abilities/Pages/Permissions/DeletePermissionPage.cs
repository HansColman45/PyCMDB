using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Permissions
{
    public class DeletePermissionPage : MainPage
    {
        public DeletePermissionPage(IWebDriver webDriver): base(webDriver)
        {
        }

        public void Delete()
        {
            ClickElementByXpath("//button[.='Delete']");
        }
    }
}
