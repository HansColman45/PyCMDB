using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class CreateAccountPage : MainPage
    {
        public CreateAccountPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string UserId
        {
            set => EnterInTextboxByXPath("//input[@id='UserID']", value);
        }
        public string Type
        {
            set => SelectTektInDropDownByXpath("//select[@id='Type']", value);
        }
        public string Application
        {
            set => SelectTektInDropDownByXpath("//select[@id='Application']", value);
        }
    }
}
