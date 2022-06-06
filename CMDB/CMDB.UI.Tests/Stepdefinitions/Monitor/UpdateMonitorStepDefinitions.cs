using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using System;
using TechTalk.SpecFlow;
using CMDB.Domain.Entities;
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
        private MonitorAssignIdentityPage assignPage;

        private readonly Random rnd = new();
        private int rndNr;
        Screen screen;
        Identity identity;
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
            string Vendor, Type, assetType;
            AssetType AssetType = new();
            if(field == "Type")
            {
                AssetCategory category = context.GetAssetCategory("Monitor");
                assetType = newValue;
                Vendor = assetType.Split(" ")[0];
                Type = assetType.Split(" ")[1];
                AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
            }
            updatePage = overviewPage.Update();
            updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Update");
            updatedField = field;
            
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
        [Given(@"There is an actives monitor existing")]
        public async Task GivenThereIsAnActivesMonitorExisting()
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
        [When(@"I deactivate the monotor with reason (.*)")]
        public void WhenIDeactivateTheMonotorWithReasonTest(string reason)
        {
            newValue = reason;
            var deativate = overviewPage.Deactivate();
            deativate.Reason = newValue;
            deativate.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReasonEntered");
            deativate.Delete();
            deativate.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deleted");
        }
        [Then(@"The monitor is deactivated")]
        public void ThenTheMonitorIsDeactivated()
        {
            expectedlog = $"The Monitor with type {screen.Type} in table screen is deleted due to {newValue} by {admin.Account.UserID}";
            overviewPage.Search(screen.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log is not matching");
        }

        [Given(@"There is an inactive monitor existing")]
        public async Task GivenThereIsAnInactiveMonitorExisting()
        {
            screen = await context.CreateMonitor(admin, false);
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
        [When(@"I activate the monitor")]
        public void WhenIActivateTheMonitor()
        {
            overviewPage.Activate();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
        [Then(@"The monitor is active")]
        public void ThenTheMonitorIsActive()
        {
            expectedlog = $"The Monitor with type {screen.Type} in table screen is activated by {admin.Account.UserID}";
            overviewPage.Search(screen.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log is not matching");
        }

        [When(@"I assign the montitor to the Identity")]
        public void WhenIAssignTheMontitorToTheIdentity()
        {
            identity = (Identity)TestData.Get("Identity");
            assignPage = overviewPage.AssignIdentity();
            assignPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignPage");
            assignPage.Title.Should().BeEquivalentTo("Assign Identity to Monitor", "Title should be correct");
            assignPage.SelectIdentity(identity);
            assignPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_IdentitySelected");
        }
        [When(@"I fill in the assign form for my montitor")]
        public void WhenIFillInTheAssignFormForMyMontitor()
        {
            var assignForm = assignPage.Assign();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignForm");
            assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            assignForm.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            assignForm.CreatePDF();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_PDFCreated");
        }
        [Then(@"The Identity is assigned to the montitor")]
        public void ThenTheIdentityIsAssignedToTheMontitor()
        {
            overviewPage.Search(screen.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Details");
            expectedlog = $"The Monitor with {screen.AssetTag} is assigned to Identity width name: {identity.Name} by {admin.Account.UserID} in table screen";
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log should match");
        }
    }
}
