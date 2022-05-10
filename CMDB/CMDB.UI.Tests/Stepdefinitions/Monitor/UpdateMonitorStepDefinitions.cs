using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using System;
using TechTalk.SpecFlow;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using FluentAssertions;


namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public class UpdateMonitorStepDefinitions : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private MonitorOverviewPage overviewPage;

        private readonly Random rnd = new();
        private int rndNr;
        helpers.Monitor monitor;
        entity.Screen screen;
        string expectedlog, updatedField, newValue;

        public UpdateMonitorStepDefinitions(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }

        [Given(@"There is an monitor existing")]
        public void GivenTHereIsAnMonitorExisting()
        {

        }
        [When(@"I update the (.*) with (.*) on my monitor and I save")]
        public void WhenIUpdateTheSerialNumberWithOnMyMonitorAndISave(string field, string newValue)
        {

        }
        [Then(@"Then the monitor is saved")]
        public void ThenThenTheMonitorIsSaved()
        {

        }
    }
}
