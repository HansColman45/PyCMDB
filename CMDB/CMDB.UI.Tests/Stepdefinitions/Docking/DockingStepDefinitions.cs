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
using FluentAssertions;

namespace CMDB.UI.Tests.Stepdefinitions.Docking
{
    [Binding]
    public class DockingStepDefinitions : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private DockingOverviewPage overviewPage;
        private CreateDockingPage CreatePage;
        private DockingAssignIdentityPage AssignPage;
        private DockingReleaseIdentityPage releaseIdenity;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.DockingStation dockingStation;
        private entity.Docking Docking;
        private entity.Identity Identity;
        string expectedlog, updatedField, newValue;
        public DockingStepDefinitions(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }
        [Given(@"I want to create a new Dockingstation with these details")]
        public void GivenIWantToCreateANewDockingstationWithTheseDetails(Table table)
        {
            dockingStation = table.CreateInstance<helpers.DockingStation>();
            entity.AssetCategory category = context.GetAssetCategory("Docking station");
            string Vendor, Type, assetType;
            assetType = dockingStation.Type;
            Vendor = assetType.Split(" ")[0];
            Type = assetType.Split(" ")[1];
            entity.AssetType AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
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

        [Given(@"There is an Docking existing")]
        public async Task GivenThereIsAnDockingExisting()
        {
            Docking = await context.CreateDocking(admin);
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
            overviewPage.Search(Docking.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
        }
        [When(@"I update the (.*) with (.*) on my Doking and I save")]
        public void WhenIUpdateTheFieldWithOnMyDokingAndISave(string field, string value)
        {
            updatedField = field;
            rndNr = rnd.Next();
            entity.AssetCategory category = context.GetAssetCategory("Docking station");
            string Vendor, Type, assetType;
            entity.AssetType AssetType = new();
            if (updatedField == "Type")
            {
                assetType = value;
                Vendor = assetType.Split(" ")[0];
                Type = assetType.Split(" ")[1];
                AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
            }
            var updatePage = overviewPage.Update();
            switch (updatedField)
            {
                case "SerialNumber":
                    updatePage.SerialNumber = value + rndNr.ToString();
                    newValue = value + rndNr.ToString();
                    updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterSerial");
                    break;
                case "Type":
                    newValue = AssetType.ToString();
                    updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterSerial");
                    updatePage.Type = AssetType.TypeID.ToString();
                    break;
            }
            updatePage.Edit();
            updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Edited");
        }
        [Then(@"Then The Docking is saved")]
        public void ThenThenTheDockingIsSaved()
        {
            overviewPage.Search(Docking.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            switch (updatedField)
            {
                case "SerialNumber":
                    expectedlog = $"The SerialNumber in table docking has been changed from {Docking.SerialNumber} to {newValue} by {admin.Account.UserID}";
                    break;
                case "Type":
                    expectedlog = $"The Type in table docking has been changed from {Docking.Type} to {newValue} by {admin.Account.UserID}";
                    break;
            }
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            string log = detail.GetLastLog();
            Assert.Equal(log, expectedlog);
        }
        [Given(@"There is an active Docking existing")]
        public async Task GivenThereIsAnActiveDockingExisting()
        {
            Docking = await context.CreateDocking(admin);
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
            overviewPage.Search(Docking.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
        }
        [When(@"I deactivate the Docking with reason (.*)")]
        public void WhenIDeactivateTheDockingWithReasonTest(string reason)
        {
            var deactivatepage = overviewPage.Deactivate();
            deactivatepage.Reason = reason;
            newValue = reason;
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterReason");
            deactivatepage.Delete();
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivated");
        }
        [Then(@"The Docking is deactivated")]
        public void ThenTheDockingIsDeactivated()
        {
            expectedlog = $"The Docking station with type {Docking.Type} in table docking is deleted due to {newValue} by {admin.Account.UserID}";
            overviewPage.Search(Docking.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log is not matching");
        }

        [Given(@"There is an inactve Docking existing")]
        public async Task GivenThereIsAnInactveDockingExisting()
        {
            Docking = await context.CreateDocking(admin,false);
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
            overviewPage.Search(Docking.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
        }
        [When(@"I activate the docking station")]
        public void WhenIActivateTheDockingStation()
        {
            overviewPage.Activate();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
        [Then(@"The docking station is activated")]
        public void ThenTheDockingStationIsActivated()
        {
            expectedlog = $"The Docking station with type {Docking.Type} in table docking is activated by {admin.Account.UserID}";
            overviewPage.Search(Docking.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            string log = detail.GetLastLog();
            Assert.Equal(log, expectedlog);
        }

        [When(@"I assign the Docking to the Identity")]
        public void WhenIAssignTheDockingToTheIdentity()
        {
            Identity = (entity.Identity)TestData.Get("Identity");
            AssignPage = overviewPage.AssignIdentity();
            AssignPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignPage");
            AssignPage.Title.Should().BeEquivalentTo("Assign identity to docking", "Title should be correct");
            AssignPage.SelectIdentity(Identity);
            AssignPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_IdentitySelected");
        }

        [When(@"I fill in the assign form for my Docking")]
        public void WhenIFillInTheAssignFormForMyDocking()
        {
            var assignForm = AssignPage.Assign();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignForm");
            assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            assignForm.Employee.Should().BeEquivalentTo(Identity.Name, "The employee should be the name of the identity");
            assignForm.CreatePDF();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_PDFCreated");
        }

        [Then(@"The Identity is assigned to the Docking")]
        public void ThenTheIdentityIsAssignedToTheDocking()
        {
            overviewPage.Search(Docking.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Details");
            expectedlog = $"The Docking station with {Docking.AssetTag} is assigned to Identity width name: {Identity.Name} by {admin.Account.UserID} in table docking";
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log should match");
        }

        [Given(@"that Identity is assigned to my Docking")]
        public async Task GivenThatIdentityIsAssignedToMyDocking()
        {
            Identity = (entity.Identity)TestData.Get("Identity");
            await context.AssignIdentity2Device(admin, Docking, Identity);
        }
        [When(@"I release that identity from my Docking")]
        public void WhenIReleaseThatIdentityFromMyDocking()
        {
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeatilPage");
            releaseIdenity = detail.ReleaseIdentity();
            releaseIdenity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeatilPage");
        }
        [When(@"I fill in the release form for my Docking")]
        public void WhenIFillInTheReleaseFormForMyDocking()
        {
            releaseIdenity.Title.Should().BeEquivalentTo("Release identity from Docking", "Title should be correct");
            releaseIdenity.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            releaseIdenity.Employee.Should().BeEquivalentTo(Identity.Name, "The employee should be the name of the identity");
            releaseIdenity.CreatePDF();
            releaseIdenity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_PDFCreated");
        }
        [Then(@"The identity is released from my Docking")]
        public void ThenTheIdentityIsReleasedFromMyDocking()
        {
            overviewPage.Search(Docking.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Details");
            expectedlog = $"The Docking station with {Docking.AssetTag} is released from Identity with name: {Identity.Name} by {admin.Account.UserID} in table docking";
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log should match");
        }

    }
}
