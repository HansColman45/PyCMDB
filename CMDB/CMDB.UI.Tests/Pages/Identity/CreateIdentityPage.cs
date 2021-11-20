using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class CreateIdentityPage : MainPage
    {
        public CreateIdentityPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string FirstName
        {
            set => EnterInTextboxByXPath("//input[@name='FirstName']", value);
        }
        public string LastName
        {
            set => EnterInTextboxByXPath("//input[@name='LastName']", value);
        }
        public string Email
        {
            set => EnterInTextboxByXPath("//input[@name='EMail']", value);
        }
        public string Language
        {
            set => SelectValueInDropDownByXpath("//select[@id='Language']", value);
        }
        public string Company
        {
            set => EnterInTextboxByXPath("//input[@name='Company']", value);
        }
        public string Type
        {
            set => SelectTektInDropDownByXpath("//select[@id='Type']", value);
        }
        public string UserId
        {
            set => EnterInTextboxByXPath("//input[@name='UserID']", value);
        }
    }
}
