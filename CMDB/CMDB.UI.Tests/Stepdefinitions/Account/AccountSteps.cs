using CMDB.UI.Tests.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace CMDB.UI.Tests.Stepdefinitions.Account
{
    [Binding]
    public sealed class AccountSteps : TestBase
    {
        public AccountSteps(ScenarioData scenarioData) : base(scenarioData)
        {
        }
        [Given(@"I want to create a Account with the following details")]
        public void GivenIWantToCreateAAccountWithTheFollowingDetails(Table table)
        {
        }
        [When(@"I save the account")]
        public void WhenISaveTheAccount()
        {
        }
        [Then(@"The account is saved")]
        public void ThenTheAccountIsSaved()
        {
        }

        [Given(@"There is an account existing")]
        public void GivenThereIsAnAccountExisting()
        {
        }
        [When(@"I change the (.*) to (.*) and I save the changes")]
        public void WhenIChangeTheUserIdToTestjeAndISaveTheChanges(string field, string newValue)
        {
        }
        [Then(@"The changes in account are saved")]
        public void ThenTheChangesInAccountAreSaved()
        {
        }

        [Given(@"There is an active account existing")]
        public void GivenThereIsAnActiveAccountExisting()
        {
        }
        [When(@"I deactivate the account with reason Test")]
        public void WhenIDeactivateTheAccountWithReasonTest()
        {
        }
        [Then(@"the account is inactive")]
        public void ThenTheAccountIsInactive()
        {
        }

        [Given(@"There is an inactive account existing")]
        public void GivenThereIsAnInactiveAccountExisting()
        {
        }
        [When(@"I activate the account")]
        public void WhenIActivateTheAccount()
        {
        }
        [Then(@"The account is active")]
        public void ThenTheAccountIsActive()
        {
        }

    }
}
