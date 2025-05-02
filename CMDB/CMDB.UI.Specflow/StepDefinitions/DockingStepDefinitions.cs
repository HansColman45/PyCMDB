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
        DockingIdentityActor dockingIdentityActor;

        Helpers.DockingStation dockingStation;
        Docking Docking;
        Identity Identity;
        Kensington Kensington;
        public DockingStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }

        [Given(@"I want to create a new Dockingstation with these details")]
        public async Task GivenIWantToCreateANewDockingstationWithTheseDetails(Table table)
        {
            dockingCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(dockingCreator);
            Admin = await dockingCreator.CreateNewAdmin();
            dockingStation = table.CreateInstance<Helpers.DockingStation>();
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
            Admin = await dockingUpdator.CreateNewAdmin();
            Docking = await dockingUpdator.CreateNewDocking();
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
            Admin = await dockingUpdator.CreateNewAdmin();
            Docking = await dockingUpdator.CreateNewDocking(false);
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
        [Given(@"There is an active Docking existing")]
        public async Task GivenThereIsAnActiveDockingExisting()
        {
            dockingIdentityActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(dockingIdentityActor);
            Admin = await dockingIdentityActor.CreateNewAdmin();
            Docking = await dockingIdentityActor.CreateNewDocking();
            dockingIdentityActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = dockingIdentityActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            dockingIdentityActor.OpenDockingOverviewPage();
            dockingIdentityActor.Search(Docking.AssetTag);
        }
        #region Assign Identity
        [Given(@"The Identity exist as well")]
        public async Task GivenTheIdentityExistAsWell()
        {
            Identity = await dockingIdentityActor.CreateNewIdentity(); 
        }
        [When(@"I assign the Docking to the Identity")]
        public void WhenIAssignTheDockingToTheIdentity()
        {
            dockingIdentityActor.DoAssignIdentityt2Docking(Identity, Docking);
        }
        [When(@"I fill in the assign form for my Docking")]
        public void WhenIFillInTheAssignFormForMyDocking()
        {
            dockingIdentityActor.FillInAssignForm(Identity);
        }
        [Then(@"The Identity is assigned to the Docking")]
        public void ThenTheIdentityIsAssignedToTheDocking()
        {
            dockingIdentityActor.Search(Docking.AssetTag);
            var lastLog = dockingIdentityActor.DockingLastLogLine;
            dockingIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #endregion
        #region Release Identity
        [Given(@"that Identity is assigned to my Docking")]
        public async Task GivenThatIdentityIsAssignedToMyDocking()
        {
            await dockingIdentityActor.AssignIdenity2Docking(Identity, Docking);
        }
        [When(@"I release the Identity from the Docking and I have filled in the release form")]
        public void WhenIReleaseTheIdentityFromTheDockingAndIHaveFilledInTheReleaseForm()
        {
            dockingIdentityActor.DoReleaseIdentityFromDocking(Identity, Docking);
        }
        [Then(@"The Identity is released from the Docking")]
        public void ThenTheIdentityIsReleasedFromTheDocking()
        {
            dockingIdentityActor.Search(Docking.AssetTag);
            var lastLog = dockingIdentityActor.DockingLastLogLine;
            dockingIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #endregion
        #region Assign and release Key
        [Given(@"A Key to assign to my Docking is existing in the system")]
        public async Task GivenAKeyToAssignToMyDockingIsExistingInTheSystem()
        {
            Kensington = await dockingIdentityActor.CreateKensington();
        }
        [When(@"I assign the Key to the Docking")]
        public void WhenIAssignTheKeyToTheDocking()
        {
            dockingIdentityActor.DoAssignTheKey2Docking(Docking, Kensington);
        }
        [Then(@"The Key is assigned to the Docking")]
        public void ThenTheKeyIsAssignedToTheDocking()
        {
            dockingIdentityActor.Search(Docking.AssetTag);
            var lastLog = dockingIdentityActor.DockingLastLogLine;
            dockingIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }

        [Given(@"that Key is assigned to my Docking")]
        public async Task GivenThatKeyIsAssignedToMyDocking()
        {
            await dockingIdentityActor.AssignKey(Docking, Kensington);
        }
        [When(@"I release the Key from my Docking and I fill in the release form")]
        public void WhenIReleaseTheKeyFromMyDockingAndIFillInTheReleaseForm()
        {
            dockingIdentityActor.DoReleaseKey4Docking(Docking, Kensington, Identity);
        }
        [Then(@"The Key is released from my Docking")]
        public void ThenTheKeyIsReleasedFromMyDocking()
        {
            dockingIdentityActor.Search(Docking.AssetTag);
            var lastLog = dockingIdentityActor.DockingLastLogLine;
            dockingIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #endregion
    }
}
