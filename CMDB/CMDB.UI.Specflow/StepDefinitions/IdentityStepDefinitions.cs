using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using CMDB.UI.Specflow.Actors;
using CMDB.UI.Specflow.Questions;
using Microsoft.Graph;
using TechTalk.SpecFlow.Assist;
using Identity = CMDB.Domain.Entities.Identity;
using Table = TechTalk.SpecFlow.Table;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class IdentityStepDefinitions: TestBase
    {
        private Helpers.Identity iden;
        private Identity Identity;
        private IdentityCreator identityCreator;
        private IdentityUpdator identityUpdator;
        private CreateIdentityPage createIdentity;
        private string updatedfield;

        public IdentityStepDefinitions(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }
        #region Create Identity
        [Given(@"I want to create an Identity with these details")]
        public async Task GivenIWantToCreateAnIdentityWithTheseDetails(Table table)
        {
            identityCreator = new(ScenarioContext);
            iden = table.CreateInstance<Helpers.Identity>();
            Admin = await identityCreator.CreateNewAdmin();
            identityCreator.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityCreator.IsTheUserLoggedIn;
            result.Should().BeTrue();
            identityCreator.OpenIdentityOverviewPage();
            createIdentity = identityCreator.OpenCreateIdentityPage();
            identityCreator.CreateNewIdentity(iden);
        }
        [When(@"I save")]
        public void WhenISave()
        {
            createIdentity.Create();
            createIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Saved");
        }
        [Then(@"I can find the newly created Identity back")]
        public void ThenICanFindTheNewlyCreatedIdentityBack()
        {
            identityCreator.SearchIdentity(iden);
            var lastLog = identityCreator.IdentityLastLogLine;
            expectedlog = identityCreator.ExpectedLog;
            lastLog.Should().BeEquivalentTo(expectedlog);
            identityCreator.Dispose();
        }
        #endregion
        #region Edit Identity
        [Given(@"An Identity exisist in the system")]
        public async Task GivenAnIdentityExisistInTheSystem()
        {
            identityUpdator = new(ScenarioContext);
            Admin = await identityUpdator.CreateNewAdmin();
            identityUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityUpdator.IsTheUserLoggedIn;
            result.Should().BeTrue();
            identityUpdator.OpenIdentityOverviewPage(); 
        }
        [When(@"I want to update (.*) with (.*)")]
        public async Task WhenIWantToUpdateFirstNameWithTestje(string field, string newValue)
        {
            updatedfield = field;
            Identity = await identityUpdator.CreateNewIdentity();
            identityUpdator.Search(Identity.FirstName);
            identityUpdator.OpenUpdateIdentityPage();
            Identity = identityUpdator.UpdateIdentity(field, newValue, Identity);
        }
        [Then(@"The identity is updated")]
        public void ThenTheIdentityIsUpdated()
        {
            switch (updatedfield)
            {
                case "FirstName":
                    identityUpdator.Search(Identity.LastName);
                    break;
                case "LastName":
                case "Company":
                case "UserID":
                case "Email":
                    identityUpdator.Search(Identity.FirstName);
                    break;
            }
            var log = identityUpdator.IdentityLastLogLine;
            expectedlog = identityUpdator.ExpectedLog;
            log.Should().BeEquivalentTo(expectedlog);
            identityUpdator.Dispose();
        }
        #endregion
        #region Ideneity Actions
        [Given(@"An inactive Identity exisist in the system")]
        public async Task GivenAnInactiveIdentityExisistInTheSystem()
        {
            identityUpdator = new(ScenarioContext);
            Admin = await identityUpdator.CreateNewAdmin();
            identityUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityUpdator.IsTheUserLoggedIn;
            result.Should().BeTrue();
            identityUpdator.OpenIdentityOverviewPage();
            Identity = await identityUpdator.CreateNewIdentity(false);
            identityUpdator.Search(Identity.FirstName);
        }
        [When(@"I want to activate this identity")]
        public void WhenIWantToActivateThisIdentity()
        {
            identityUpdator.Activate();
            identityUpdator.ExpectedLog = $"The Identity width name: {Identity.Name} is activated by {Admin.Account.UserID} in table identity";
        }
        [Then(@"The Identity is active")]
        public void ThenTheIdentityIsActive()
        {
            identityUpdator.Search(Identity.FirstName);
            expectedlog = identityUpdator.ExpectedLog;
            var log = identityUpdator.IdentityLastLogLine;
            log.Should().BeEquivalentTo(expectedlog);
            identityUpdator.Dispose();
        }

        [Given(@"An acive Identity exisist in the system")]
        public async Task GivenAnAciveIdentityExisistInTheSystem()
        {
            identityUpdator = new(ScenarioContext);
            Admin = await identityUpdator.CreateNewAdmin();
            identityUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityUpdator.IsTheUserLoggedIn;
            result.Should().BeTrue();
            identityUpdator.OpenIdentityOverviewPage();
            Identity = await identityUpdator.CreateNewIdentity();
            identityUpdator.Search(Identity.FirstName);
        }
        [When(@"I want to deactivete the identity whith the reason (.*)")]
        public void WhenIWantToDeactiveteTheIdentityWhithTheReasonTest(string reason)
        {
            identityUpdator.OpenDeactivateIdentityPage();
            identityUpdator.ExpectedLog = $"The Identity width name: {Identity.Name} is deleted due to {reason} by {Admin.Account.UserID} in table identity";
            identityUpdator.Deactivate(reason);
        }
        [Then(@"The Idenetity is inactive")]
        public void ThenTheIdenetityIsInactive()
        {
            identityUpdator.Search(Identity.FirstName);
            expectedlog = identityUpdator.ExpectedLog;
            var log = identityUpdator.IdentityLastLogLine;
            log.Should().BeEquivalentTo(expectedlog);
            identityUpdator.Dispose();
        }
        #endregion
    }
}
