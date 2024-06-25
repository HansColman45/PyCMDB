using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using CMDB.UI.Specflow.Actors;
using CMDB.UI.Specflow.Questions;
using CMDB.UI.Specflow.Questions.Account;
using TechTalk.SpecFlow.Assist;
using Helpers = CMDB.UI.Specflow.Helpers;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class AccountStepDefinitions: TestBase
    {
        private Helpers.Account account;
        private Account _account;
        private AccountCreator accountCreator;
        private AccountUpdator accountUpdator;

        private CreateAccountPage createAccountPage;
        public AccountStepDefinitions(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }
        #region Create Account
        [Given(@"I want to create an Account with the following details")]
        public async Task GivenIWantToCreateAAccountWithTheFollowingDetails(Table table)
        {
            accountCreator = new(ScenarioContext);
            account = table.CreateInstance<Helpers.Account>();
            Admin = await accountCreator.CreateNewAdmin();
            accountCreator.DoLogin(Admin.Account.UserID, "1234");
            bool result = accountCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            accountCreator.OpenAccountOverviewPage();
            createAccountPage = accountCreator.OpenAccountCreatePage();
            accountCreator.CreateNewAccount(account);
        }

        [When(@"I save the account")]
        public void WhenISaveTheAccount()
        {
            createAccountPage.Create();
            createAccountPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Saved");
        }

        [Then(@"The account is saved")]
        public void ThenTheAccountIsSaved()
        {
            accountCreator.SearchAccount(account);
            expectedlog = accountCreator.ExpectedLog;
            var lastLog = accountCreator.AccountLastLogLine;
            lastLog.Should().BeEquivalentTo(expectedlog);
        }
        #endregion
        #region Update Account
        [Given(@"There is an account existing")]
        public async Task GivenThereIsAnAccountExisting()
        {
            accountUpdator = new(ScenarioContext);
            Admin = await accountUpdator.CreateNewAdmin();
            _account = await accountUpdator.CreateAccount();
            accountUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = accountUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            accountUpdator.OpenAccountOverviewPage();
        }
        [When(@"I change the (.*) to (.*) and I save the changes")]
        public void WhenIChangeTheUserIdToTestjeAndISaveTheChanges(string field, string newValue)
        {
            accountUpdator.Search(_account.UserID);
            _account = accountUpdator.UpdateAccount(field, newValue, _account);
        }
        [Then(@"The changes in account are saved")]
        public void ThenTheChangesInAccountAreSaved()
        {
            accountUpdator.Search(_account.UserID);
            expectedlog = accountUpdator.ExpectedLog;
            var lastLog = accountUpdator.AccountLastLogLine;
            lastLog.Should().BeEquivalentTo(expectedlog);
        }
        #endregion
    }
}
