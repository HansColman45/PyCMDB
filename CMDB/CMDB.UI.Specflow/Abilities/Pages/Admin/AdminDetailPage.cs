using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Admin
{
    public class AdminDetailPage:MainPage
    {
        public AdminDetailPage(IWebDriver driver): base(driver)
        {
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'admin')]");
        }
    }
}
