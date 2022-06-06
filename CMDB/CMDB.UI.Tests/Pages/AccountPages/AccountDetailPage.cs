using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
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
            return new(driver);
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'account')]");
        }
    }
}
