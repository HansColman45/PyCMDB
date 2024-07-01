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
        public void GivenThereIsAnDockingExisting()
        {
            throw new PendingStepException();
        }
        [When(@"I update the (.*) with (.*) on my Doking and I save")]
        public void WhenIUpdateTheSerialNumberWithOnMyDokingAndISave(string field, string value)
        {
            throw new PendingStepException();
        }

        [Then(@"Then The Docking is saved")]
        public void ThenThenTheDockingIsSaved()
        {
            throw new PendingStepException();
        }

        #endregion
    }
}
