using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class AssetTypeEditPage : MainPage
    {
        public AssetTypeEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string Category
        {
            set => SelectTektInDropDownByXpath("//select[@id='Category']", value);
        }
        public string Vendor
        {
            set => EnterInTextboxByXPath("//input[@id='Vendor']", value);
            get => TekstFromTextBox("//input[@id='Vendor']");
        }
        public string Type
        {
            set => EnterInTextboxByXPath("//input[@id='Type']", value);
            get => TekstFromTextBox("//input[@id='Type']");
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
            WaitOnAddNew();
        }
    }
}
