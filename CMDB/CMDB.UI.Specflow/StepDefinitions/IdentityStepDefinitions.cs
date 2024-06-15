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
        private IdentityOverviewPage overviewPage;
        private CreateIdentityPage createIdentity;
        private string updatedfield, reason, expectedlog;

        public IdentityStepDefinitions(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }
        #region Create Identity
        [Given(@"I want to create an Identity with these details")]
        public async Task GivenIWantToCreateAnIdentityWithTheseDetails(Table table)
        {
            identityCreator = new(_scenarioContext);
            iden = table.CreateInstance<Helpers.Identity>();
            Admin = await identityCreator.CreateNewAdmin();
            identityCreator.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            overviewPage = identityCreator.Perform(new OpenTheIdentityOverviewPage());
            identityCreator.IsAbleToDoOrUse(overviewPage);
            overviewPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Overview");
            createIdentity = identityCreator.Perform(new OpenTheCreateIdentityPage());
            identityCreator.IsAbleToDoOrUse(createIdentity);
            identityCreator.CreateNewIdentity(iden);
        }
        [When(@"I save")]
        public void WhenISave()
        {
            createIdentity.Create();
            createIdentity.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Saved");
        }
        [Then(@"I can find the newly created Identity back")]
        public void ThenICanFindTheNewlyCreatedIdentityBack()
        {
            identityCreator.SearchIdentity(iden);
            var log = identityCreator.IdentityLastLogLine;
            expectedlog = identityCreator.ExpectedLog;
            log.Should().BeEquivalentTo(expectedlog);
            identityCreator.Dispose();
        }
        #endregion
        #region Edit Identity
        [Given(@"An Identity exisist in the system")]
        public async Task GivenAnIdentityExisistInTheSystem()
        {
            identityUpdator = new(_scenarioContext);
            Admin = await identityUpdator.CreateNewAdmin();
            identityUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            overviewPage = identityUpdator.Perform(new OpenTheIdentityOverviewPage());
            identityUpdator.IsAbleToDoOrUse(overviewPage);
            overviewPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Overview");
        }
        [When(@"I want to update (.*) with (.*)")]
        public async Task WhenIWantToUpdateFirstNameWithTestje(string field, string newValue)
        {
            updatedfield = field;
            Identity = await identityUpdator.CreateNewIdentity();
            overviewPage.Search(Identity.FirstName);
            overviewPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Search");
            var updatepage = identityUpdator.Perform(new OpenTheUpdateIdentityPage());
            identityUpdator.IsAbleToDoOrUse(updatepage);
            Identity = identityUpdator.UpdateIdentity(field, newValue, Identity);
        }
        [Then(@"The identity is updated")]
        public void ThenTheIdentityIsUpdated()
        {
            switch (updatedfield)
            {
                case "FirstName":
                    overviewPage.Search(Identity.LastName);
                    break;
                case "LastName":
                case "Company":
                case "UserID":
                case "Email":
                    overviewPage.Search(Identity.FirstName);
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
            identityUpdator = new(_scenarioContext);
            Admin = await identityUpdator.CreateNewAdmin();
            identityUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            overviewPage = identityUpdator.Perform(new OpenTheIdentityOverviewPage());
            identityUpdator.IsAbleToDoOrUse(overviewPage);
            overviewPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Overview");
            Identity = await identityUpdator.CreateNewIdentity(false);
            overviewPage.Search(Identity.FirstName);
            overviewPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Search");
        }
        [When(@"I want to activate this identity")]
        public void WhenIWantToActivateThisIdentity()
        {
            overviewPage.Activate();
            identityUpdator.ExpectedLog = $"The Identity width name: {Identity.Name} is activated by {Admin.Account.UserID} in table identity";
        }
        [Then(@"The Identity is active")]
        public void ThenTheIdentityIsActive()
        {
            overviewPage.Search(Identity.FirstName);
            overviewPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Search");
            var log = identityUpdator.IdentityLastLogLine;
            log.Should().BeEquivalentTo(expectedlog);
            identityUpdator.Dispose();
        }

        [Given(@"An acive Identity exisist in the system")]
        public async Task GivenAnAciveIdentityExisistInTheSystem()
        {
            identityUpdator = new(_scenarioContext);
            Admin = await identityUpdator.CreateNewAdmin();
            identityUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            overviewPage = identityUpdator.Perform(new OpenTheIdentityOverviewPage());
            identityUpdator.IsAbleToDoOrUse(overviewPage);
            overviewPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Overview");
            Identity = await identityUpdator.CreateNewIdentity();
            overviewPage.Search(Identity.FirstName);
            overviewPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Search");
        }
        [When(@"I want to deactivete the identity whith the reason (.*)")]
        public void WhenIWantToDeactiveteTheIdentityWhithTheReasonTest(string reason)
        {
            var deactivatePage = identityUpdator.Perform(new OpenTheDeactivateIdentityPage());
            identityUpdator.IsAbleToDoOrUse(deactivatePage);
            this.reason = reason;
            identityUpdator.ExpectedLog = $"The Identity width name: {Identity.Name} is deleted due to {reason} by {Admin.Account.UserID} in table identity";
            identityUpdator.Deactivate(reason);
        }
        [Then(@"The Idenetity is inactive")]
        public void ThenTheIdenetityIsInactive()
        {
            overviewPage.Search(Identity.FirstName);
            overviewPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Search");
            var log = identityUpdator.IdentityLastLogLine;
            log.Should().BeEquivalentTo(expectedlog);
            identityUpdator.Dispose();
        }
        #endregion
    }
}
