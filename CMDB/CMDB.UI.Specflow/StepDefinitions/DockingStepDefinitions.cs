using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.Dockings;
using CMDB.UI.Specflow.Questions;
using TechTalk.SpecFlow.Assist;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class DockingStepDefinitions: TestBase
    {
        DockingCreator dockingCreator;
        DockingUpdator dockingUpdator;
        Helpers.DockingStation dockingStation;
        Docking Docking;
        public DockingStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }

        [Given(@"I want to create a new Dockingstation with these details")]
        public async Task GivenIWantToCreateANewDockingstationWithTheseDetails(Table table)
        {
            dockingCreator = new(ScenarioContext);
            dockingStation = table.CreateInstance<Helpers.DockingStation>();
            Admin = await dockingCreator.CreateNewAdmin();
            dockingCreator.DoLogin(Admin.Account.UserID, "1234");
            bool result = dockingCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            dockingCreator.OpenDockingOverviewPage();
        }
        [When(@"I save the Dockingstion")]
        public async Task WhenISaveTheDockingstion()
        {
            dockingStation = await dockingCreator.CreateNewDocking(dockingStation);
        }
        [Then(@"I can find the newly created Docking station")]
        public void ThenICanFindTheNewlyCreatedDockingStation()
        {
            dockingCreator.SearchDocking(dockingStation);
            var lastLog = dockingCreator.DockingLastLogLine;
            dockingCreator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #region Update Docking
        [Given(@"There is an Docking existing")]
        public async Task GivenThereIsAnDockingExisting()
        {
            dockingUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(dockingUpdator);
            Docking = await dockingUpdator.CreateNewDocking();
            Admin = await dockingUpdator.CreateNewAdmin();
            dockingUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = dockingUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            dockingUpdator.OpenDockingOverviewPage();
            dockingUpdator.Search(Docking.AssetTag);
        }
        [When(@"I update the (.*) with (.*) on my Doking and I save")]
        public async Task WhenIUpdateTheSerialNumberWithOnMyDokingAndISave(string field, string value)
        {
            Docking = await dockingUpdator.UpdateDocking(Docking, field, value);
        }
        [Then(@"Then The Docking is saved")]
        public void ThenThenTheDockingIsSaved()
        {
            dockingUpdator.Search(Docking.AssetTag);
            var lastLog = dockingUpdator.DockingLastLogLine;
            dockingUpdator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #endregion
        #region Deactivate Docking
        [Given(@"There is an active Docking existing")]
        public async Task GivenThereIsAnActiveDockingExisting()
        {
            dockingUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(dockingUpdator);
            Docking = await dockingUpdator.CreateNewDocking();
            Admin = await dockingUpdator.CreateNewAdmin();
            dockingUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = dockingUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            dockingUpdator.OpenDockingOverviewPage();
            dockingUpdator.Search(Docking.AssetTag);
        }
        [When(@"I deactivate the Docking with reason (.*)")]
        public void WhenIDeactivateTheDockingWithReasonTest(string reason)
        {
            dockingUpdator.DeactivateDocking(Docking, reason);
        }
        [Then(@"The Docking is deactivated")]
        public void ThenTheDockingIsDeactivated()
        {
            dockingUpdator.Search(Docking.AssetTag);
            var lastLog = dockingUpdator.DockingLastLogLine;
            dockingUpdator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #endregion
        #region Activate Docking
        [Given(@"There is an inactve Docking existing")]
        public async Task GivenThereIsAnInactveDockingExisting()
        {
            dockingUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(dockingUpdator);
            Docking = await dockingUpdator.CreateNewDocking(false);
            Admin = await dockingUpdator.CreateNewAdmin();
            dockingUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = dockingUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            dockingUpdator.OpenDockingOverviewPage();
            dockingUpdator.Search(Docking.AssetTag);
        }
        [When(@"I activate the docking station")]
        public void WhenIActivateTheDockingStation()
        {
            dockingUpdator.ActivateDocking(Docking);
        }
        [Then(@"The docking station is activated")]
        public void ThenTheDockingStationIsActivated()
        {
            dockingUpdator.Search(Docking.AssetTag);
            var lastLog = dockingUpdator.DockingLastLogLine;
            dockingUpdator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }

        #endregion
    }
}
