using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
