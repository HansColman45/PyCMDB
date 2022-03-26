using CMDB.UI.Tests.Hooks;
using TechTalk.SpecFlow;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using CMDB.UI.Tests.Pages;
using System;
using CMDB.Testing.Helpers;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace CMDB.UI.Tests.Stepdefinitions.Docking
{
    [Binding]
    public class DockingSteps : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private DockingOverviewPage overviewPage;
        private CreateDockingPage CreatePage;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.DockingStation dockingStation;
        private entity.Docking Docking;
        string expectedlog, updatedField, newValue;
        public DockingSteps(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }
        [Given(@"I want to create a new Dockingstation with these details")]
        public void GivenIWantToCreateANewDockingstationWithTheseDetails(Table table)
        {
            dockingStation = table.CreateInstance<helpers.DockingStation>();
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
            overviewPage = main.DockingStationOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DockingOverview");
            CreatePage = overviewPage.New();
            CreatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreateDocking");
            CreatePage.AssetTag = dockingStation.AssetTag + rndNr.ToString();
            CreatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SetAssetTag");
            entity.AssetCategory category = context.GetAssetCategory("Docking station");
            string Vendor, Type, assetType;
            assetType= dockingStation.Type;
            Vendor = assetType.Split(" ")[0];
            Type = assetType.Split(" ")[1];
            entity.AssetType AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
            CreatePage.Type = AssetType.TypeID.ToString();
            CreatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SetType");
            CreatePage.SerialNumber = dockingStation.SerialNumber + rndNr.ToString();
            CreatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SetSerial");
        }
        [When(@"I save the Dockingstion")]
        public void WhenISaveTheDockingstion()
        {
            CreatePage.Create();
            CreatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }
        [Then(@"I can find the newly created Docking station")]
        public void ThenICanFindTheNewlyCreatedDockingStation()
        {
            expectedlog = $"The Docking station with type {dockingStation.Type} is created by {admin.Account.UserID} in table docking";
            overviewPage.Search(dockingStation.AssetTag + rndNr.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            string log = detail.GetLastLog();
            Assert.Equal(log, expectedlog);
        }

    }
}
