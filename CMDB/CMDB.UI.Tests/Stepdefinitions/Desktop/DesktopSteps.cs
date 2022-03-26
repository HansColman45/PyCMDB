using CMDB.UI.Tests.Hooks;
using TechTalk.SpecFlow;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using CMDB.UI.Tests.Pages;
using TechTalk.SpecFlow.Assist;
using System;
using Xunit;
using System.Threading.Tasks;
using CMDB.Testing.Helpers;

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
        public DesktopSteps(ScenarioData scenarioData, ScenarioContext context) : base(scenarioData, context)
        {
        }
        [Given(@"I want to create a new Desktop with these details")]
        public void GivenIWantToCreateANewDesktopWithTheseDetails(Table table)
        {
            desktop = table.CreateInstance<helpers.Desktop>();
            rndNr = rnd.Next();
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
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

        [Given(@"There is an Desktop existing")]
        public async Task GivenThereIsAnDesktopExisting()
        {
            Desktop = await context.CreateDesktop(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.DesktopOverview();
            overviewPage.Search(Desktop.AssetTag);
        }
        [When(@"I update the (.*) with (.*) on my Desktop and I save")]
        public void WhenIUpdateTheSerialnumberWithOnMyDesktopAndISave(string field, string value)
        {
            rndNr = rnd.Next();
            var updatepage = overviewPage.Update();
            desktop = new()
            {
                AssetTag = updatepage.AssetTag,
                SerialNumber = updatepage.SerialNumber,
                Type = updatepage.Type,
                RAM = updatepage.RAM
            };
            updatedField = field;
            updatedField = field;
            switch (field)
            {
                case "Serialnumber":
                    newValue = value + rndNr.ToString();
                    updatepage.SerialNumber = newValue;
                    break;
                case "RAM":
                    newValue = value;
                    updatepage.RAM = newValue;
                    break;
            }
            updatepage.Edit();
        }
        [Then(@"The Desktop is saved")]
        public void ThenTheDesktopIsSaved()
        {
            overviewPage.Search(desktop.AssetTag);
            var detail = overviewPage.Detail();
            string log = detail.GetLastLog();
            switch (updatedField)
            {
                case "Serialnumber":
                    expectedlog = $"The SerialNumber in table desktop has been changed from {desktop.SerialNumber} to {newValue} by {admin.Account.UserID}";
                    break;
                case "RAM":
                    var oldRam = context.GetRAM(desktop.RAM);
                    var newRam = context.GetRAM(newValue);
                    expectedlog = $"The RAM in table desktop has been changed from {oldRam.Value} to {newRam.Value} by {admin.Account.UserID}";
                    break;
            }
            Assert.Equal(log, expectedlog);
        }

        [Given(@"There is an active Desktop existing")]
        public async Task GivenThereIsAnActiveLaptopExisting()
        {
            Desktop = await context.CreateDesktop(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.DesktopOverview();
            overviewPage.Search(Desktop.AssetTag);
        }
        [When(@"I deactivate the Desktop with reason (.*)")]
        public void WhenIDeactivateTheLaptopWithReasonTest(string reason)
        {
            newValue = reason;
            var deactivatepage = overviewPage.Deactivate();
            deactivatepage.Reason = reason;
            deactivatepage.Delete();
        }
        [Then(@"The desktop is deactivated")]
        public void ThenTheLaptopIsDeactivated()
        {
            overviewPage.Search(Desktop.AssetTag);
            var detail = overviewPage.Detail();
            string log = detail.GetLastLog();
            expectedlog = $"The Desktop with type {Desktop.Type} in table desktop is deleted due to {newValue} by {admin.Account.UserID}";
            Assert.Equal(log, expectedlog);
        }

        [Given(@"There is an inactive Desktop existing")]
        public async Task GivenThereIsAnInactiveLaptopExisting()
        {
            Desktop = await context.CreateDesktop(admin, false);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.DesktopOverview();
            overviewPage.Search(Desktop.AssetTag);
        }
        [When(@"I activate the Desktop")]
        public void WhenIActivateTheLaptop()
        {
            overviewPage.Activate();
        }
        [Then(@"The desktop is active")]
        public void ThenTheLaptopIsActive()
        {
            overviewPage.Search(Desktop.AssetTag);
            var detail = overviewPage.Detail();
            string log = detail.GetLastLog();
            expectedlog = $"The Desktop with type {Desktop.Type} in table desktop is activated by {admin.Account.UserID}";
            Assert.Equal(log, expectedlog);
        }
    }
}
