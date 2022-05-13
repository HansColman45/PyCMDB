using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using CMDB.Domain.Entities;
using CMDB.Testing.Helpers;
using FluentAssertions;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public class MonitorActionsStepDefinitions: TestBase
    {
        private LoginPage login;
        private MainPage main;
        private MonitorOverviewPage overviewPage;

        string expectedlog, newValue;
        Screen screen;

        public MonitorActionsStepDefinitions(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }

        [Given(@"There is an actives monitor existing")]
        public async Task GivenThereIsAnActivesMonitorExisting()
        {
            screen = await context.CreateMonitor(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.MonitorOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(screen.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I deactivate the monotor with reason (.*)")]
        public void WhenIDeactivateTheMonotorWithReasonTest(string reason)
        {
            newValue = reason;
            var deativate = overviewPage.Deactivate();
            deativate.Reason = newValue;
            deativate.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReasonEntered");
            deativate.Delete();
            deativate.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deleted");
        }
        [Then(@"The monitor is deactivated")]
        public void ThenTheMonitorIsDeactivated()
        {
            expectedlog = $"The Monitor with type {screen.Type} in table screen is deleted due to {newValue} by {admin.Account.UserID}";
            overviewPage.Search(screen.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log is not matching");
        }

        [Given(@"There is an inactive monitor existing")]
        public async Task GivenThereIsAnInactiveMonitorExisting()
        {
            screen = await context.CreateMonitor(admin, false);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.MonitorOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(screen.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I activate the monitor")]
        public void WhenIActivateTheMonitor()
        {
            overviewPage.Activate();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
        [Then(@"The monitor is active")]
        public void ThenTheMonitorIsActive()
        {
            expectedlog = $"The Monitor with type {screen.Type} in table screen is activated by {admin.Account.UserID}";
            overviewPage.Search(screen.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log is not matching");
        }
    }
}
