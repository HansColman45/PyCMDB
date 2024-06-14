using Bright.ScreenPlay.Actors;
using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using CMDB.UI.Specflow.Actors;
using CMDB.UI.Specflow.Questions;
using CMDB.UI.Specflow.Tasks;
using TechTalk.SpecFlow.Assist;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class IdentityStepDefinitions: TestBase
    {
        private Helpers.Identity iden;
        private IdentityCreator identityCreator;
        private IdentityOverviewPage overviewPage;
        private CreateIdentityPage createIdentity;
        private string updatedfield, newvalue, reason, expectedlog;

        public IdentityStepDefinitions(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            identityCreator = new(scenarioContext);
        }

        [Given(@"I want to create an Identity with these details")]
        public async Task GivenIWantToCreateAnIdentityWithTheseDetails(Table table)
        {
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
            var log = identityCreator.GetLastLogLine();
            expectedlog = identityCreator.ExpectedLog;
            log.Should().BeEquivalentTo(expectedlog);
        }
    }
}
