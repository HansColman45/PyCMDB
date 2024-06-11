using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.Mobile
{
    public class CreateMobilePage : MainPage
    {
        public CreateMobilePage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string IMEI
        {
            set => EnterInTextboxByXPath("//input[@id='IMEI']", value);
        }
        public string Type
        {
            set => SelectValueInDropDownByXpath("//select[@id='MobileType']", value);
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}
