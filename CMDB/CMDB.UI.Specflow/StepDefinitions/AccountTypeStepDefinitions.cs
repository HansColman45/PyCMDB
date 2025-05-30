using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.AccountTypes;
using CMDB.UI.Specflow.Questions;
using Reqnroll;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class AccountTypeStepDefinitions : TestBase
    {
		private Helpers.AccountType accountType;
        private AccountTypeCreator accountTypeCreator;
		private AccountTypeUpdator accountTypeUpdator;
		private AccountType AccountType;
		public AccountTypeStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }
		#region create accounttype
		[Given(@"I want to create an accounttype as follows")]
        public async Task GivenIWantToCreateAnAccounttypeAsFollows(Table table)
        {
            accountTypeCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(accountTypeCreator);
            accountType = table.CreateInstance<Helpers.AccountType>();
			Admin = await accountTypeCreator.CreateNewAdmin();
            accountTypeCreator.DoLogin(Admin.Account.UserID, "1234");
            bool result = accountTypeCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            accountTypeCreator.OpenAccountTypeOverviewPage();
		}
        [When(@"I save the accounttype")]
        public void WhenISaveTheAccounttype()
        {
            accountTypeCreator.CreateAccountType(accountType);
        }
        [Then(@"The new accounttype can be find in the system")]
        public void ThenTheNewAccounttypeCanBeFindInTheSystem()
        {
			accountTypeCreator.SearchAccountType(accountType);
			var lastLog = accountTypeCreator.AccountTypeLastLogLine;
			expectedlog = accountTypeCreator.ExpectedLog;
			lastLog.Should().BeEquivalentTo(expectedlog);

		}
		#endregion
		[Given(@"There is an accounttype existing in the system")]
		public async Task GivenThereIsAnAccounttypeExistingInTheSystem()
		{
			accountTypeUpdator = new(ScenarioContext);
			ActorRegistry.RegisterActor(accountTypeUpdator);
			Admin = await accountTypeUpdator.CreateNewAdmin();
			AccountType = await accountTypeUpdator.CreateNewAccountType();
			accountTypeUpdator.DoLogin(Admin.Account.UserID, "1234");
			bool result = accountTypeUpdator.Perform(new IsTheUserLoggedIn());
			result.Should().BeTrue();
			accountTypeUpdator.OpenAccountTypeOverviewPage();
			accountTypeUpdator.Search(AccountType.Description);
		}
		#region Update accounttype
		[When(@"I update the (.*) and change it to (.*) and I save the accounttype")]
		public void WhenIUpdateTheTypeAndChangeItToRootAndISaveTheAccounttype(string field, string value)
		{
			AccountType = accountTypeUpdator.UpdateAccountType(AccountType, field, value);
		}
		[Then(@"The account type has been saved")]
		public void ThenTheAccountTypeHasBeenSaved()
		{
			accountTypeUpdator.Search(AccountType.Description);
		}
		[Then(@"the Change is done")]
		public void ThenTheChangeIsDone()
		{
			var lastLog = accountTypeUpdator.AccountTypeLastLogLine;
			expectedlog = accountTypeUpdator.ExpectedLog;
			lastLog.Should().BeEquivalentTo(expectedlog);
		}
		#endregion
		#region Deactivate account
		[When(@"I deactivate the accountType with reason (.*)")]
		public void WhenIDeactivateTheAccountTypeWithReasonTest(string reason)
		{
			accountTypeUpdator.DeactivateAccount(AccountType, reason);
		}
		[Then(@"The accountType is deacticated")]
		public void ThenTheAccountTypeIsDeacticated()
		{
            accountTypeUpdator.Search(AccountType.Description);
            var lastLog = accountTypeUpdator.AccountTypeLastLogLine;
			expectedlog = accountTypeUpdator.ExpectedLog;
			lastLog.Should().BeEquivalentTo(expectedlog);
		}
		#endregion
		#region Activate account
		[Given(@"There is an inactive accountType existing in the system")]
		public async Task GivenThereIsAnInactiveAccountTypeExistingInTheSystem()
		{
			accountTypeUpdator = new(ScenarioContext);
			ActorRegistry.RegisterActor(accountTypeUpdator);
			Admin = await accountTypeUpdator.CreateNewAdmin();
			AccountType = await accountTypeUpdator.CreateNewAccountType(false);
			accountTypeUpdator.DoLogin(Admin.Account.UserID, "1234");
			bool result = accountTypeUpdator.Perform(new IsTheUserLoggedIn());
			result.Should().BeTrue();
			accountTypeUpdator.OpenAccountTypeOverviewPage();
			accountTypeUpdator.Search(AccountType.Description);
		}
		[When(@"I activate the accountType")]
		public void WhenIActivateTheAccountType()
		{
			accountTypeUpdator.ActivateAccount(AccountType);
		}
		[Then(@"The accountType is active")]
		public void ThenTheAccountTypeIsActive()
		{
			accountTypeUpdator.Search(AccountType.Description);
			var lastLog = accountTypeUpdator.AccountTypeLastLogLine;
			expectedlog = accountTypeUpdator.ExpectedLog;
			lastLog.Should().BeEquivalentTo(expectedlog);
		}
		#endregion
	}
}
