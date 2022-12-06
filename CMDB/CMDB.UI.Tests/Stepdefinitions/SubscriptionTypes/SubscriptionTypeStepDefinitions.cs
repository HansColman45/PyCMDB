using CMDB.Testing.Helpers;
using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using entity = CMDB.Domain.Entities;
using helpers = CMDB.UI.Tests.Helpers;
using Table = TechTalk.SpecFlow.Table;

namespace CMDB.UI.Tests.Stepdefinitions.SubscriptionTypes
{
    [Binding]
    public class CreateSubscriptionTypeStepDefinitions: TestBase
    {
        private LoginPage login;
        private MainPage main;
        private SubscriptionTypeOverviewPage overviewPage;
        private CreateSubscriptionTypePage createPage;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.SubscriptionType SubscriptionType;
        private entity.SubscriptionType subscriptionType;
        string expectedlog, updatedField;

        public CreateSubscriptionTypeStepDefinitions(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }

        [Given(@"I want to create a Subscriptiontype with the folowing details")]
        public void GivenIWantToCreateASubscriptiontypeWithTheFolowingDetails(Table table)
        {
            SubscriptionType = table.CreateInstance<helpers.SubscriptionType>();
            rndNr = rnd.Next();
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.SubscriptionTypeOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            createPage = overviewPage.Create();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_New");
            createPage.Category = SubscriptionType.Category;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Category");
            createPage.Provider = SubscriptionType.Provider+rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Provider");
            createPage.Type = SubscriptionType.Type+rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
            createPage.Description = SubscriptionType.Description+rndNr.ToString();
        }
        [When(@"I save the subscriptiontype")]
        public void WhenISaveTheSubscriptiontype()
        {
            createPage.Create();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }
        [Then(@"I can find the newly create subscriptiontype back")]
        public void ThenICanFindTheNewlyCreateSubscriptiontypeBack()
        {
            expectedlog = $"The {SubscriptionType.Category} with {SubscriptionType.Provider}{rndNr} and {SubscriptionType.Type}{rndNr} is created by {admin.Account.UserID} in table subscriptiontype";
            overviewPage.Search(SubscriptionType.Type + rndNr.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }

        [Given(@"There is a subscription type existing")]
        public async Task GivenThereIsASubscriptionTypeExisting()
        {
            subscriptionType = await context.CreateSubscriptionType(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.SubscriptionTypeOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
        }
        [When(@"I update the (.*) and change it to (.*) and save the subscriptiontype")]
        public void WhenIUpdateTheTypeAndChangeItToValueAndSaveTheSubscriptiontype(string type, string value)
        {
            overviewPage.Search(subscriptionType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var editPage = overviewPage.Update();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Update");
            updatedField = type;
            switch (type)
            {
                case "provider":
                    editPage.Provider = value;
                    expectedlog = $"The {type} has been changed from {subscriptionType.Provider} to {value} by {admin.Account.UserID} in table subscriptiontype";
                    editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Provider");
                    break;
                case "type":
                    expectedlog = $"The {type} has been changed from {subscriptionType.Type} to {value} by {admin.Account.UserID} in table subscriptiontype";
                    editPage.Type = value;
                    editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
                    break;
                case "description":
                    expectedlog = $"The {type} has been changed from {subscriptionType.Description} to {value} by {admin.Account.UserID} in table subscriptiontype";
                    editPage.Description = value;
                    editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Description");
                    break;
            }
            editPage.Edit();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Updaeted");
        }
        [Then(@"I can see the changes in the subscription type")]
        public void ThenICanSeeTheChangesInTheSubscriptionType()
        {
            if(updatedField == "type")
                overviewPage.Search(subscriptionType.Description);
            else
                overviewPage.Search(subscriptionType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }

        [When(@"I deactivate the subscriptiontype with (.*)")]
        public void WhenIDeactivateTheSubscriptiontypeWithTest(string reason)
        {
            overviewPage.Search(subscriptionType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var deactivatePage = overviewPage.Deactivate();
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivate");
            deactivatePage.Reason = reason;
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_reason");
            deactivatePage.Delete();
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivated");
            expectedlog = $"The {subscriptionType.Category.Category} with {subscriptionType.Provider} and {subscriptionType.Type} is deleted due to {reason} by {admin.Account.UserID} in table subscriptiontype";
        }
        [Then(@"the subscriptiontype is deactivated")]
        public void ThenTheSubscriptiontypeIsDeactivated()
        {
            overviewPage.Search(subscriptionType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }

        [Given(@"There is an inactive subscriptiontype existing")]
        public async Task GivenThereIsAnInactiveSubscriptiontypeExisting()
        {
            subscriptionType = await context.CreateSubscriptionType(admin,false);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.SubscriptionTypeOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(subscriptionType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }

        [When(@"I activate the subscriptiontype")]
        public void WhenIActivateTheSubscriptiontype()
        {
            overviewPage.Activate();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
            expectedlog = $"The {subscriptionType.Category.Category} with {subscriptionType.Provider} and {subscriptionType.Type} is activated by {admin.Account.UserID} in table subscriptiontype";
        }

        [Then(@"the subscriptiontype is activated")]
        public void ThenTheSubscriptiontypeIsActivated()
        {
            overviewPage.Search(subscriptionType.Type);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }
    }
}
