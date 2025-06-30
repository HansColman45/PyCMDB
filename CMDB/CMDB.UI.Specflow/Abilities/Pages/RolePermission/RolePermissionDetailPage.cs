using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.RolePermission
{
    public class RolePermissionDetailPage : MainPage
    {
        public RolePermissionDetailPage(IWebDriver webDriver): base(webDriver)
        {
        }
        public string LastLogLine
        {
            get
            {
                ScrollToElement(By.XPath("//h3[.='Log overview']"));
                return TekstFromElementByXpath("//td[contains(text(),'permission')]");
            }
        }
    }
}
