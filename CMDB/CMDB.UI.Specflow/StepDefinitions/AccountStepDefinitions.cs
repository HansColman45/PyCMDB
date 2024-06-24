using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using CMDB.UI.Specflow.Actors;
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

        private CreateAccountPage createAccountPage;
        public AccountStepDefinitions(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [Given(@"I want to create an Account with the following details")]
        public async Task GivenIWantToCreateAAccountWithTheFollowingDetails(Table table)
        {
            accountCreator = new(ScenarioContext);
            account = table.CreateInstance<Helpers.Account>();
            Admin = await accountCreator.CreateNewAdmin();
            accountCreator.DoLogin(Admin.Account.UserID, "1234");
            bool result = accountCreator.IsTheUserLoggedIn;
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
    }
}
