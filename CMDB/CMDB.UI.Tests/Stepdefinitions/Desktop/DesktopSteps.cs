using CMDB.UI.Tests.Hooks;
using TechTalk.SpecFlow;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using CMDB.UI.Tests.Pages;
using TechTalk.SpecFlow.Assist;
using System;
using Xunit;

namespace CMDB.UI.Tests.Stepdefinitions.Desktop
{
    [Binding]
    public sealed class DesktopSteps : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private DesktopOverviewPage overviewPage;
        private CreateDesktopPage CreateDesktop;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.Desktop desktop;
        private entity.Desktop Desktop;
        string expectedlog, updatedField, newValue;
        public DesktopSteps(ScenarioData scenarioData) : base(scenarioData)
        {
        }
        [Given(@"I want to create a new Desktop with these details")]
        public void GivenIWantToCreateANewDesktopWithTheseDetails(Table table)
        {
            desktop = table.CreateInstance<helpers.Desktop>();
            rndNr = rnd.Next();
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.DesktopOverview();
            entity.AssetCategory category = context.GetAssetCategory("Desktop");
            string Vendor, Type, assetType;
            assetType = desktop.Type;
            Vendor = assetType.Split(" ")[0];
            Type = assetType.Split(" ")[1];
            entity.AssetType AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
            CreateDesktop = overviewPage.New();
            CreateDesktop.AssetTag = desktop.AssetTag + rndNr.ToString();
            CreateDesktop.SerialNumber = desktop.SerialNumber + rndNr.ToString();
            CreateDesktop.RAM = desktop.RAM;
            CreateDesktop.Type = AssetType.TypeID.ToString();
        }
        [When(@"I save the Desktop")]
        public void WhenISaveTheDesktop()
        {
            CreateDesktop.Create();
        }
        [Then(@"I can find the newly created Desktop back")]
        public void ThenICanFindTheNewlyCreatedDesktopBack()
        {
            expectedlog = $"The Desktop with type {desktop.Type} is created by {admin.Account.UserID} in table desktop";
            overviewPage.Search(desktop.AssetTag + rndNr.ToString());
            var detail = overviewPage.Detail();
            string log = detail.GetLastLog();
            Assert.Equal(log, expectedlog);
        }

    }
}
