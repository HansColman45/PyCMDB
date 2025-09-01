using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Permissions
{
    public class PermissionDetailPage: MainPage
    {
        public PermissionDetailPage(IWebDriver driver): base(driver)
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
