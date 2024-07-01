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
        private Helpers.Desktop _desktop;
        private Domain.Entities.Desktop Desktop;
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
        [Given(@"There is an active Desktop existing")]
        public async Task GivenThereIsAnActiveDesktopExisting()
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
    }
}
