using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.PermissionActors;
using CMDB.UI.Specflow.Questions;
using CMDB.UI.Specflow.Questions.Permissions;
using Reqnroll;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class PermissionStepDefinitions: TestBase
    {   
        private Helpers.Permission permission;
        private Permission Permission;

        private PermissionCreator creator;
        private PermissionUpdator updator;
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

        [Given("There is a permission existing in the system")]
        public async Task GivenThereIsAPermissionExistingInTheSystem()
        {
            updator = new(ScenarioContext);
            ActorRegistry.RegisterActor(updator);
            Admin = await updator.CreateNewAdmin();
            updator.DoLogin(Admin.Account.UserID, "1234");
            bool result = updator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            Permission = await updator.Perform(new CreateThePermission());
            updator.OpenPermissionOverviewPage();
            updator.Search(Permission.Rights);
        }
        [When("I change the (.*) to (.*) for my permission and save")]
        public void WhenIChangeTheRightToUpdatedTestForMyPermissionAndSave(string field, string newValue)
        {
            Permission = updator.DoUpdatePermission(Permission, field, newValue);
        }
        [Then("I can see the changes done to my permission")]
        public void ThenICanSeeTheChangesDoneToMyPermission()
        {
            updator.Search(Permission.Rights);
            string lastLog = updator.LastLogLine();
            updator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #region delete
        [When("I delete the permission")]
        public void WhenIDeleteThePermission()
        {
            updator.DoDelete();
        }
        [Then("I can no longer find the permission in the system")]
        public void ThenICanNoLongerFindThePermissionInTheSystem()
        {
            updator.Search(Permission.Rights);
            int count = updator.CountSearchedElements();
            count.Should().Be(1);
        }

        #endregion
    }
}
