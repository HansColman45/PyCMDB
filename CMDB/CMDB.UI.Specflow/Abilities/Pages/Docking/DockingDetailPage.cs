using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Docking
{
    public class DockingDetailPage : MainPage
    {
        public DockingDetailPage(IWebDriver web) : base(web)
        {
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'docking')]");
        }
    }
}
