using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
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
                return Int32.Parse(id);
            }
        }
        public ReleaseAccountPage ReleaseAccount()
        {
            ClickElementByXpath(ReleaseAccountXPath);
            return new(driver);
        }
        public ReleaseDevicePage ReleaseDevice()
        {
            ClickElementByXpath(ReleaseDeviceXPath);
            return new(driver);
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'identity')]");
        }
    }
}
