using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using System;
using TechTalk.SpecFlow;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using TechTalk.SpecFlow.Assist;
using CMDB.Testing.Helpers;
using FluentAssertions;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public class CreateMonitorStepDefinitions : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private MonitorOverviewPage overviewPage;
        private CreateMonitorPage createPage;

        private readonly Random rnd = new();
        private int rndNr;
        helpers.Monitor monitor;
        string expectedlog;

        public CreateMonitorStepDefinitions(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }

        [Given(@"I want to create a monitor with the folowing details")]
        public void GivenIWantToCreateAMonitorWithTheFolowingDetails(Table table)
        {
            monitor = table.CreateInstance<helpers.Monitor>();
            createPage.SerialNumber = monitor.SerialNumber + rndNr.ToString();
            entity.AssetCategory category = context.GetAssetCategory("Monitor");
            string Vendor, Type, assetType;
            assetType = monitor.Type;
            Vendor = assetType.Split(" ")[0];
            Type = assetType.Split(" ")[1];
            rndNr = rnd.Next();
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.MonitorOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            createPage = overviewPage.New();
            createPage.TakeScreenShot("${ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Create");
            createPage.AssetTag = monitor.AssetTag + rndNr.ToString();
            entity.AssetType AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
            createPage.Type = AssetType.TypeID.ToString();
        }
        [When(@"I save the monitor")]
        public void WhenISaveTheMonitor()
        {
            createPage.Create();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }
        [Then(@"The monitor can be found")]
        public void ThenTheMonitorCanBeFound()
        {
            expectedlog = $"The Monitor with type {monitor.Type} is created by {admin.Account.UserID} in table screen";
            overviewPage.Search(monitor.AssetTag + rndNr.ToString());
            var detail = overviewPage.Detail();
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }
    }
}
