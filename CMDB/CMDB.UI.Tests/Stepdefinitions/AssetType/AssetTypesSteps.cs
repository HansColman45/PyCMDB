using CMDB.Domain.Entities;
using CMDB.Testing.Helpers;
using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Extensions.Ordering;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public sealed class AssetTypesSteps : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private AssetTypeOverviewPage overviewPage;
        private CreateAssetTypePage createAssetTypePage;

        private readonly Random rnd = new();
        private int rndNr;
        private string vendor, type, newtype, reason;
        private AssetType assetType;
        public AssetTypesSteps(ScenarioData scenarioData, ScenarioContext context) : base(scenarioData, context)
        {
        }
        [Order(1)]
        [Given(@"I want to create a new (.*) with (.*) and (.*)")]
        public void GivenIWantToCreateANewKensingtonWithKensingtonAndBlack(string category, string vendor, string type)
        {
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
            this.vendor = vendor + rndNr.ToString();
            this.type = type + rndNr.ToString();
            overviewPage = main.AssetTypeOverview();
            createAssetTypePage = overviewPage.New();
            createAssetTypePage.Category = category;
            createAssetTypePage.Vendor = this.vendor;
            createAssetTypePage.Type = this.type;
        }
        [Order(2)]
        [When(@"I create that (.*)")]
        public void WhenICreateThatKensington(string category)
        {
            log.Debug($"Gooing to create a {category} type");
            createAssetTypePage.Create();
        }
        [Order(3)]
        [Then(@"The (.*) is created")]
        public void ThenTheIsCreated(string category)
        {
            overviewPage.Search(vendor);
            string expectedlog = $"The {category} type Vendor: {vendor} and type {type} is created by {admin.Account.UserID} in table assettype";
            var detail = overviewPage.Detail();
            var log = detail.GetLastLog();
            expectedlog.Should().BeEquivalentTo(log);
        }
        [Order(4)]
        [Given(@"There is an AssetType existing")]
        public async Task GivenThereIsAnAssetTypeExisting()
        {
            assetType = await context.CreateAssetType(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.AssetTypeOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(assetType.Vendor);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
        }
        [Order(5)]
        [When(@"I change the Type and save the changes")]
        public void WhenIChangeTheTypeAndSaveTheChanges()
        {
            rndNr = rnd.Next();
            newtype = "Orange" + rndNr.ToString();
            var editPage = overviewPage.Edit();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Edit");
            type = editPage.Type;
            vendor = editPage.Vendor;
            editPage.Type = newtype;
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
            editPage.Edit();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Edited");
        }
        [Order(6)]
        [Then(@"The changes are saved")]
        public void ThenTheChangesAreSaved()
        {
            overviewPage.Search(assetType.Vendor);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            string expectedlog = $"The Type has been changed from {type} to {newtype} by {admin.Account.UserID} in table assettype";
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            var log = detail.GetLastLog();
            expectedlog.Should().BeEquivalentTo(log);
        }
        [Order(7)]
        [Given(@"There is an active AssetType existing")]
        public async Task GivenThereIsAnActiveAssetTypeExisting()
        {
            assetType = await context.CreateAssetType(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.AssetTypeOverview();
            overviewPage.Search(assetType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [Order(8)]
        [When(@"I want to deactivate the assettype with reason (.*)")]
        public void WhenIWantToDeactivateTheAssettypeWithReasonTest(string reason)
        {
            var deactivatepage = overviewPage.Deactivate();
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivate");
            deactivatepage.Reason = reason;
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            this.reason = reason;
            deactivatepage.Delete();
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivated");
        }
        [Order(9)]
        [Then(@"the assettype has been deactiveted")]
        public void ThenTheAssettypeHasBeenDeactiveted()
        {
            overviewPage.Search(assetType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string expectedlog = $"The {assetType.Category.Category} type Vendor: {assetType.Vendor} and type {assetType.Type} is deleted due to {reason} by {admin.Account.UserID} in table assettype";
            var log = detail.GetLastLog();
            expectedlog.Should().BeEquivalentTo(log);
        }
        [Order(10)]
        [Given(@"There is an Inactive AssetType existing")]
        public async Task GivenThereIsAnInactiveAssetTypeExisting()
        {
            assetType = await context.CreateAssetType(admin, false);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.AssetTypeOverview();
            overviewPage.Search(assetType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [Order(11)]
        [When(@"I want to activate the assettype")]
        public void WhenIWantToActivateTheAssettype()
        {
            overviewPage.Activate();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
        [Order(12)]
        [Then(@"the assettype has been activeted")]
        public void ThenTheAssettypeHasBeenActiveted()
        {
            overviewPage.Search(assetType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            string expectedlog = $"The {assetType.Category.Category} type Vendor: {assetType.Vendor} and type {assetType.Type} is activated by {admin.Account.UserID} in table assettype";
            var log = detail.GetLastLog();
            expectedlog.Should().BeEquivalentTo(log);
        }

    }
}
