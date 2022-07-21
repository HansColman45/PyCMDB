using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;
using System.Threading.Tasks;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using CMDB.UI.Tests.Pages;
using FluentAssertions;
using CMDB.UI.Tests.Hooks;
using CMDB.Testing.Helpers;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public class IdentityTypeStepDefinitions: TestBase
    {
        private LoginPage login;
        private MainPage main;
        private TypeOverviewPage overviewPage;
        private CreateTypePage createPage;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.IdentiyType identiyType;
        private entity.IdentityType IdentityType;
        private string expectedLog, updatedField, newValue;
        public IdentityTypeStepDefinitions(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }

        [Given(@"I want to create an Identity type with these details")]
        public void GivenIWantToCreateAnIdentityTypeWithTheseDetails(Table table)
        {
            identiyType = table.CreateInstance<helpers.IdentiyType>();
            rndNr = rnd.Next();
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LogedIn");
            overviewPage = main.IdentityTypeOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            createPage = overviewPage.New();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreatePage");
            createPage.Type = identiyType.Type + rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SetType");
            createPage.Description = identiyType.Description + rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SetDescription");
        }
        [When(@"I save the Identity type")]
        public void WhenISaveTheIdentityType()
        {
            createPage.Create();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }

        [Then(@"The I can find the newly create Identity type back")]
        public void ThenTheICanFindTheNewlyCreateIdentityTypeBack()
        {
            expectedLog = $"The Identitytype created with type: {identiyType.Type + rndNr.ToString()} and description: {identiyType.Description + rndNr.ToString()} " +
                $"is created by {admin.Account.UserID} in table identitytype";
            overviewPage.Search(identiyType.Type + rndNr.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedLog,"Log should match");
        }

        [Given(@"There is an Identity type existing")]
        public async Task GivenThereIsAnIdentityTypeExisting()
        {
            IdentityType = await context.CreateIdentityType(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LogedIn");
            overviewPage = main.IdentityTypeOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            overviewPage.Search(IdentityType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I change the (.*) to (.*) and I save the Identity type")]
        public void WhenIChangeTheTypeToAlienAndISaveTheIdentityType(string field, string newValue)
        {
            this.newValue = newValue;
            updatedField = field;
            var editPage = overviewPage.Update();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditPage");
            switch (field)
            {
                case "Type":
                    editPage.Type = newValue;
                    editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_TypeChanged");
                    break;
                case "Description":
                    editPage.Description = newValue;
                    editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DescriptionChanged");
                    break;
            }
            editPage.Edit();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Updated");
        }
        [Then(@"The Identity type is changed and the new values are visable")]
        public void ThenTheIdentityTypeIsChangedAndTheNewValuesAreVisable()
        {
            switch (updatedField)
            {
                case "Type":
                    expectedLog =  $"The {updatedField} has been changed from {IdentityType.Type} to {newValue} by {admin.Account.UserID} in table identitytype";
                    overviewPage.Search(IdentityType.Description);
                    break;
                case "Description":
                    expectedLog = $"The {updatedField} has been changed from {IdentityType.Description} to {newValue} by {admin.Account.UserID} in table identitytype";
                    overviewPage.Search(IdentityType.Type);
                    break;
            }
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedLog, "Log should match");
        }

        [When(@"I want to deactivate the identity type with reason (.*)")]
        public void WhenIWantToDeactivateTheIdentityTypeWithReasonTest(string reason)
        {
            newValue = reason;
            var deactivatePage = overviewPage.Deactivate();
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeactivatePage");
            deactivatePage.Reason = reason;
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            deactivatePage.Delete();
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivated");

        }
        [Then(@"The Identity type is deactivated")]
        public void ThenTheIdentityTypeIsDeactivated()
        {
            expectedLog = $"The Identitytype with type: {IdentityType.Type} and description: {IdentityType.Description} is deleted due to {newValue} by {admin.Account.UserID} in table identitytype";
            overviewPage.Search(IdentityType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedLog, "Log should match");
        }

        [Given(@"There is an inactive Identitytype existing")]
        public async Task GivenThereIsAnInactiveIdentitytypeExisting()
        {
            IdentityType = await context.CreateIdentityType(admin,false);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LogedIn");
            overviewPage = main.IdentityTypeOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            overviewPage.Search(IdentityType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I want to activate the Idenity type")]
        public void WhenIWantToActivateTheIdenityType()
        {
            overviewPage.Activate();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activate");
        }
        [Then(@"The Identity type is active")]
        public void ThenTheIdentityTypeIsActive()
        {
            expectedLog = $"The Identitytype with type: {IdentityType.Type} and description: {IdentityType.Description} is activated by {admin.Account.UserID} in table identitytype";
            overviewPage.Search(IdentityType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedLog, "Log should match");
        }

    }
}
