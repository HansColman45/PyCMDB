using Bright.ScreenPlay.Abilities;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages
{
    public class LoginPage : OpenAWebPage
    {
        public LoginPage()
        {
            Settings.BaseUrl = "https://localhost:44314/";
        }
        public LoginPage OpenLoginPage(IWebDriver webDriver)
        {
            WebDriver = webDriver;
            WebDriver.Navigate().GoToUrl(Settings.BaseUrl);
            WebDriver.Manage().Window.Maximize();
            return this;
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
            return new(WebDriver);
        }
    }
}
