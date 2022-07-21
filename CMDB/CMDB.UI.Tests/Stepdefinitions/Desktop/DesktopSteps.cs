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
using FluentAssertions;

namespace CMDB.UI.Tests.Stepdefinitions.Desktop
{
    [Binding]
    public sealed class DesktopSteps : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private DesktopOverviewPage overviewPage;
        private CreateDesktopPage CreateDesktop;
        private DesktopAssignIdentityPage AssignPage;
        private DesktopReleaseIdentityPage releaseIdenity;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.Desktop desktop;
        private entity.Desktop Desktop;
        private entity.Identity Identity;
        string expectedlog, updatedField, newValue;
        public DesktopSteps(ScenarioData scenarioData, ScenarioContext context) : base(scenarioData, context)
        {
        }
        #region Create
        [Given(@"I want to create a new Desktop with these details")]
        public void GivenIWantToCreateANewDesktopWithTheseDetails(Table table)
        {
            desktop = table.CreateInstance<helpers.Desktop>();
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
            overviewPage = main.DesktopOverview();
            entity.AssetCategory category = context.GetAssetCategory("Desktop");
            string Vendor, Type, assetType;
            assetType = desktop.Type;
            Vendor = assetType.Split(" ")[0];
            Type = assetType.Split(" ")[1];
            entity.AssetType AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
            CreateDesktop = overviewPage.New();
            CreateDesktop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreateNew");
            CreateDesktop.AssetTag = desktop.AssetTag + rndNr.ToString();
            CreateDesktop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssetTag");
            CreateDesktop.SerialNumber = desktop.SerialNumber + rndNr.ToString();
            CreateDesktop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SerialNumber");
            CreateDesktop.RAM = desktop.RAM;
            CreateDesktop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_RAM");
            CreateDesktop.Type = AssetType.TypeID.ToString();
            CreateDesktop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
        }
        [When(@"I save the Desktop")]
        public void WhenISaveTheDesktop()
        {
            CreateDesktop.Create();
            CreateDesktop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }
        [Then(@"I can find the newly created Desktop back")]
        public void ThenICanFindTheNewlyCreatedDesktopBack()
        {
            expectedlog = $"The Desktop with type {desktop.Type} is created by {admin.Account.UserID} in table desktop";
            overviewPage.Search(desktop.AssetTag + rndNr.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog();
            Assert.Equal(log, expectedlog);
        }
        #endregion
        [Given(@"There is an Desktop existing")]
        public async Task GivenThereIsAnDesktopExisting()
        {
            Desktop = await context.CreateDesktop(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.DesktopOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(Desktop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        #region update
        [When(@"I update the (.*) with (.*) on my Desktop and I save")]
        public void WhenIUpdateTheSerialnumberWithOnMyDesktopAndISave(string field, string value)
        {
            rndNr = rnd.Next();
            var updatepage = overviewPage.Update();
            updatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Update");
            desktop = new()
            {
                AssetTag = updatepage.AssetTag,
                SerialNumber = updatepage.SerialNumber,
                Type = updatepage.Type,
                RAM = updatepage.RAM
            };
            updatedField = field;
            switch (field)
            {
                case "Serialnumber":
                    newValue = value + rndNr.ToString();
                    updatepage.SerialNumber = newValue;
                    updatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SerialNumber");
                    break;
                case "RAM":
                    newValue = value;
                    updatepage.RAM = newValue;
                    updatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_RAM");
                    break;
            }
            updatepage.Edit();
            updatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Updated");
        }
        [Then(@"The Desktop is saved")]
        public void ThenTheDesktopIsSaved()
        {
            overviewPage.Search(desktop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog();
            switch (updatedField)
            {
                case "Serialnumber":
                    expectedlog = $"The SerialNumber has been changed from {desktop.SerialNumber} to {newValue} by {admin.Account.UserID} in table desktop";
                    break;
                case "RAM":
                    var oldRam = context.GetRAM(desktop.RAM);
                    var newRam = context.GetRAM(newValue);
                    expectedlog = $"The RAM has been changed from {oldRam.Value} to {newRam.Value} by {admin.Account.UserID} in table desktop";
                    break;
            }
            Assert.Equal(log, expectedlog);
        }
        #endregion
        #region deactivate
        [Given(@"There is an active Desktop existing")]
        public async Task GivenThereIsAnActiveLaptopExisting()
        {
            Desktop = await context.CreateDesktop(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.DesktopOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(Desktop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I deactivate the Desktop with reason (.*)")]
        public void WhenIDeactivateTheLaptopWithReasonTest(string reason)
        {
            newValue = reason;
            var deactivatepage = overviewPage.Deactivate();
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivate");
            deactivatepage.Reason = reason;
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            deactivatepage.Delete();
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivated");
        }
        [Then(@"The desktop is deactivated")]
        public void ThenTheLaptopIsDeactivated()
        {
            overviewPage.Search(Desktop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog();
            expectedlog = $"The Desktop with type {Desktop.Type} is deleted due to {newValue} by {admin.Account.UserID} in table desktop";
            Assert.Equal(log, expectedlog);
        }
        #endregion
        #region Activate
        [Given(@"There is an inactive Desktop existing")]
        public async Task GivenThereIsAnInactiveLaptopExisting()
        {
            Desktop = await context.CreateDesktop(admin, false);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.DesktopOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(Desktop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I activate the Desktop")]
        public void WhenIActivateTheLaptop()
        {
            overviewPage.Activate();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
        [Then(@"The desktop is active")]
        public void ThenTheLaptopIsActive()
        {
            overviewPage.Search(Desktop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog();
            expectedlog = $"The Desktop with type {Desktop.Type} is activated by {admin.Account.UserID} in table desktop";
            Assert.Equal(log, expectedlog);
        }
        #endregion
        #region Assign identity
        [When(@"I assign the Desktop to the Identity")]
        public void WhenIAssignTheDesktopToTheIdentity()
        {
            Identity = (entity.Identity)TestData.Get("Identity");
            AssignPage = overviewPage.AssignIdentity();
            AssignPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignPage");
            AssignPage.Title.Should().BeEquivalentTo("Assign identity to Desktop", "Title should be correct");
            AssignPage.SelectIdentity(Identity);
            AssignPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_IdentitySelected");
        }
        [When(@"I fill in the assign form for my Desktop")]
        public void WhenIFillInTheAssignFormForMyDesktop()
        {
            var assignForm = AssignPage.Assign();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignForm");
            assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            assignForm.Employee.Should().BeEquivalentTo(Identity.Name, "The employee should be the name of the identity");
            assignForm.CreatePDF();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_PDFCreated");
        }
        [Then(@"The Identity is assigned to the Desktop")]
        public void ThenTheIdentityIsAssignedToTheDesktop()
        {
            overviewPage.Search(Desktop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Details");
            expectedlog = $"The Desktop with {Desktop.AssetTag} is assigned to Identity with name: {Identity.Name} by {admin.Account.UserID} in table desktop";
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log should match");
        }
        #endregion
        [Given(@"that Identity is assigned to my Desktop")]
        public async Task GivenThatIdentityIsAssignedToMyDesktop()
        {
            Identity = (entity.Identity)TestData.Get("Identity");
            await context.AssignIdentity2Device(admin, Desktop, Identity);
        }
        [When(@"I release that identity from my Desktop")]
        public void WhenIReleaseThatIdentityFromMyDesktop()
        {
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeatilPage");
            releaseIdenity = detail.ReleaseIdentity();
            releaseIdenity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeatilPage");
        }
        [When(@"I fill in the release form for my Desktop")]
        public void WhenIFillInTheReleaseFormForMyDesktop()
        {
            releaseIdenity.Title.Should().BeEquivalentTo("Release identity from Desktop", "Title should be correct");
            releaseIdenity.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            releaseIdenity.Employee.Should().BeEquivalentTo(Identity.Name, "The employee should be the name of the identity");
            releaseIdenity.CreatePDF();
            releaseIdenity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_PDFCreated");
        }
        [Then(@"The identity is released from my Desktop")]
        public void ThenTheIdentityIsReleasedFromMyDesktop()
        {
            overviewPage.Search(Desktop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Details");
            expectedlog = $"The Desktop with {Desktop.AssetTag} is released from Identity with name: {Identity.Name} by {admin.Account.UserID} in table desktop";
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log should match");
        }

    }
}
