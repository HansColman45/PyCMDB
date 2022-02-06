using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public sealed class AccountSteps : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private AccountOverviewPage overviewPage;
        private CreateAccountPage createAccount;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.Account account;
        private entity.Account Account;
        private string expectedlog, oldValue, newValue, changedField;

        public AccountSteps(ScenarioData scenarioData) : base(scenarioData)
        {
        }
        [Given(@"I want to create a Account with the following details")]
        public void GivenIWantToCreateAAccountWithTheFollowingDetails(Table table)
        {
            account = table.CreateInstance<helpers.Account>();
            rndNr = rnd.Next();
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.AccountOverview();
            createAccount = overviewPage.New();
            createAccount.UserId = account.UserId + rndNr.ToString();
            createAccount.Type = account.Type;
            createAccount.Application = account.Application;
        }
        [When(@"I save the account")]
        public void WhenISaveTheAccount()
        {
            createAccount.Create();
        }
        [Then(@"The account is saved")]
        public void ThenTheAccountIsSaved()
        {
            overviewPage.Search(account.UserId + rndNr.ToString());
            var detail = overviewPage.Detail();
            var log = detail.GetLastLog();
            expectedlog = $"The Account width UserID: {account.UserId + rndNr.ToString()} with type {account.Type} for application {account.Application} is created by {admin.Account.UserID} in table account";
            Assert.Equal(expectedlog, log);
        }

        [Given(@"There is an account existing")]
        public async Task GivenThereIsAnAccountExisting()
        {
            Account = await context.CreateAccount(admin);
            rndNr = rnd.Next();
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.AccountOverview();
            overviewPage.Search(Account.UserID);
        }
        [When(@"I change the (.*) to (.*) and I save the changes")]
        public void WhenIChangeTheUserIdToTestjeAndISaveTheChanges(string field, string newValue)
        {
            var editPage = overviewPage.Edit();
            changedField = field;
            switch (field)
            {
                case "UserId":
                    oldValue = editPage.UserId;
                    this.newValue = newValue + rndNr.ToString();
                    editPage.UserId = this.newValue;
                    break;
                case "Type":
                    oldValue = "Normal User";
                    this.newValue = newValue;
                    editPage.Type = newValue;
                    break;
                case "Application":
                    oldValue = "CMDB";
                    this.newValue = newValue;
                    editPage.Application = newValue;
                    break;
            }
            editPage.Edit();
        }
        [Then(@"The changes in account are saved")]
        public void ThenTheChangesInAccountAreSaved()
        {
            switch (changedField)
            {
                case "UserId":
                    overviewPage.Search(newValue);
                    expectedlog = $"The UserId in table account has been changed from {oldValue} to {newValue} by {admin.Account.UserID}";
                    break;
                case "Type":
                    overviewPage.Search(Account.UserID);
                    expectedlog = $"The Type in table account has been changed from {oldValue} to {newValue} by {admin.Account.UserID}";
                    break;
                case "Application":
                    overviewPage.Search(Account.UserID);
                    expectedlog = $"The Application in table account has been changed from {oldValue} to {newValue} by {admin.Account.UserID}";
                    break;
            }
            var detail = overviewPage.Detail();
            var log = detail.GetLastLog();
            Assert.Equal(expectedlog, log);
        }

        [Given(@"There is an active account existing")]
        public async Task GivenThereIsAnActiveAccountExisting()
        {
            Account = await context.CreateAccount(admin);
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.AccountOverview();
            overviewPage.Search(Account.UserID);
        }
        [When(@"I deactivate the account with reason (.*)")]
        public void WhenIDeactivateTheAccountWithReasonTest(string reason)
        {
            var deactivatepage = overviewPage.Deactivate();
            deactivatepage.Reason = reason;
            newValue = reason;
            deactivatepage.Delete();
        }
        [Then(@"the account is inactive")]
        public void ThenTheAccountIsInactive()
        {
            overviewPage.Search(Account.UserID);
            var detail = overviewPage.Detail();
            var accId = detail.Id;
            entity.Account account = context.GetAccount(accId);
            expectedlog = $"The Account width UserID: {account.UserID} and type {account.Type.Description} in table account is deleted due to {newValue} by {admin.Account.UserID}";
            var log = detail.GetLastLog();
            Assert.Equal(expectedlog, log);
        }

        [Given(@"There is an inactive account existing")]
        public async Task GivenThereIsAnInactiveAccountExisting()
        {
            Account = await context.CreateAccount(admin, false);
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.AccountOverview();
            overviewPage.Search(Account.UserID);
        }
        [When(@"I activate the account")]
        public void WhenIActivateTheAccount()
        {
            overviewPage.Activate();
        }
        [Then(@"The account is active")]
        public void ThenTheAccountIsActive()
        {
            overviewPage.Search(Account.UserID);
            var detail = overviewPage.Detail();
            var accId = detail.Id;
            entity.Account account = context.GetAccount(accId);
            expectedlog = $"The Account width UserID: {account.UserID} and type {account.Type.Description} in table account is activated by {admin.Account.UserID}";
            var log = detail.GetLastLog();
            Assert.Equal(expectedlog, log);
        }

    }
}
