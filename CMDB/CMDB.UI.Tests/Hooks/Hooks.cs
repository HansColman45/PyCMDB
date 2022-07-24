using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;
using CMDB.UI.Tests.Data;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using CMDB.Domain.Entities;
using CMDB.Testing.Helpers;
using CMDB.UI.Tests.Pages;
using System.Linq;
using System.Reflection;

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
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// This function will run before evry scenario
        /// </summary>
        /// <param name="context">The Scenario context</param>
        [BeforeScenario]
        public void BeforeScenario(ScenarioContext context, ScenarioData scenarioData)
        {
            log.Debug("Scenario {0} started", context.ScenarioInfo.Title);
            var options = new FirefoxOptions
            {
                AcceptInsecureCertificates = true
            };
            //options.AddArgument("-headless");
            options.AddArgument("-disable-extensions");
            options.AddArgument("-disable-dev-shm-usage");
            options.AddArgument("-no-sandbox");
            IWebDriver webDriver = new FirefoxDriver(options);
            scenarioData.Driver = webDriver;
            scenarioData.Driver.Manage().Window.Maximize();
        }
        /// <summary>
        /// This funtion will runn after evry scenario
        /// </summary>
        /// <param name="context"></param>
        [AfterScenario]
        public void AfterScenario(ScenarioContext context, ScenarioData scenarioData)
        {
            log.Debug("Scenario {0} stoped", context.ScenarioInfo.Title);
            scenarioData.Driver.Close();
            scenarioData.Driver.Quit();
            List<Process> processes = Process.GetProcesses().Where(p => p.ProcessName == "chromedriver" && p.ProcessName == "geckodriver").ToList();
            foreach (var proc in processes)
            {
                proc.Kill();
            }
        }
        /// <summary>
        /// This function will run before each Feature
        /// </summary>
        /// <param name="scenarioData">The data of the scenario</param>
        [BeforeFeature]
        public static async Task BeforeFeature(ScenarioData scenarioData)
        {
            List<Process> processes = Process.GetProcesses().Where(p => p.ProcessName == "chromedriver" && p.ProcessName == "geckodriver").ToList();
            foreach (var proc in processes)
            {
                proc.Kill();
            }
            scenarioData.Context = new DataContext();
            scenarioData.Admin = await scenarioData.Context.CreateNewAdmin();
        }
        /// <summary>
        /// This function will runn after each feature
        /// </summary>
        /// <param name="scenarioData">The data of the scenario</param>
        [AfterFeature]
        public static async Task AfterFeature(ScenarioData scenarioData)
        {
            try
            {
                var createdorupdated = await scenarioData.Context.DeleteAllCreatedOrUpdated(scenarioData.Admin);
            }
            catch (Exception ex)
            {
                log.Error($"DB exception {ex}");
            }
        }
        /// <summary>
        /// This function will run after each step
        /// </summary>
        /// <param name="scenarioData">The data of the scenario</param>
        /// <param name="context">The context of the scenario</param>
        [AfterStep]
        public void AfterStep(ScenarioData scenarioData, ScenarioContext context)
        {
            var result = context.StepContext.Status;
            if (result == ScenarioExecutionStatus.TestError)
            {
                log.Error("The scenario {0} on step {2} ended with {1}", context.ScenarioInfo.Title, result, context.CurrentScenarioBlock);
                ITakesScreenshot takesScreenshot = (ITakesScreenshot)scenarioData.Driver;
                var screenshot = takesScreenshot.GetScreenshot();
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");
                string fileName = $"{context.CurrentScenarioBlock}_Error_{DateTime.Now:yyyy-MM-dd'T'HH-mm-ss}.png";
                Directory.CreateDirectory(Path.Combine(path, @"../../../Screenshots/", context.ScenarioInfo.Title));
                string tempFileName = Path.Combine(path, @$"../../../Screenshots/{context.ScenarioInfo.Title}/", fileName);
                screenshot.SaveAsFile(tempFileName, ScreenshotImageFormat.Png);
                log.Debug("Screenshot saved: {0}", tempFileName);
                List<Process> processes = Process.GetProcesses().Where(p => p.ProcessName == "chromedriver" && p.ProcessName == "geckodriver").ToList();
                foreach (var proc in processes)
                {
                    proc.Kill();
                }
            }
        }
    }
}
