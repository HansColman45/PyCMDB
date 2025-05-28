using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.Monitors;
using CMDB.UI.Specflow.Questions;
using TechTalk.SpecFlow.Assist;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class MonitorStepDefinitions :  TestBase
    {
        private MonitorCreator monitorCreator;
        private MonitorUpdator monitorUpdator;
        private MonitorIdentityActor monitorIdentityActor;

        private Helpers.Monitor monitor;
        private Screen Monitor;
        private Identity Identity;
        private Kensington kensington;
        public MonitorStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }

        [Given(@"I want to create a monitor with the folowing details")]
        public async Task GivenIWantToCreateAMonitorWithTheFolowingDetails(Table table)
        {
            monitor = table.CreateInstance<Helpers.Monitor>();
            monitorCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(monitorCreator);
            Admin = await monitorCreator.CreateNewAdmin();
            monitorCreator.DoLogin(Admin.Account.UserID, "1234");
            bool result = monitorCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            monitorCreator.OpenMonitorOverviewPage();
        }
        [When(@"I save the monitor")]
        public async Task WhenISaveTheMonitor()
        {
            await monitorCreator.CreateNewMonitor(monitor);
        }
        [Then(@"The monitor can be found")]
        public void ThenTheMonitorCanBeFound()
        {
            monitorCreator.SearchMonitor(monitor);
            var lastLog = monitorCreator.GetLastMonitorLogLine;
            monitorCreator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        
        [Given(@"There is an monitor existing")]
        public async Task GivenThereIsAnMonitorExisting()
        {
            monitorUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(monitorUpdator);
            Admin = await monitorUpdator.CreateNewAdmin();
            Monitor = await monitorUpdator.CreateNewMonitor();
            log.Info($"Monitor created with SerialNumber {Monitor.SerialNumber}");
            monitorUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = monitorUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            monitorUpdator.OpenMonitorOverviewPage();
            monitorUpdator.Search(Monitor.AssetTag);
        }
        #region Update Monitor
        [When(@"I update the (.*) with (.*) on my monitor and I save")]
        public async Task WhenIUpdateTheSerialNumberWithOnMyMonitorAndISave(string field, string value)
        {
            Monitor = await monitorUpdator.UpdateMonitor(Monitor, field, value);
        }
        [Then(@"The monitor is saved")]
        public void ThenTheMonitorIsSaved()
        {
            monitorUpdator.Search(Monitor.AssetTag);
            string lastLogLine = monitorUpdator.GetLastMonitorLogLine;
            lastLogLine.Should().BeEquivalentTo(monitorUpdator.ExpectedLog);
        }
        #endregion
        #region Deactivate Monitor
        [When(@"I deactivate the monotor with reason (.*)")]
        public void WhenIDeactivateTheMonotorWithReasonTest(string reason)
        {
            monitorUpdator.DeactivateMonitor(Monitor, reason);
        }
        [Then(@"The monitor is deactivated")]
        public void ThenTheMonitorIsDeactivated()
        {
            monitorUpdator.Search(Monitor.AssetTag);
            string lastLogLine = monitorUpdator.GetLastMonitorLogLine;
            lastLogLine.Should().BeEquivalentTo(monitorUpdator.ExpectedLog);
        }
        #endregion
        #region Activate Monitor
        [Given(@"There is an inactive monitor existing")]
        public async Task GivenThereIsAnInactiveMonitorExisting()
        {
            monitorUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(monitorUpdator);
            Admin = await monitorUpdator.CreateNewAdmin();
            Monitor = await monitorUpdator.CreateNewMonitor(false);
            log.Info($"Monitor created with SerialNumber {Monitor.SerialNumber}");
            monitorUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = monitorUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            monitorUpdator.OpenMonitorOverviewPage();
            monitorUpdator.Search(Monitor.AssetTag);
        }
        [When(@"I activate the monitor")]
        public void WhenIActivateTheMonitor()
        {
            monitorUpdator.ActivateMonitor(Monitor);
        }
        [Then(@"The monitor is active")]
        public void ThenTheMonitorIsActive()
        {
            monitorUpdator.Search(Monitor.AssetTag);
            string lastLogLine = monitorUpdator.GetLastMonitorLogLine;
            lastLogLine.Should().BeEquivalentTo(monitorUpdator.ExpectedLog);
        }
        #endregion
        #region Assign and release
        [Given(@"There is an active monitor existing")]
        public async Task GivenThereIsAnActiveMonitorExisting()
        {
            monitorIdentityActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(monitorIdentityActor);
            Admin = await monitorIdentityActor.CreateNewAdmin();
            Monitor = await monitorIdentityActor.CreateNewMonitor();
            monitorIdentityActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = monitorIdentityActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            monitorIdentityActor.OpenMonitorOverviewPage();
            monitorIdentityActor.Search(Monitor.AssetTag);
        }

        [Given(@"The Identity to assign to my monitor is existing")]
        public async Task GivenTheIdentityToAssignToMyMonitorIsExisting()
        {
            Identity = await monitorIdentityActor.CreateNewIdentity();
        }

        [When(@"I assign the monitor to the Identity")]
        public void WhenIAssignTheMonitorToTheIdentity()
        {
            monitorIdentityActor.AssignTheIdentity2Monitor(Monitor,Identity);
        }
        [When(@"I fill in the assign form for my monitor")]
        public void WhenIFillInTheAssignFormForMyMonitor()
        {
            monitorIdentityActor.FillInAssignForm(Identity);
        }
        [Then(@"The Identity is assigned to the monitor")]
        public void ThenTheIdentityIsAssignedToTheMonitor()
        {
            monitorIdentityActor.Search(Monitor.AssetTag);
            var lastLogLine = monitorIdentityActor.GetLastMonitorLogLine;
            monitorIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }

        [Given(@"that Identity is assigned to my monitor")]
        public async Task GivenThatIdentityIsAssignedToMyMonitor()
        {
            await monitorIdentityActor.AssignIdentity(Monitor,Identity);
        }
        [When(@"I release that identity from my monitor and I fill in the release form")]
        public void WhenIReleaseThatIdentityFromMyMonitorAndIFillInTheReleaseForm()
        {
            monitorIdentityActor.ReleaseIdentity(Monitor,Identity);
        }
        [Then(@"The identity is released from my monitor")]
        public void ThenTheIdentityIsReleasedFromMyMonitor()
        {
            monitorIdentityActor.Search(Monitor.AssetTag);
            var lastLogLine = monitorIdentityActor.GetLastMonitorLogLine;
            monitorIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }
        #endregion
        #region Assign and release a Kensington
        [Given(@"A Key to assign to my monitor is existing in the system")]
        public async Task GivenAKeyToAssignToMyMonitorIsExistingInTheSystem()
        {
            kensington = await monitorIdentityActor.CreateKensington();
        }
        [When(@"I assign the Key to the monitor")]
        public void WhenIAssignTheKeyToTheMonitor()
        {
            monitorIdentityActor.DoAssignTheKey2Monitor(Monitor, kensington);
        }
        [Then(@"The Key is assigned to the monitor")]
        public void ThenTheKeyIsAssignedToTheMonitor()
        {
            monitorIdentityActor.Search(Monitor.AssetTag);
            var lastLogLine = monitorIdentityActor.GetLastMonitorLogLine;
            monitorIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }

        [Given(@"that Key is assigned to my monitor")]
        public async Task GivenThatKeyIsAssignedToMyMonitor()
        {
            await monitorIdentityActor.AssignKey(Monitor, kensington);
        }
        [When(@"I release the Key from my monitor and I fill in the release form")]
        public void WhenIReleaseTheKeyFromMyMonitorAndIFillInTheReleaseForm()
        {
            monitorIdentityActor.DoReleaseKey4Monitor(Monitor, kensington, Identity);
        }
        [Then(@"The Key is released from my monitor")]
        public void ThenTheKeyIsReleasedFromMyMonitor()
        {
            monitorIdentityActor.Search(Monitor.AssetTag);
            var lastLogLine = monitorIdentityActor.GetLastMonitorLogLine;
            monitorIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }
        #endregion
    }
}
