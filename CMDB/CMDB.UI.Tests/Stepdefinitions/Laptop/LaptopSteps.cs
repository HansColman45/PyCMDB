using CMDB.UI.Tests.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace CMDB.UI.Tests.Stepdefinitions.Laptop
{
    [Binding]
    public sealed class LaptopSteps : TestBase
    {
        public LaptopSteps(ScenarioData scenarioData) : base(scenarioData)
        {
        }
        [Given(@"I want to create a new Laptop with these details")]
        public void GivenIWantToCreateANewLaptopWithTheseDetails(Table table)
        {
        }
        [When(@"I save the Laptop")]
        public void WhenISaveTheLaptop()
        {
        }
        [Then(@"I can find the newly created Laptop back")]
        public void ThenICanFindTheNewlyCreatedLaptopBack()
        {
        }

        [Given(@"There is an Laptop existing")]
        public void GivenThereIsAnLaptopExisting()
        {
        }
        [When(@"I update the (.*) with (.*) and I save")]
        public void WhenIUpdateTheSerialnumberWithAndISave(string field, string value)
        {
        }
        [Then(@"The Laptop is saved")]
        public void ThenTheLaptopIsSaved()
        {
        }

        [Given(@"There is an active Laptop existing")]
        public void GivenThereIsAnActiveLaptopExisting()
        {
        }
        [When(@"I deactivate the Laptop with reason Test")]
        public void WhenIDeactivateTheLaptopWithReasonTest()
        {
        }
        [Then(@"The laptop is deactivated")]
        public void ThenTheLaptopIsDeactivated()
        {
        }

        [Given(@"There is an inactive Laptop existing")]
        public void GivenThereIsAnInactiveLaptopExisting()
        {
        }
        [When(@"I activate the Laptop")]
        public void WhenIActivateTheLaptop()
        {
        }
        [Then(@"The laptop is active")]
        public void ThenTheLaptopIsActive()
        {
        }

    }
}
