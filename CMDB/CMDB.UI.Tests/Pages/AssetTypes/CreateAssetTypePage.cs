using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class CreateAssetTypePage : MainPage
    {
        public CreateAssetTypePage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string Category
        {
            set => SelectTektInDropDownByXpath("//select[@id='Category']", value);
        }
        public string Vendor
        {
            set => EnterInTextboxByXPath("//input[@id='Vendor']", value);
        }
        public string Type
        {
            set => EnterInTextboxByXPath("//input[@id='Type']", value);
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}
