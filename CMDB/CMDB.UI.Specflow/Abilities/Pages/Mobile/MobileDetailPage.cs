using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Mobile
{
    public class MobileDetailPage : MainPage
    {
        public MobileDetailPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'mobile')]");
        }
    }
}
