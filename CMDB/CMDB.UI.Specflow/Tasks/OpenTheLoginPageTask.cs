using Bright.ScreenPlay.Actors;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using Task = Bright.ScreenPlay.Tasks.Task;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Tasks
{
    public class OpenTheLoginPageTask : Task
    {
        public override void PerformAs(IPerformer actor)
        {
        }
        public static LoginPage OpenLoginPageAs(IPerformer actor)
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
            return actor.GetAbility<LoginPage>().OpenLoginPage(webDriver);
        }
    }
}
