using Bright.ScreenPlay.Actors;
using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using CMDB.UI.Specflow.Actors;
using CMDB.UI.Specflow.Questions;
using CMDB.UI.Specflow.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

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
        private string updatedfield, newvalue, reason, expectedlog;

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
            var log = identityCreator.LastLogLine;
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
            this.newvalue = newValue;
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
                    overviewPage.Search(Identity.FirstName);
                    break;
                case "Company":
                case "UserID":
                case "Email":
                    overviewPage.Search(Identity.FirstName);
                    break;
            }
            var log = identityUpdator.LastLogLine;
            expectedlog = identityUpdator.ExpectedLog;
            log.Should().BeEquivalentTo(expectedlog);
            identityUpdator.Dispose();
        }
        #endregion
    }
}
