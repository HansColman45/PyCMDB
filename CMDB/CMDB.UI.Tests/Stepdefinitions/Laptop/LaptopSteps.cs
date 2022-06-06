using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System;
using Xunit;
using System.Threading.Tasks;
using CMDB.Testing.Helpers;
using FluentAssertions;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public sealed class LaptopSteps : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private LaptopOverviewPage overviewPage;
        private CreateLaptopPage CreateLaptop;
        private LaptopAssignIdentityPage assignLaptop;
        private AssignFormPage assignForm;
        private LaptopReleaseIdentityPage releaseIdenity;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.Laptop laptop;
        private entity.Laptop Laptop;
        private entity.Identity Identity;
        string expectedlog, updatedField, newValue;
        public LaptopSteps(ScenarioData scenarioData, ScenarioContext context) : base(scenarioData, context)
        {
        }
        #region Create new
        [Given(@"I want to create a new Laptop with these details")]
        public void GivenIWantToCreateANewLaptopWithTheseDetails(Table table)
        {
            laptop = table.CreateInstance<helpers.Laptop>();
            rndNr = rnd.Next();
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Begin");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterUserId");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPassword");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.LaptopOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            entity.AssetCategory category = context.GetAssetCategory("Laptop");
            string Vendor, Type, assetType;
            assetType = laptop.Type;
            Vendor = assetType.Split(" ")[0];
            Type = assetType.Split(" ")[1];
            entity.AssetType AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
            CreateLaptop = overviewPage.New();
            CreateLaptop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_NewLaptop");
            CreateLaptop.AssetTag = laptop.AssetTag + rndNr.ToString();
            CreateLaptop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssetTag");
            CreateLaptop.SerialNumber = laptop.SerialNumber + rndNr.ToString();
            CreateLaptop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SerialNumber");
            CreateLaptop.RAM = laptop.RAM;
            CreateLaptop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Ram");
            CreateLaptop.Type = AssetType.TypeID.ToString();
            CreateLaptop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
        }
        [When(@"I save the Laptop")]
        public void WhenISaveTheLaptop()
        {
            CreateLaptop.Create();
            CreateLaptop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }
        [Then(@"I can find the newly created Laptop back")]
        public void ThenICanFindTheNewlyCreatedLaptopBack()
        {
            expectedlog = $"The Laptop with type {laptop.Type} is created by {admin.Account.UserID} in table laptop";
            overviewPage.Search(laptop.AssetTag + rndNr.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log should match");
        }
        #endregion
        [Given(@"There is an Laptop existing")]
        public async Task GivenThereIsAnLaptopExisting()
        {
            Laptop = await context.CreateLaptop(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Begin");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterUserId");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPassword");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.LaptopOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            overviewPage.Search(Laptop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        #region update 
        [When(@"I update the (.*) with (.*) on my Laptop and I save")]
        public void WhenIUpdateTheSerialnumberWithAndISave(string field, string value)
        {
            rndNr = rnd.Next();
            var updatepage = overviewPage.Update();
            updatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_UpdatePage");
            laptop = new()
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
                    updatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Ram");
                    break;
            }
            updatepage.Edit();
            updatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Updated");
        }
        [Then(@"The Laptop is saved")]
        public void ThenTheLaptopIsSaved()
        {
            overviewPage.Search(laptop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog();
            switch (updatedField)
            {
                case "Serialnumber":
                    expectedlog = $"The SerialNumber in table laptop has been changed from {laptop.SerialNumber} to {newValue} by {admin.Account.UserID}";
                    break;
                case "RAM":
                    var oldRam = context.GetRAM(laptop.RAM);
                    var newRam = context.GetRAM(newValue);
                    expectedlog = $"The RAM in table laptop has been changed from {oldRam.Value} to {newRam.Value} by {admin.Account.UserID}";
                    break;
            }
            log.Should().BeEquivalentTo(expectedlog, "Log should match");
            Assert.Equal(log, expectedlog);
        }
        #endregion
        #region Deactivate
        [Given(@"There is an active Laptop existing")]
        public async Task GivenThereIsAnActiveLaptopExisting()
        {
            Laptop = await context.CreateLaptop(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Begin");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterUserId");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPassword");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.LaptopOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            overviewPage.Search(Laptop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I deactivate the Laptop with reason (.*)")]
        public void WhenIDeactivateTheLaptopWithReasonTest(string reason)
        {
            newValue = reason;
            var deactivatepage = overviewPage.Deactivate();
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeactivatePage");
            deactivatepage.Reason = reason;
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            deactivatepage.Delete();
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivated");
        }
        [Then(@"The laptop is deactivated")]
        public void ThenTheLaptopIsDeactivated()
        {
            overviewPage.Search(Laptop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeatilPage");
            string log = detail.GetLastLog();
            expectedlog = $"The Laptop with type {Laptop.Type} in table laptop is deleted due to {newValue} by {admin.Account.UserID}";
            Assert.Equal(log, expectedlog);
        }
        #endregion
        #region Activate
        [Given(@"There is an inactive Laptop existing")]
        public async Task GivenThereIsAnInactiveLaptopExisting()
        {
            Laptop = await context.CreateLaptop(admin, false);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Begin");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterUserId");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPassword");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.LaptopOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            overviewPage.Search(Laptop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I activate the Laptop")]
        public void WhenIActivateTheLaptop()
        {
            overviewPage.Activate();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
        [Then(@"The laptop is active")]
        public void ThenTheLaptopIsActive()
        {
            overviewPage.Search(Laptop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeatilPage");
            string log = detail.GetLastLog();
            expectedlog = $"The Laptop with type {Laptop.Type} in table laptop is activated by {admin.Account.UserID}";
            log.Should().BeEquivalentTo(expectedlog, "Log should match");
        }
        #endregion
        [Given(@"an Identity exist as well")]
        public async Task GivenAnIdentyExistAsWell()
        {
            Identity = await context.CreateIdentity(admin);
            TestData.Add("Identity", Identity);
        }
        #region Assign2Identity
        [When(@"I assign the Laptop to the Identity")]
        public void WhenIAssignTheLaptopToTheIdentity()
        {
            assignLaptop = overviewPage.AssignIdentity();
            assignLaptop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignIdentityPage");
            assignLaptop.Title.Should().BeEquivalentTo("Assign identity to Laptop", "Title should be correct");
            assignLaptop.SelectIdentity(Identity);
            assignLaptop.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedIdentity");
            assignForm = assignLaptop.Assign();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Assigned");
        }
        [When(@"I fill in the assign form for my Laptop")]
        public void WhenIFillInTheAssignFormForMyLaptop()
        {
            assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID,"The IT employee should be the admin");
            assignForm.Employee.Should().BeEquivalentTo(Identity.Name, "The employee should be the name of the identity");
            assignForm.CreatePDF();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_PDFCreated");
        }
        [Then(@"The Identity is assigned to the Laptop")]
        public void ThenTheIdentityIsAssignedToTheLaptop()
        {
            overviewPage.Search(Laptop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Details");
            expectedlog = $"The Laptop with {Laptop.AssetTag} is assigned to Identity with name: {Identity.Name} by {admin.Account.UserID} in table laptop";
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log should match");
        }
        #endregion
        #region ReleaseIdentity
        [Given(@"that Identity is assigned to my laptop")]
        public async Task GivenThatIdentityIsAssignedToMyLaptop()
        {
            await context.AssignIdentity2Device(admin,Laptop,Identity);
        }
        [When(@"I release that identity")]
        public void WhenIReleaseThatIdentity()
        {
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeatilPage");
            releaseIdenity = detail.ReleaseIdentity();
            releaseIdenity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeatilPage");
        }
        [When(@"I fill in the release form for my laptop")]
        public void WhenIFillInTheReleaseFormForMyLaptop()
        {
            releaseIdenity.Title.Should().BeEquivalentTo("Release identity from Laptop", "Title should be correct");
            releaseIdenity.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            releaseIdenity.Employee.Should().BeEquivalentTo(Identity.Name, "The employee should be the name of the identity");
            releaseIdenity.CreatePDF();
            releaseIdenity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_PDFCreated");
        }
        [Then(@"The identity is released from my laptop")]
        public void ThenTheIdentityIsReleasedFromMyLaptop()
        {
            overviewPage.Search(Laptop.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Details");
            expectedlog = $"The Laptop with {Laptop.AssetTag} is released from Identity with name: {Identity.Name} by {admin.Account.UserID} in table laptop";
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log should match");
        }
        #endregion
    }
}
