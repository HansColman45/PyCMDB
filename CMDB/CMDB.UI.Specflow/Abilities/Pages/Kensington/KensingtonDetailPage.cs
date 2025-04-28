using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Kensington
{
    class KensingtonDetailPage: MainPage
    {
        public KensingtonDetailPage(IWebDriver web) : base(web)
        {
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath(LogOverviewXpath));
            return TekstFromElementByXpath("//td[contains(text(),'kensington')]");
        }
    }
}
