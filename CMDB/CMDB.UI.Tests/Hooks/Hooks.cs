using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;
using CMDB.UI.Tests.Data;

namespace CMDB.UI.Tests.Hooks
{
    /// <summary>
    /// This is the hooks class For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
    /// </summary>
    [Binding]
    public sealed class Hooks
    {
        /// <summary>
        /// The Nlog logger
        /// </summary>
        private readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// This function will run before evry scenario
        /// </summary>
        /// <param name="context">The Scenario context</param>
        [BeforeScenario]
        public void BeforeScenario(ScenarioContext context)
        {
            log.Debug("Scenario {0} started", context.ScenarioInfo.Title);
        }
        /// <summary>
        /// This funtion will runn after evry scenario
        /// </summary>
        /// <param name="context"></param>
        [AfterScenario]
        public void AfterScenario(ScenarioContext context)
        {
            log.Debug("Scenario {0} stoped", context.ScenarioInfo.Title);
        }
        /// <summary>
        /// This function will run before each Feature
        /// </summary>
        /// <param name="scenarioData">The data of the scenario</param>
        [BeforeFeature]
        public static void BeforeFeature(ScenarioData scenarioData)
        {
            scenarioData.Context = new DataContext();
            scenarioData.Admin = scenarioData.Context.CreateNewAdmin();
            /*var options = new FirefoxOptions
            {
                AcceptInsecureCertificates = true
            };
            //options.AddArgument("-headless");
            IWebDriver webDriver = new FirefoxDriver(options);*/
            var options = new ChromeOptions()
            {
                AcceptInsecureCertificates = true
            };
            IWebDriver webDriver = new ChromeDriver(options);
            scenarioData.Driver = webDriver;
            scenarioData.Driver.Manage().Window.Maximize();
        }
        /// <summary>
        /// This function will runn after each feature
        /// </summary>
        /// <param name="scenarioData">The data of the scenario</param>
        [AfterFeature]
        public static void AfterFeature(ScenarioData scenarioData)
        {
            scenarioData.Driver.Close();
            scenarioData.Driver.Quit();
            scenarioData.Context = null;
        }
    }
}
