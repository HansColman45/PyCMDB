using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Kensington
{
    class KensingtonDetailPage: MainPage
    {
        public string GetLastLog()
        {
            ScrollToElement(By.XPath(LogOverviewXpath));
            return TekstFromElementByXpath("//td[contains(text(),'kensington')]");
        }
    }
}
