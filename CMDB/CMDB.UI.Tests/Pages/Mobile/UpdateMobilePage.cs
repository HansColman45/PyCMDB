using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class UpdateMobilePage : MainPage
    {
        public UpdateMobilePage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string IMEI
        {
            set => EnterInTextboxByXPath("//input[@id='IMEI']", value);
            get => TekstFromElementByXpath("//input[@id='IMEI']");
        }
        public string Type
        {
            set => SelectValueInDropDownByXpath("//select[@id='MobileType.Id']", value);
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
            WaitOnAddNew();
        }
    }
}
