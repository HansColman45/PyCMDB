using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Threading.Tasks;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using CMDB.Testing.Helpers;
using FluentAssertions;
using Xunit.Extensions.Ordering;

namespace CMDB.UI.Tests.Stepdefinitions.AccountType
{
    [Binding]
    public class AccountTypeSteps: TestBase
    {
        private LoginPage login;
        private MainPage main;
        private TypeOverviewPage overviewPage;
        private CreateTypePage createPage;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.AccountType accountType;
        private entity.AccountType AccountType;
        private string expectedLog, updatedField, newValue;

        public AccountTypeSteps(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }
        [Order(1)]
        [Given(@"I want to create an accounttype as follows:")]
        public void GivenIWantToCreateAnAccounttypeAsFollows(Table table)
        {
            accountType = table.CreateInstance<helpers.AccountType>();
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
            overviewPage = main.AccountTypeyOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            createPage = overviewPage.New();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreatePage");
            createPage.Type = accountType.Type + rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SetType");
            createPage.Description = accountType.Description + rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SetDescription");
        }
        [Order(2)]
        [When(@"I save the accounttype")]
        public void WhenISaveTheAccounttype()
        {
            createPage.Create();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }
        [Order(3)]
        [Then(@"The new accounttype can be find in the system")]
        public void ThenTheNewAccounttypeCanBeFindInTheSystem()
        {
            expectedLog = $"The Accounttype with type: {accountType.Type + rndNr.ToString()} and description: {accountType.Description + rndNr.ToString()} " +
                $"is created by {admin.Account.UserID} in table accounttype";
            overviewPage.Search(accountType.Type + rndNr.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog("accounttype");
            log.Should().BeEquivalentTo(expectedLog, "Log should match");
        }

        [Given(@"There is an accounttype existing in the system")]
        public async Task GivenThereIsAnAccounttypeExistingInTheSystem()
        {
            AccountType = await context.CreateAccountType(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LogedIn");
            overviewPage = main.AccountTypeyOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            overviewPage.Search(AccountType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [Order(4)]
        [When(@"I update the (.*) and change it to (.*) and I save the accounttype")]
        public void WhenIUpdateTheTypeAndChangeItToRootAndISaveTheAccounttype(string field, string value)
        {
            rndNr = rnd.Next();
            this.newValue = newValue + rndNr.ToString();
            updatedField = field;
            var editPage = overviewPage.Update();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditPage");
            switch (updatedField)
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
        [Order(5)]
        [Then(@"The account type has been saved")]
        public void ThenTheAccountTypeHasBeenSaved()
        {
            switch (updatedField)
            {
                case "Type":
                    expectedLog = $"The {updatedField} has been changed from {AccountType.Type} to {newValue} by {admin.Account.UserID} in table accounttype";
                    overviewPage.Search(AccountType.Description);
                    break;
                case "Description":
                    expectedLog = $"The {updatedField} has been changed from {AccountType.Description} to {newValue} by {admin.Account.UserID} in table accounttype";
                    overviewPage.Search(AccountType.Type);
                    break;
            }
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [Order(6)]
        [Then(@"the Change is done")]
        public void ThenTheChangeIsDone()
        {
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog("accounttype");
            log.Should().BeEquivalentTo(expectedLog, "Log should match");
        }
        
        [Order(7)]
        [When(@"I deactivate the accountType with reason (.*)")]
        public void WhenIDeactivateTheAccountTypeWithReasonTest(string reason)
        {
            newValue = reason;
            var deactivatePage = overviewPage.Deactivate();
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeactivatePage");
            deactivatePage.Reason = reason;
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            deactivatePage.Delete();
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivated");
        }
        [Order(8)]
        [Then(@"The accountType is deacticated")]
        public void ThenTheAccountTypeIsDeacticated()
        {
            expectedLog = $"The Accounttype with type: {AccountType.Type} and description: {AccountType.Description} is deleted due to {newValue} by {admin.Account.UserID} in table accounttype";
            overviewPage.Search(AccountType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog("accounttype");
            log.Should().BeEquivalentTo(expectedLog, "Log should match");
        }

        [Order(9)]
        [Given(@"There is an inactive accountType existing in the system")]
        public async Task GivenThereIsAnInactiveAccountTypeExistingInTheSystem()
        {
            AccountType = await context.CreateAccountType(admin, false);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LogedIn");
            overviewPage = main.AccountTypeyOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            overviewPage.Search(AccountType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [Order(10)]
        [When(@"I activate the accountType")]
        public void WhenIActivateTheAccountType()
        {
            overviewPage.Activate();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activate");
        }
        [Order(11)]
        [Then(@"The accountType is active")]
        public void ThenTheAccountTypeIsActive()
        {
            expectedLog = $"The Accounttype with type: {AccountType.Type} and description: {AccountType.Description} is activated by {admin.Account.UserID} in table accounttype";
            overviewPage.Search(AccountType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog("accounttype");
            log.Should().BeEquivalentTo(expectedLog, "Log should match");
        }
    }
}
