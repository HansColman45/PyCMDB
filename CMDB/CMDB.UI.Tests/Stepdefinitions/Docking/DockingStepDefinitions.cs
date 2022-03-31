using CMDB.UI.Tests.Hooks;
using TechTalk.SpecFlow;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using CMDB.UI.Tests.Pages;
using System;
using CMDB.Testing.Helpers;
using TechTalk.SpecFlow.Assist;
using Xunit;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Stepdefinitions.Docking
{
    [Binding]
    public class DockingStepDefinitions : TestBase
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
        public DockingStepDefinitions(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }
        [Given(@"I want to create a new Dockingstation with these details")]
        public void GivenIWantToCreateANewDockingstationWithTheseDetails(Table table)
        {
            dockingStation = table.CreateInstance<helpers.DockingStation>();
            rndNr = rnd.Next();
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            if (Settings.TakeScreenShot)
                main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.DockingStationOverview();
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DockingOverview");
            CreatePage = overviewPage.New();
            if (Settings.TakeScreenShot)
                CreatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreateDocking");
            CreatePage.AssetTag = dockingStation.AssetTag + rndNr.ToString();
            if (Settings.TakeScreenShot)
                CreatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SetAssetTag");
            entity.AssetCategory category = context.GetAssetCategory("Docking station");
            string Vendor, Type, assetType;
            assetType= dockingStation.Type;
            Vendor = assetType.Split(" ")[0];
            Type = assetType.Split(" ")[1];
            entity.AssetType AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
            CreatePage.Type = AssetType.TypeID.ToString();
            if (Settings.TakeScreenShot)
                CreatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SetType");
            CreatePage.SerialNumber = dockingStation.SerialNumber + rndNr.ToString();
            if (Settings.TakeScreenShot)
                CreatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SetSerial");
        }
        [When(@"I save the Dockingstion")]
        public void WhenISaveTheDockingstion()
        {
            CreatePage.Create();
            if (Settings.TakeScreenShot)
                CreatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }
        [Then(@"I can find the newly created Docking station")]
        public void ThenICanFindTheNewlyCreatedDockingStation()
        {
            expectedlog = $"The Docking station with type {dockingStation.Type} is created by {admin.Account.UserID} in table docking";
            overviewPage.Search(dockingStation.AssetTag + rndNr.ToString());
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            if (Settings.TakeScreenShot)
                detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            string log = detail.GetLastLog();
            Assert.Equal(log, expectedlog);
        }

        [Given(@"There is an Docking existing")]
        public async Task GivenThereIsAnDockingExisting()
        {
            Docking = await context.CreateDocking(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            if (Settings.TakeScreenShot)
                main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.DockingStationOverview();
            overviewPage.Search(Docking.AssetTag);
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
        }
        [When(@"I update the (.*) with (.*) on my Doking and I save")]
        public void WhenIUpdateTheFieldWithOnMyDokingAndISave(string field, string value)
        {
            updatedField = field;
            rndNr = rnd.Next();
            switch (updatedField)
            {
                case "SerialNumber":
                    break;
                case "Type":
                    break;
            }
        }
        [Then(@"Then The Docking is saved")]
        public void ThenThenTheDockingIsSaved()
        {

        }


    }
}
