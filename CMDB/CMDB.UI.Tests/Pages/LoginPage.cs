using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class LoginPage : Page
    {
        public LoginPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public void EnterUserID(string userId)
        {
            EnterInTextboxByXPath("//input[@type='text']", userId);
        }
        public void EnterPassword(string password)
        {
            EnterInTextboxByXPath("//input[@type='password']", password);
        }
        public MainPage LogIn()
        {
            ClickElementByXpath("//button[@type='submit']");
            WaitUntilElmentVisableByXpath("//h1");
            return new(driver);
        }
    }
}
