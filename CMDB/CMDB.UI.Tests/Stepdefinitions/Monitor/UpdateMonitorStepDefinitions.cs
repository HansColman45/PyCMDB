using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using System;
using TechTalk.SpecFlow;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using FluentAssertions;
using System.Threading.Tasks;
using CMDB.Testing.Helpers;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public class UpdateMonitorStepDefinitions : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private MonitorOverviewPage overviewPage;
        private UpdateMonitorPage updatePage;

        private readonly Random rnd = new();
        private int rndNr;
        helpers.Monitor monitor;
        entity.Screen screen;
        string expectedlog, updatedField, newValue;

        public UpdateMonitorStepDefinitions(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }

        [Given(@"There is an monitor existing")]
        public async Task GivenTHereIsAnMonitorExisting()
        {
            screen = await context.CreateMonitor(admin);
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
            overviewPage.Search(screen.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I update the (.*) with (.*) on my monitor and I save")]
        public void WhenIUpdateTheSerialNumberWithOnMyMonitorAndISave(string field, string newValue)
        {
            updatePage = overviewPage.Update();
            updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Update");
            updatedField = field;
            string Vendor, Type, assetType;
            entity.AssetCategory category = context.GetAssetCategory("Monitor");
            assetType = newValue;
            Vendor = assetType.Split(" ")[0];
            Type = assetType.Split(" ")[1];
            entity.AssetType AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
            switch (field)
            {
                case "SerialNumber":
                    newValue += rndNr.ToString();
                    updatePage.SerialNumber = newValue;
                    this.newValue = newValue;
                    updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SerailUpdated");
                    break;
                case "Type":                    
                    updatePage.Type = AssetType.TypeID.ToString();
                    this.newValue = AssetType.ToString();
                    updatePage.Type = AssetType.TypeID.ToString();
                    updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_TypeUpdated");
                    break;
            }
            updatePage.Edit();
            updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_updated");
        }
        [Then(@"Then the monitor is saved")]
        public void ThenThenTheMonitorIsSaved()
        {
            switch (updatedField)
            {
                case "SerialNumber":
                    expectedlog = $"The {updatedField} in table screen has been changed from {screen.SerialNumber} to {newValue} by {admin.Account.UserID}";
                    break;
                case "Type":
                    expectedlog = $"The {updatedField} in table screen has been changed from {screen.Type} to {newValue} by {admin.Account.UserID}";
                    break;
            }
            overviewPage.Search(screen.AssetTag);
            var detail = overviewPage.Detail();
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog,"Log is not as expected");
        }
    }
}
