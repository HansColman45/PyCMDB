using CMDB.UI.Tests.Hooks;
using System;
using TechTalk.SpecFlow;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using TechTalk.SpecFlow.Assist;
using CMDB.UI.Tests.Pages;
using CMDB.Testing.Helpers;
using FluentAssertions;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public class MobileStepDefinitions: TestBase
    {
        private LoginPage login;
        private MainPage main;
        private MobileOverviewPage overviewPage;
        private CreateMobilePage createPage;
        private AssignFormPage assignForm;

        private helpers.Mobile mobile;
        private entity.Mobile Mobile;
        private entity.Identity identity;
        private readonly Random rnd = new();
        private int rndNr;
        private string expectedlog, newValue, changedField;

        public MobileStepDefinitions(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }

        [Given(@"I want to create a new Mobile with these details")]
        public void GivenIWantToCreateANewMobileWithTheseDetails(Table table)
        {
            rndNr = rnd.Next();
            mobile = table.CreateInstance<helpers.Mobile>();
            entity.AssetCategory category = context.GetAssetCategory("Mobile");
            string Vendor, Type, assetType;
            assetType = mobile.Type;
            Vendor = assetType.Split(" ")[0];
            Type = assetType.Split(" ")[1];
            entity.AssetType AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.MobileOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            createPage = overviewPage.New();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_New");
            createPage.IMEI = mobile.IMEI + rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_IMEI");
            createPage.Type = AssetType.TypeID.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
        }
        [When(@"I save the mobile")]
        public void WhenISaveTheMobile()
        {
            createPage.Create();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }
        [Then(@"I can find the newly created Mobile back")]
        public void ThenICanFindTheNewlyCreatedMobileBack()
        {
            expectedlog = $"The Mobile with type {mobile.Type} is created by {admin.Account.UserID} in table mobile";
            overviewPage.Search(mobile.IMEI + rndNr.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }

        [Given(@"There is a mobile existing in the system")]
        public async Task GivenThereIsAMobileExistingInTheSystem()
        {
            Mobile = await context.CreateMobile(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.MobileOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(mobile.IMEI + rndNr.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I change the (.*) to (.*) and I save the changes for my mobile")]
        public void WhenIChangeTheIMEIToAndISaveTheChangesForMyMobile(string updatedField, string value)
        {
            rndNr = rnd.Next();
            changedField = updatedField;
            entity.AssetCategory category = context.GetAssetCategory("Mobile");
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
            updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_UpdatePage");
            switch (changedField)
            {
                case "IMEI":
                    updatePage.IMEI = value + rndNr.ToString();
                    newValue = value +rndNr.ToString();
                    updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_IMEI");
                    break;
                case "Type":
                    updatePage.Type = AssetType.TypeID.ToString();
                    newValue = AssetType.ToString();
                    updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
                    break;
            }
            updatePage.Edit();
        }
        [Then(@"The changes in mobile are saved")]
        public void ThenTheChangesInMobileAreSaved()
        {
            switch (changedField)
            {
                case "IMEI":
                    overviewPage.Search(newValue);
                    expectedlog = $"The IMEI in table mobile has been changed from {Mobile.IMEI} to {newValue} by {admin.Account.UserID}";
                    break;
                case "Type":
                    overviewPage.Search(Mobile.IMEI.ToString());
                    expectedlog = $"The Type in table mobile has been changed from {Mobile.MobileType} to {newValue} by {admin.Account.UserID}";
                    break;
            }
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }

        [When(@"I deactivate the mobile with reason (.*)")]
        public void WhenIDeactivateTheMobileWithReasonTest(string reason)
        {
            newValue = reason;
            var deactivatePage = overviewPage.Deactivate();
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeactivatePage");
            deactivatePage.Reason = reason;
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            deactivatePage.Delete();
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivated");
        }
        [Then(@"The mobile is deactivated")]
        public void ThenTheMobileIsDeactivated()
        {
            expectedlog = $"The mobile with type {Mobile.MobileType} is deactivated due to {newValue} in table mobile by {admin.Account.UserID}";
            overviewPage.Search(Mobile.IMEI.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }

        [Given(@"There is an inactive mobile existing in the system")]
        public async Task GivenThereIsAnInactivaMobileExistingInTheSystem()
        {
            Mobile = await context.CreateMobile(admin,false);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.MobileOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(Mobile.IMEI.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I activate the mobile")]
        public void WhenIActivateTheMobile()
        {
            overviewPage.Activate();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
        [Then(@"The mobile is actice again")]
        public void ThenTheMobileIsActiceAgain()
        {
            expectedlog = $"The mobile with type {Mobile.MobileType} is activated in table mobile by {admin.Account.UserID}";
            overviewPage.Search(Mobile.IMEI.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }

        [When(@"I assign the identity to my mobile")]
        public void WhenIAssignTheIdentityToMyMobile()
        {
            identity = (entity.Identity)TestData.Get("Identity");
            var assignPage = overviewPage.AssignIdentity();
            assignPage.Title.Should().BeEquivalentTo("Assign identity to docking", "Title should be correct");
            assignPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignPage");
            assignPage.SelectIdentity(identity);
            assignPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectIdentity");
            assignForm = assignPage.Assign();
        }
        [When(@"I fill in the assign form for my mobile")]
        public void FillAssignForm()
        {
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignForm");
            assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            assignForm.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            assignForm.CreatePDF();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_PDFCreated");
        }
        [Then(@"The identity is assigned to my mobile")]
        public void ThenTheIdentityIsAssignedToMyMobile()
        {
            expectedlog = $"The mobile with type {Mobile.MobileType} is assigned to Identity width name: {identity.Name} by {admin.Account.UserID} in table mobile ";
            overviewPage.Search(Mobile.IMEI.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }

    }
}
