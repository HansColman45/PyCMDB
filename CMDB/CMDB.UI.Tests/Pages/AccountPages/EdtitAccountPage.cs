using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class EditAccountPage : MainPage
    {
        public EditAccountPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string UserId
        {
            set => EnterInTextboxByXPath("//input[@id='UserID']", value);
            get => TekstFromTextBox("//input[@id='UserID']");
        }
        public string Type
        {
            set => SelectTektInDropDownByXpath("//select[@id='Type_TypeId']", value);
        }
        public string Application
        {
            set => SelectTektInDropDownByXpath("//select[@id='Application_AppID']", value);
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
            WaitOnAddNew();
        }
    }
}
