using Microsoft.Graph;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.AccountPages
{
    public class AccountDetailPage : MainPage
    {
        public AccountDetailPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public int Id
        {
            get
            {
                string id = GetAttributeFromXpath("//td[@id='Id']", "innerHTML");
                return Int32.Parse(id);
            }
        }
        public AccountReleaseIdentityPage ReleaseIdentity()
        {
            ClickElementByXpath("//a[@id='ReleaseIdentity']");
            return new(WebDriver);
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'account')]");
        }
    }
}
