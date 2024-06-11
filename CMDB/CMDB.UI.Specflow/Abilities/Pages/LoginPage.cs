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
        public string UserId
        {
            set => EnterInTextboxByXPath("//input[@type='text']", value);
        }
        public string Password
        {
            set => EnterInTextboxByXPath("//input[@type='password']", value);
        }
        public MainPage LogIn()
        {
            ClickElementByXpath("//button[@type='submit']");
            WaitUntilElmentVisableByXpath("//h1");
            return new(WebDriver);
        }
    }
}
