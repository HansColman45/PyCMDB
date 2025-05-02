using CMDB.UI.Specflow.Actors.Desktops;
using CMDB.UI.Specflow.Questions;
using TechTalk.SpecFlow.Assist;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class DesktopStepDefinitions: TestBase
    {
        private DesktopCreator _desktopCreator;
        private DesktopUpdator _desktopUpdator;
        private DesktopIdentityActor _desktopIdentityActor;

        private Helpers.Desktop _desktop;
        private Domain.Entities.Desktop Desktop;
        private Domain.Entities.Identity Identity;
        private Domain.Entities.Kensington Kensington;
        public DesktopStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }

        [Given(@"I want to create a new Desktop with these details")]
        public async Task GivenIWantToCreateANewDesktopWithTheseDetails(Table table)
        {
            _desktopCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(_desktopCreator);
            _desktop = table.CreateInstance<Helpers.Desktop>();
            Admin = await _desktopCreator.CreateNewAdmin();
            _desktopCreator.DoLogin(Admin.Account.UserID, "1234");
            var result = _desktopCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            _desktopCreator.OpenDesktopOverviewPage();
        }
        [When(@"I save the Desktop")]
        public async Task WhenISaveTheDesktop()
        {
            _desktop = await _desktopCreator.CreateNewDesktop(_desktop);
        }
        [Then(@"I can find the newly created Desktop back")]
        public void ThenICanFindTheNewlyCreatedDesktopBack()
        {
            _desktopCreator.SearchDesktop(_desktop);
            var lastlog = _desktopCreator.DesktopLastLogLine;
            _desktopCreator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #region Update Desktop
        [Given(@"There is an Desktop existing")]
        public async Task GivenThereIsAnDesktopExisting()
        {
            _desktopUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(_desktopUpdator);
            Admin = await _desktopUpdator.CreateNewAdmin();
            Desktop = await _desktopUpdator.CreateDesktop();
            _desktopUpdator.DoLogin(Admin.Account.UserID, "1234");
            var result = _desktopUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            _desktopUpdator.OpenDesktopOverviewPage();
            _desktopUpdator.Search(Desktop.AssetTag);
        }
        [When(@"I update the (.*) with (.*) on my Desktop and I save")]
        public void WhenIUpdateTheSerialnumberWithOnMyDesktopAndISave(string field, string value)
        {
            Desktop = _desktopUpdator.UpdateDesktop(Desktop, field, value);
        }
        [Then(@"The Desktop is saved")]
        public void ThenTheDesktopIsSaved()
        {
            _desktopUpdator.Search(Desktop.AssetTag);   
            var lastlog = _desktopUpdator.DesktopLastLogLine;
            _desktopUpdator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
        #region Deactivate Desktop
        [When(@"I deactivate the Desktop with reason (.*)")]
        public void WhenIDeactivateTheDesktopWithReasonTest(string reason)
        {
            _desktopUpdator.DeactivateDesktop(Desktop, reason);
        }
        [Then(@"The desktop is deactivated")]
        public void ThenTheDesktopIsDeactivated()
        {
            _desktopUpdator.Search(Desktop.AssetTag);
            var lastlog = _desktopUpdator.DesktopLastLogLine;
            _desktopUpdator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
        #region Activate Desktop
        [Given(@"There is an inactive Desktop existing")]
        public async Task GivenThereIsAnInactiveDesktopExisting()
        {
            _desktopUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(_desktopUpdator);
            Admin = await _desktopUpdator.CreateNewAdmin();
            Desktop = await _desktopUpdator.CreateDesktop(false);
            _desktopUpdator.DoLogin(Admin.Account.UserID, "1234");
            var result = _desktopUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            _desktopUpdator.OpenDesktopOverviewPage();
            _desktopUpdator.Search(Desktop.AssetTag);
        }
        [When(@"I activate the Desktop")]
        public void WhenIActivateTheDesktop()
        {
            _desktopUpdator.ActivateDesktop(Desktop);
        }
        [Then(@"The desktop is active")]
        public void ThenTheDesktopIsActive()
        {
            _desktopUpdator.Search(Desktop.AssetTag);
            var lastlog = _desktopUpdator.DesktopLastLogLine;
            _desktopUpdator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
        
        [Given(@"There is an active Desktop existing")]
        public async Task GivenThereIsAnActiveDesktopExisting()
        {
            _desktopIdentityActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(_desktopIdentityActor);
            Admin = await _desktopIdentityActor.CreateNewAdmin();
            Desktop = await _desktopIdentityActor.CreateDesktop();
            _desktopIdentityActor.DoLogin(Admin.Account.UserID, "1234");
            var result = _desktopIdentityActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            _desktopIdentityActor.OpenDesktopOverviewPage();
            _desktopIdentityActor.Search(Desktop.AssetTag);
        }
        [Given(@"the Identity exist as well")]
        public async Task GivenTheIdentityExistAsWell()
        {
            Identity = await _desktopIdentityActor.CreateNewIdentity();
        }
        #region Assign Identity
        [When(@"I assign the Desktop to the Identity")]
        public void WhenIAssignTheDesktopToTheIdentity()
        {
            _desktopIdentityActor.AssignTheIdentity2Desktop(Desktop, Identity);
        }
        [When(@"I fill in the assign form for my Desktop")]
        public void WhenIFillInTheAssignFormForMyDesktop()
        {
            _desktopIdentityActor.FillInAssignForm(Identity);
        }
        [Then(@"The Identity is assigned to the Desktop")]
        public void ThenTheIdentityIsAssignedToTheDesktop()
        {
            _desktopIdentityActor.Search(Desktop.AssetTag);
            var lastlog = _desktopIdentityActor.DesktopLastLogLine;
            _desktopIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
        #region Release Identity
        [Given(@"that Identity is assigned to my Desktop")]
        public async Task GivenThatIdentityIsAssignedToMyDesktop()
        {
            await _desktopIdentityActor.AssignDesktop2Identity(Desktop, Identity);
        }
        [When(@"I release that identity from my Desktop and I fill in the release form")]
        public void WhenIReleaseThatIdentityFromMyDesktop()
        {
            _desktopIdentityActor.ReleaseIdentity(Desktop, Identity);
        }
        [Then(@"The identity is released from my Desktop")]
        public void ThenTheIdentityIsReleasedFromMyDesktop()
        {
            _desktopIdentityActor.Search(Desktop.AssetTag);
            var lastlog = _desktopIdentityActor.DesktopLastLogLine;
            _desktopIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
        #region Assign and release key
        [Given(@"A Key to assign to my desktop is existing in the system")]
        public async Task GivenAKeyToAssignToMyDesktopIsExistingInTheSystem()
        {
            Kensington = await _desktopIdentityActor.CreateKensington();
        }
        [When(@"I assign the Key to the Desktop")]
        public void WhenIAssignTheKeyToTheDesktop()
        {
            _desktopIdentityActor.DoAssignTheKey2Desktop(Desktop, Kensington);
        }
        [Then(@"The Key is assigned to the Desktop")]
        public void ThenTheKeyIsAssignedToTheDesktop()
        {
            _desktopIdentityActor.Search(Desktop.AssetTag);
            var lastlog = _desktopIdentityActor.DesktopLastLogLine;
            _desktopIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }

        [Given(@"that Key is assigned to my Desktop")]
        public async Task GivenThatKeyIsAssignedToMyDesktop()
        {
            await _desktopIdentityActor.AssignKey(Desktop, Kensington);
        }
        [When(@"I release the Key from my Desktop and I fill in the release form")]
        public void WhenIReleaseTheKeyFromMyDesktopAndIFillInTheReleaseForm()
        {
            _desktopIdentityActor.DoReleaseKey4Desktop(Desktop,Kensington,Identity);
        }
        [Then(@"The Key is released from my Desktop")]
        public void ThenTheKeyIsReleasedFromMyDesktop()
        {
            _desktopIdentityActor.Search(Desktop.AssetTag);
            var lastlog = _desktopIdentityActor.DesktopLastLogLine;
            _desktopIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
    }
}
