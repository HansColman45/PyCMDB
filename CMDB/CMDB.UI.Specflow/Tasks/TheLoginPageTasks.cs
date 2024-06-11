using Bright.ScreenPlay.Actors;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using Bright.ScreenPlay.Abilities;
using Task = Bright.ScreenPlay.Tasks.Task;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Tasks
{
    public class TheLoginPageTasks : Task
    {
        public override void PerformAs(IPerformer actor)
        {
        }
        public static void OpenLoginPageAs(IPerformer actor)
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
        public static void LoginAs(IPerformer actor, string userName, string password)
        {
            var page = actor.GetAbility<LoginPage>();
            page.UserId = userName;
            page.Password = password;
            var mainPage = page.LogIn();
            actor.SetAbility(mainPage);
        }
    }
}
