using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.Desktop
{
    public class DesktopDetailPage : MainPage
    {
        public DesktopDetailPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public DesktopReleaseIdentityPage ReleaseIdentity()
        {
            ClickElementByXpath(ReleaseIdenityXpath);
            return new(WebDriver);
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'desktop')]");
        }
    }
}
