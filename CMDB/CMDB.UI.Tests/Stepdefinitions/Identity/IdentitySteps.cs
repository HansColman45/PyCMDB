using CMDB.UI.Tests.Helpers;
using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;
using Xunit.Extensions.Ordering;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding, Order(1)]
    public sealed class IdentitySteps : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private IdentityOverviewPage overviewPage;
        private CreateIdentityPage createIdentity;
        private UpdateIdentityPage updateIdentity;

        private readonly Random rnd = new();
        private int rndNr;
        private Identity iden;
        private string updatedfield, newvalue, reason;
        public IdentitySteps(ScenarioData scenarioData) : base(scenarioData)
        {
        }
        [Order(1)]
        [Given(@"I want to create an Identity with these details")]
        public void GivenIWantToCreateAnIdentityWithTheseDetails(Table table)
        {
            rndNr = rnd.Next();
            iden = table.CreateInstance<Identity>();
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.IdentityOverview();
            createIdentity = overviewPage.New();
            createIdentity.FirstName = iden.FirstName + rndNr.ToString();
            createIdentity.LastName = iden.LastName + rndNr.ToString();
            createIdentity.Email = iden.Email;
            createIdentity.Company = iden.Company;
            createIdentity.UserId = iden.UserId + rndNr.ToString();
            createIdentity.Type = iden.Type;
            createIdentity.Language = iden.Language;
        }
        [Order(2)]
        [When(@"I save")]
        public void WhenISave()
        {
            createIdentity.Create();
        }
        [Order(3)]
        [Then(@"I can find the newly created Identity back")]
        public void ThenICanFindTheNewlyCreatedIdenotyBack()
        {
            overviewPage.Search(iden.FirstName + rndNr.ToString());
            var detail = overviewPage.Detail();
            var log = detail.GetLastLog();
            string expectedlog = $"The Identity width name: {iden.FirstName + rndNr.ToString()}, {iden.LastName + rndNr.ToString()} is created by {admin.Account.UserID} in table identity";
            Assert.Equal(expectedlog, log);
        }
        [Order(4)]
        [Given(@"An Identity exisist in the system")]
        public void GivenAnIdentityExisistInTheSystem()
        {
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.IdentityOverview();
            overviewPage.Search("Test");
        }
        [Order(5)]
        [When(@"I want to update (.*) with (.*)")]
        public void WhenIWantToUpdateFirstNameWithTestje(string field, string value)
        {
            rndNr = rnd.Next();
            updateIdentity = overviewPage.Update();
            iden = new();
            iden.FirstName = updateIdentity.FirstName.Trim();
            iden.LastName = updateIdentity.LastName.Trim();
            iden.Company = updateIdentity.Company.Trim();
            iden.UserId = updateIdentity.UserId.Trim();
            iden.Email = updateIdentity.Email.Trim();
            switch (field)
            {
                case "FirstName":
                    updatedfield = field;
                    newvalue = value + rndNr.ToString();
                    updateIdentity.FirstName = newvalue;
                    break;
                case "LastName":
                    updatedfield = field;
                    newvalue = value + rndNr.ToString();
                    updateIdentity.LastName = newvalue;
                    break;
                case "Company":
                    updatedfield = field;
                    newvalue = value + rndNr.ToString();
                    updateIdentity.Company = newvalue;
                    break;
                case "UserID":
                    updatedfield = field;
                    newvalue = value + rndNr.ToString();
                    updateIdentity.UserId = newvalue;
                    break;
                case "Email":
                    updatedfield = field;
                    newvalue = value;
                    updateIdentity.Email = newvalue;
                    break;
            }
            updateIdentity.Update();
        }
        [Order(6)]
        [Then(@"The identity is updated")]
        public void ThenTheIdentityIsUpdated()
        {
            string expectedlog = "";
            switch (updatedfield)
            {
                case "FirstName":
                    expectedlog = $"The {updatedfield} in table identity has been changed from {iden.FirstName} to {newvalue} by {admin.Account.UserID}";
                    overviewPage.Search(newvalue);
                    break;
                case "LastName":
                    expectedlog = $"The {updatedfield} in table identity has been changed from {iden.LastName} to {newvalue} by {admin.Account.UserID}";
                    overviewPage.Search(newvalue);
                    break;
                case "Company":
                    expectedlog = $"The {updatedfield} in table identity has been changed from {iden.Company} to {newvalue} by {admin.Account.UserID}";
                    overviewPage.Search(iden.FirstName);
                    break;
                case "UserID":
                    expectedlog = $"The {updatedfield} in table identity has been changed from {iden.UserId} to {newvalue} by {admin.Account.UserID}";
                    overviewPage.Search(iden.FirstName);
                    break;
                case "Email":
                    expectedlog = $"The {updatedfield} in table identity has been changed from {iden.Email} to {newvalue} by {admin.Account.UserID}";
                    overviewPage.Search(iden.FirstName);
                    break;
            }
            var detail = overviewPage.Detail();
            var log = detail.GetLastLog();
            Assert.Equal(expectedlog, log);
        }
        [Order(7)]
        [Given(@"An acive Identity exisist in the system")]
        public void GivenAnAciveIdentityExisistInTheSystem()
        {
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.IdentityOverview();
            overviewPage.Search("Test");
        }
        [Order(8)]
        [When(@"I want to deactivete the identity whith the reason (.*)")]
        public void WhenIWantToDeactiveteTheIdentityWhithTheReasonTest(string reason)
        {
            this.reason = reason;
            var deactivatepage = overviewPage.Deactivate();
            deactivatepage.Reason = reason;
            deactivatepage.Delete();
        }
        [Order(9)]
        [Then(@"The Idenetity is inactive")]
        public void ThenTheIdenetityIsInactive()
        {
            overviewPage.Search("Test");
            var detail = overviewPage.Detail();
            int Id = detail.Id;
            CMDB.Domain.Entities.Identity iden = context.GetIdentity(Id);
            string expectedlog = $"The Identity width name: {iden.FirstName} , {iden.LastName} in table identity is deleted due to {reason} by {admin.Account.UserID}";
            var log = detail.GetLastLog();
            Assert.Equal(expectedlog, log);
        }
        [Order(10)]
        [Given(@"An inactive Identity exisist in the system")]
        public void GivenAnInactiveIdentityExisistInTheSystem()
        {
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.IdentityOverview();
            overviewPage.Search("Test");
        }
        [Order(11)]
        [When(@"I want to activate this identity")]
        public void WhenIWantToActivateThisIdentity()
        {
            overviewPage.Activate();
        }
        [Order(12)]
        [Then(@"The Identity is active")]
        public void ThenTheIdentityIsActive()
        {
            overviewPage.Search("Test");
            var detail = overviewPage.Detail();
            int Id = detail.Id;
            CMDB.Domain.Entities.Identity iden = context.GetIdentity(Id);
            string expectedlog = $"The Identity width name: {iden.FirstName} , {iden.LastName} in table identity is activated by {admin.Account.UserID}";
            var log = detail.GetLastLog();
            Assert.Equal(expectedlog, log);
        }

    }
}
