using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Task = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Tasks
{
    public class OpenTheLoginPageTasks : Task
    {
        public override void PerformAs(IPerformer actor)
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
        }
    }
}
