using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.Laptop
{
    public class LaptopDetailPage : MainPage
    {
        public LaptopDetailPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public LaptopReleaseIdentityPage ReleaseIdentity()
        {
            ClickElementByXpath(ReleaseIdenityXpath);
            return new(WebDriver);
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'laptop')]");
        }
    }
}
