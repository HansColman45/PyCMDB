using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.Identity
{
    public class IdentityDetailPage : MainPage
    {
        public IdentityDetailPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public int Id
        {
            get
            {
                string id = GetAttributeFromXpath("//td[@id='Id']", "innerHTML");
                return int.Parse(id);
            }
        }
        public ReleaseAccountPage ReleaseAccount()
        {
            ClickElementByXpath(ReleaseAccountXPath);
            return new(WebDriver);
        }
        public ReleaseDevicePage ReleaseDevice()
        {
            ClickElementByXpath(ReleaseDeviceXPath);
            return new(WebDriver);
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'identity')]");
        }
    }
}
