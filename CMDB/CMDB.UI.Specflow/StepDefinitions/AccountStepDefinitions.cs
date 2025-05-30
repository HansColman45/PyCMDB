using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.AccountAcctors;
using CMDB.UI.Specflow.Questions;
using Reqnroll;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class AccountStepDefinitions: TestBase
    {
        private Helpers.Account account;
        private Account _account;
        private Identity identity;

        private AccountCreator accountCreator;
        private AccountUpdator accountUpdator;
        private AccountIdentityActor accountIdentityActor;
        public AccountStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }
        #region Create Account
        [Given(@"I want to create an Account with the following details")]
        public async Task GivenIWantToCreateAAccountWithTheFollowingDetails(Table table)
        {
            accountCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(accountCreator);
            account = table.CreateInstance<Helpers.Account>();
            Admin = await accountCreator.CreateNewAdmin();
            accountCreator.DoLogin(Admin.Account.UserID, "1234");
            bool result = accountCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            accountCreator.OpenAccountOverviewPage();
        }
        [When(@"I save the account")]
        public void WhenISaveTheAccount()
        {
            accountCreator.CreateNewAccount(account);
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
        [Given(@"There is an account existing")]
        public async Task GivenThereIsAnAccountExisting()
        {
            accountUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(accountUpdator);
            Admin = await accountUpdator.CreateNewAdmin();
            _account = await accountUpdator.CreateAccount();
            log.Info($"Account created with UserID {_account.UserID}");
            accountUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = accountUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            accountUpdator.OpenAccountOverviewPage();
            accountUpdator.Search(_account.UserID);
        }
        #region Update Account
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
        #region Account Deactivated
        [When(@"I deactivate the account with reason (.*)")]
        public void WhenIDeactivateTheAccountWithReasonTest(string reason)
        {
            accountUpdator.DeactivateAccount(_account, reason);
        }
        [Then(@"the account is inactive")]
        public void ThenTheAccountIsInactive()
        {
            accountUpdator.Search(_account.UserID);
            expectedlog = accountUpdator.ExpectedLog;
            var lastLog = accountUpdator.AccountLastLogLine;
            lastLog.Should().BeEquivalentTo(expectedlog);
        }
        #endregion
        #region Account Deactivated
        [Given(@"There is an inactive account existing")]
        public async Task GivenThereIsAnInactiveAccountExisting()
        {
            accountUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(accountUpdator);
            Admin = await accountUpdator.CreateNewAdmin();
            _account = await accountUpdator.CreateAccount(false);
            log.Info($"Account created with UserID {_account.UserID}");
            accountUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = accountUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            accountUpdator.OpenAccountOverviewPage();
            accountUpdator.Search(_account.UserID);
        }
        [When(@"I activate the account")]
        public void WhenIActivateTheAccount()
        {
            accountUpdator.AcctivateAccount(_account);
        }
        [Then(@"The account is active")]
        public void ThenTheAccountIsActive()
        {
            accountUpdator.Search(_account.UserID);
            expectedlog = accountUpdator.ExpectedLog;
            var lastLog = accountUpdator.AccountLastLogLine;
            lastLog.Should().BeEquivalentTo(expectedlog);
        }
        #endregion
        [Given(@"There is an active account existing")]
        public async Task GivenThereIsAnActiveAccountExisting()
        {
            accountIdentityActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(accountIdentityActor);
            Admin = await accountIdentityActor.CreateNewAdmin();
            _account = await accountIdentityActor.CreateAccount();
            log.Info($"Account created with UserID {_account.UserID}");
            accountIdentityActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = accountIdentityActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            accountIdentityActor.OpenAccountOverviewPage();
            accountIdentityActor.Search(_account.UserID);
        }
        #region Assign Identity to Account
        [Given(@"an Identity exist as well")]
        public async Task GivenAnIdentityExistAsWell()
        {
            identity = await accountIdentityActor.CreateNewIdentity();
        }
        [When(@"I assign the identity to my account")]
        public void WhenIAssignTheIdentityToMyAccount()
        {
            accountIdentityActor.AssignTheIdentity2Account(_account, identity);
        }
        [When(@"I fill in the assig form for my account")]
        public void WhenIFillInTheAssigFormForMyAccount()
        {
            accountIdentityActor.FillInAssignForm();
        }
        [Then(@"The identity is assigned to my account")]
        public void ThenTheIdentityIsAssignedToMyAccount()
        {
            accountIdentityActor.Search(_account.UserID);
            var lastLog = accountIdentityActor.AccountLastLogLine;
            lastLog.Should().BeEquivalentTo(accountIdentityActor.ExpectedLog);
        }
        #endregion
        #region Release Identity from Account
        [Given(@"There is an Identity assigned")]
        public async Task GivenThereIsAnIdentityAssigned()
        {
            identity = await accountIdentityActor.CreateNewIdentity();
            await accountIdentityActor.AssignAccount2Identity(_account, identity);
        }
        [When(@"I release the Identity and the release form is filled in")]
        public void WhenIReleaseTheIdentity()
        {
            accountIdentityActor.ReleaseIdentity(_account, identity);
        }
        [Then(@"The identity is released from my account")]
        public void ThenTheIdentityIsReleasedFromMyAccount()
        {
            accountIdentityActor.Search(_account.UserID);
            var lastLog = accountIdentityActor.AccountLastLogLine;
            lastLog.Should().BeEquivalentTo(accountIdentityActor.ExpectedLog);
        }
        #endregion
    }
}