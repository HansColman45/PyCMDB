using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class AssetTypeDetailPage : MainPage
    {
        public AssetTypeDetailPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public int Id
        {
            get
            {
                string id = TekstFromTextBox("//td[@id='Id']");
                return Int32.Parse(id);
            }
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'assettype')]");
        }
    }
}
