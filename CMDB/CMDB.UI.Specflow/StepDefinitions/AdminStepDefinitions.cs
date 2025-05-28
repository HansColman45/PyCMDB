using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.Admins;
using CMDB.UI.Specflow.Questions;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class AdminStepDefinitions: TestBase
    {
        private Admin newAdmin;
        private Account account;

        private AdminCreator adminCreator;
        private AdminUpdater adminUpdater;
        public AdminStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }
        [Given(@"There is a account existing in the system")]
        public async Task GivenThereIsAAccountExistingInTheSystem()
        {
            adminCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(adminCreator);
            Admin = await adminCreator.CreateNewAdmin();
            account = await adminCreator.CreateAccount();
            adminCreator.DoLogin(Admin.Account.UserID, "1234");
            bool result = adminCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            adminCreator.OpenAdminOverviewPage();
        }

        [When(@"I create the new admin for that account with level (.*)")]
        public void WhenICreateTheNewAdminForThatAccountWithLevel(string level)
        {
            adminCreator.DoCreateNewAdmin(account, level);
        }

        [Then(@"The newly created amin exists in the system")]
        public void ThenTheNewlyCreatedAminExistsInTheSystem()
        {
            adminCreator.Search(account.UserID);
            var lastLog = adminCreator.LastLogLine;
            lastLog.Should().BeEquivalentTo(adminCreator.ExpectedLog);
        }

        #region Edit
        [Given(@"There is a admin existing in the system")]
        public async Task GivenThereIsAAdminExistingInTheSystem()
        {
            adminUpdater = new(ScenarioContext);
            ActorRegistry.RegisterActor(adminUpdater);
            Admin = await adminUpdater.CreateNewAdmin();
            account = await adminUpdater.CreateAccount();
            newAdmin = await adminUpdater.CreateAdminForAccount(account);
            adminUpdater.DoLogin(Admin.Account.UserID, "1234");
            bool result = adminUpdater.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            adminUpdater.OpenAdminOverviewPage();
            adminUpdater.Search(account.UserID);
        }
        [When(@"I change the admin's level to (.*) and update it")]
        public void WhenIChangeTheAdminsLevelToAndUpdateIt(string level)
        {
            adminUpdater.DoUpdateAdmin(newAdmin, level);
        }
        [Then(@"The updated admin exists in the system")]
        public void ThenTheUpdatedAdminExistsInTheSystem()
        {
            adminUpdater.Search(account.UserID);
            var lastLog = adminUpdater.LastLogLine;
            lastLog.Should().BeEquivalentTo(adminUpdater.ExpectedLog);
        }
        #endregion
        #region activation
        [When(@"I deactivate the admin with the reason (.*)")]
        public void WhenIDeactivateTheAdminWithTheReasonTest(string reason)
        {
            adminUpdater.Deactivate(newAdmin, reason);
        }
        [Then(@"The admin is deactivated")]
        public void ThenTheAdminIsDeactivated()
        {
            adminUpdater.Search(account.UserID);
            var lastLog = adminUpdater.LastLogLine;
            lastLog.Should().BeEquivalentTo(adminUpdater.ExpectedLog);
        }
        #endregion
        #region activate
        [Given(@"There is a deactivated admin existing in the system")]
        public async Task GivenThereIsADeactivatedAdminExistingInTheSystem()
        {
            adminUpdater = new(ScenarioContext);
            ActorRegistry.RegisterActor(adminUpdater);
            Admin = await adminUpdater.CreateNewAdmin();
            account = await adminUpdater.CreateAccount();
            newAdmin = await adminUpdater.CreateAdminForAccount(account, false);
            adminUpdater.DoLogin(Admin.Account.UserID, "1234");
            bool result = adminUpdater.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            adminUpdater.OpenAdminOverviewPage();
            adminUpdater.Search(account.UserID);
        }

        [When(@"I activate the admin")]
        public void WhenIActivateTheAdmin()
        {
            adminUpdater.Activate(newAdmin);
        }
        [Then(@"The admin is activated")]
        public void ThenTheAdminIsActivated()
        {
            adminUpdater.Search(account.UserID);
            var lastLog = adminUpdater.LastLogLine;
            lastLog.Should().BeEquivalentTo(adminUpdater.ExpectedLog);
        }
        #endregion
    }
}
