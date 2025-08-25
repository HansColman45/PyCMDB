using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.PermissionActors;
using CMDB.UI.Specflow.Questions;
using Reqnroll;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class RolePermissionStepDefinitions: TestBase
    {
        private Helpers.RolePerm rolePermission;
        private RolePerm RolePerm;
        private Menu menu;

        private RolePermissionCreator rolePermissionCreator;
        private RolePermissionUpdator rolePermissionUpdator;
        public RolePermissionStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry): base(scenarioContext, actorRegistry)
        {
        }
        [Given("I want to create a RolePermission as")]
        public async Task GivenIWantToCreateARolePermissionAs(DataTable dataTable)
        {
            rolePermission = dataTable.CreateInstance<Helpers.RolePerm>();
            rolePermissionCreator = new RolePermissionCreator(ScenarioContext);
            ActorRegistry.RegisterActor(rolePermissionCreator);
            Admin = await rolePermissionCreator.CreateNewAdmin();
            rolePermissionCreator.DoLogin(Admin.Account.UserID, "1234");
            bool result = rolePermissionCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            rolePermissionCreator.OpenRolePermissionOverviewPage();
        }
        [When("I save the RolePermission")]
        public async Task WhenISaveTheRolePermission()
        {
            await rolePermissionCreator.CreateNewRolePermission(rolePermission);
        }
        [Then("I should be able to find the created RolePermission in the system")]
        public void ThenIShouldBeAbleToFindTheCreatedRolePermissionInTheSystem()
        {
            rolePermissionCreator.Search(rolePermission.Menu);
            string lastlog = rolePermissionCreator.LastLogLine;
            rolePermissionCreator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }

        #region Udate
        [Given("There is a RolePermission existing in the system")]
        public async Task GivenThereIsARolePermissionExistingInTheSystem()
        {
            rolePermissionUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(rolePermissionUpdator);
            Admin = await rolePermissionUpdator.CreateNewAdmin();
            menu = await rolePermissionUpdator.GetOrCreateMenu("Test Menu");
            RolePerm = await rolePermissionUpdator.CreateRolePerm(menu);
            rolePermissionUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = rolePermissionUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            rolePermissionUpdator.OpenRolePermissionOverviewPage();
            rolePermissionUpdator.Search(menu.Label);
        }
        [When("I change the Level to (.*) and update the RolePermission")]
        public void WhenIChangeTheLevelToAndUpdateTheRolePermission(string level)
        {
            rolePermissionUpdator.DoUpdate(RolePerm, level);
        }
        [Then("I should be able to find the updated RolePermission in the system")]
        public void ThenIShouldBeAbleToFindTheUpdatedRolePermissionInTheSystem()
        {
            rolePermissionUpdator.Search(menu.Label);
            string lastlog = rolePermissionUpdator.LastLogLine;
            rolePermissionUpdator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
        #region delete
        [When("I delete the RolePermission")]
        public void WhenIDeleteTheRolePermission()
        {
            rolePermissionUpdator.DoDelete();
        }
        [Then("I should not be able to find the deleted RolePermission in the system")]
        public void ThenIShouldNotBeAbleToFindTheDeletedRolePermissionInTheSystem()
        {
            rolePermissionUpdator.Search(menu.Label);
            int result = rolePermissionUpdator.CountSearchedElements();
            result.Should().Be(1);
        }
        #endregion
    }
}
