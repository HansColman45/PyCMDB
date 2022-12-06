using OpenQA.Selenium;

namespace CMDB.UI.Tests.Pages
{
    public class SubscriptionDetailPage : MainPage
    {
        public SubscriptionDetailPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'subscription')]");
        }
    }
}