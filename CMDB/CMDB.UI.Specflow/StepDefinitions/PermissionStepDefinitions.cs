using CMDB.UI.Specflow.Actors.PermissionActors;
using CMDB.UI.Specflow.Questions;
using Reqnroll;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class PermissionStepDefinitions: TestBase
    {   
        private Helpers.Permission permission;
        private PermissionCreator creator;
        public PermissionStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry): base(scenarioContext, actorRegistry)
        {
        }

        [Given("I want to create a permission with the following details")]
        public async Task GivenIWantToCreateAPermissionWithTheFollowingDetails(DataTable dataTable)
        {
            permission = dataTable.CreateInstance<Helpers.Permission>();
            creator = new(ScenarioContext);
            ActorRegistry.RegisterActor(creator);
            Admin = await creator.CreateNewAdmin();
            creator.DoLogin(Admin.Account.UserID, "1234");
            bool result = creator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            creator.OpenPermissionOverviewPage();
        }
        [When("I create the permission")]
        public void WhenICreateThePermission()
        {
            creator.DoCreatePermission(permission);
        }
        [Then("I can find the newly created permission in the system")]
        public void ThenICanFindTheNewlyCreatedPermissionInTheSystem()
        {
            creator.SearchPermission(permission);
            string lastLog = creator.LastLogLine();
            creator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
    }
}
