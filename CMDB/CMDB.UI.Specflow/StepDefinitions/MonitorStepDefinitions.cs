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

        private Helpers.Monitor monitor;
        private Domain.Entities.Screen Monitor;
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
    }
}
