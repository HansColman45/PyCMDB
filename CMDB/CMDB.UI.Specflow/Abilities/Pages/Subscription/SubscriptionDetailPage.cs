using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Subscription
{
    public class SubscriptionDetailPage : MainPage
    {
        public SubscriptionDetailPage(IWebDriver web) : base(web)
        {
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'subscription')]");
        }
    }
}