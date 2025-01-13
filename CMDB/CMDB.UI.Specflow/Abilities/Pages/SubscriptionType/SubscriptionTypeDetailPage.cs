using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.SubscriptionType
{
    public class SubscriptionTypeDetailPage : MainPage
    {
        public SubscriptionTypeDetailPage() : base()
        {
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'subscriptiontype')]");
        }
    }
}
