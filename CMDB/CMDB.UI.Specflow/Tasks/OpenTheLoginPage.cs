using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace CMDB.UI.Specflow.Tasks
{
    public class OpenTheLoginPage : Question<IWebDriver>
    {
        public override IWebDriver PerformAs(IPerformer actor)
        {
            var options = new FirefoxOptions
            {
                AcceptInsecureCertificates = true
            };
            //options.AddArgument("-headless");
            options.AddArgument("-disable-extensions");
            options.AddArgument("-disable-dev-shm-usage");
            options.AddArgument("-no-sandbox");
            IWebDriver webDriver = new FirefoxDriver(options);
            var page = actor.GetAbility<LoginPage>();
            page.WebDriver = webDriver;
            page.WebDriver.Navigate().GoToUrl(page.Settings.BaseUrl);
            page.WebDriver.Manage().Window.Maximize();
            return page.WebDriver;
        }
    }
}
